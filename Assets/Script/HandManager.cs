using System;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace CGC.App
{
    public class HandManager : MonoBehaviour
    {
        [SerializeField]
        private TileManager _tileManager;
        private List<TileObject> _tiles = new();
        private TileObject _tsumoTile;
        public int TilesCount => _tiles.Count;

        private ReactiveProperty<bool> _waitDiscard = new(false);
        private IDisposable _waitDiscardDisposable;

        public IReadOnlyReactiveProperty<bool> WaitDiscard => _waitDiscard;
        private float xPadding = 1.05f;
        private float yPadding = 0.2f;
        private float zPadding = 1.0f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateHandLayout();
        }

        // 牌を加える処理
        public void AddTiles(GameObject receiveObject)
        {

            receiveObject.transform.SetParent(transform);
            if (receiveObject.TryGetComponent<TileObject>(out var tileObject))
            {
                _tiles.Add(tileObject);
            }
        }
        // 牌を引く処理
        private void ReceiveTile(GameObject receiveObject)
        {
            if (_tsumoTile != null)
            {
                throw new InvalidOperationException("ツモ牌がすでに積まれています。");
            }

            receiveObject.transform.SetParent(transform);
            if (receiveObject.TryGetComponent<TileObject>(out var tileObject))
            {
                _tsumoTile = tileObject;
                _waitDiscard.Value = true;
            }
        }

        // 牌を切る処理
        public void DiscardTile(TileObject targetTile)
        {
            if (_tsumoTile == null)
            {
                Debug.LogError($"[DiscardTile] 牌を切ることができるタイミングではありません。: {_tsumoTile.tile}");
                return;
            }

            // ツモ切り
            if (_tsumoTile == targetTile)
            {
                Destroy(targetTile.gameObject);
                ResetTsumoTile();
                return;
            }
            else
            {
                // 手牌切り
                int index = _tiles.FindIndex(t => t.Me == targetTile.Me);
                //
                if (index < 0)
                {
                    Debug.LogError($"[DiscardTile] 指定された牌が見つかりません: {targetTile}");
                    return;
                }
                // 
                DiscardHand(index);
                Destroy(targetTile.gameObject);

                ResetTsumoTile();

                return;
            }
        }

        // 手牌を切った時の処理
        private void DiscardHand(int index)
        {
            _tiles.RemoveAt(index);
            _tiles.Add(_tsumoTile);
        }

        // ツモ牌を削除し
        private void ResetTsumoTile()
        {
            _tsumoTile = null;
            _waitDiscard.Value = false;
            SortTile();
        }

        // 牌の自動整理
        public void SortTile()
        {

            // ソート基準: Suit → Number → 赤ドラ
            _tiles.Sort((a, b) =>
            {
                // Suitで比較
                int suitComparison = a.tile.Suit.CompareTo(b.tile.Suit);
                if (suitComparison != 0)
                {
                    return suitComparison;
                }

                // Numberで比較
                int numberComparison = a.tile.Number.CompareTo(b.tile.Number);
                if (numberComparison != 0)
                {
                    return numberComparison;
                }

                // 赤ドラ
                return b.tile.IsRedDora.CompareTo(a.tile.IsRedDora);
            });
        }
        private bool CanExecuteTurn()
        {
            return _tsumoTile == null;
        }
        // 手番ロジック
        public void ExecuteTurn(Action onTurnEnd)
        {

            if (!CanExecuteTurn())
            {
                return;
            }

            try
            {
                // ツモ牌を取得
                ReceiveTile(_tileManager.DistributeTile());
            }
            catch (Exception ex)
            {
                Debug.LogError($"手番実行エラー: {ex.Message}");
            }
            // 購読の重複を禁止
            _waitDiscardDisposable?.Dispose();

            // 待機中に手番終了を監視し、手番終了時のコールバックを呼び出す
            _waitDiscardDisposable = _waitDiscard
                .Where(b => b == false)
                .First()
                .Subscribe(_ =>
                {
                    ResetTsumoTile();
                    onTurnEnd?.Invoke();
                });
        }

        // 手札のUI更新 
        private void UpdateHandLayout()
        {
            //
            for (int i = 0; i < _tiles.Count; i++)
            {
                TileObject tile = _tiles[i];

                if (tile.IsDragging)
                {
                    SetTweenMoveTileOnDragging(tile);
                }
                else if (tile.CurrentPositionTween == null && !tile.CurrentPositionTween.IsActive())
                {

                    Vector3 pos = CalculateTilePosition(i);
                    SetTweenMoveTile(tile, pos);
                }
            }

            // ツモ牌
            if (_tsumoTile != null)
            {

                if (_tsumoTile.IsDragging)
                {
                    SetTweenMoveTileOnDragging(_tsumoTile);
                }
                else if (_tsumoTile.CurrentPositionTween == null && !_tsumoTile.CurrentPositionTween.IsActive())
                {
                    Vector3 pos = CalculateTilePosition(_tiles.Count);
                    // ツモ牌はTween不要
                    // SetTweenMoveTile(_tsumoTile, pos);
                    _tsumoTile.transform.position = pos;
                }
                // 
            }
        }
        // タイルがドラッグ状態の動作を設定する
        private void SetTweenMoveTileOnDragging(TileObject targetTile)
        {

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            Vector3 newPos = Camera.main.ScreenToWorldPoint(mousePos);
            // ドラッグ中は常に対象を追尾するTweenで上書きする
            if (targetTile.CurrentPositionTween != null && targetTile.CurrentPositionTween.IsActive())
            {
                targetTile.CurrentPositionTween.Kill();
            }
            targetTile.CurrentPositionTween = targetTile.transform.DOMove(newPos, 0.1f)
                    .OnComplete(() => targetTile.CurrentPositionTween = null);
        }
        // 牌の順番に対する位置を取得する
        private Vector3 CalculateTilePosition(int index)
        {

            float xPosition = index * xPadding;
            if (index == _tiles.Count)
            {
                // ツモ牌の場合
                xPosition += xPadding;
            }
            float yPosition = yPadding;
            float zPosition = index * zPadding + 1;

            return new Vector3(xPosition, yPosition, zPosition);
        }

        // 牌が規定の位置に居ない場合、規定位置へ移動させる
        private void SetTweenMoveTile(TileObject targetTile, Vector3 assertPosition)
        {
            if (targetTile.transform.position != assertPosition && targetTile.CurrentPositionTween == null)
            {
                targetTile.CurrentPositionTween = targetTile.transform.DOLocalMove(assertPosition, 0.1f)
                        .OnComplete(() => targetTile.CurrentPositionTween = null);
            }
        }
    }
}
