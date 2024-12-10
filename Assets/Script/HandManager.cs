using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;
using CGC.App.TileExtensions;
using System.Collections.ObjectModel;
using Unity.VisualScripting;

namespace CGC.App
{
    public class HandManager : MonoBehaviour
    {
        [SerializeField]
        private TileManager _tileManager;
        private List<TileObject> _tileObjects = new();

        private TileObject _tsumoTile;

        private ReactiveProperty<bool> _waitDiscard = new(false);
        private IDisposable _waitDiscardDisposable;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // UpdateHandLayout();
        }

        // 牌を加える処理
        public void AddTiles(GameObject receiveObject)
        {

            receiveObject.transform.SetParent(transform);
            if (receiveObject.TryGetComponent<TileObject>(out var tileObject))
            {
                _tileObjects.Add(tileObject);
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
                int index = _tileObjects.FindIndex(t => t.Me == targetTile.Me);
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
            _tileObjects.RemoveAt(index);
            _tileObjects.Add(_tsumoTile);
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
            _tileObjects.SortTileObjects();
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

        public List<Tile> GetTiles()
        {
            List<Tile> sortedTiles = _tileObjects.Select(obj => obj.tile).ToList();
            if (_tsumoTile != null)
            {
                sortedTiles.Add(_tsumoTile.tile);
            }
            sortedTiles.SortTiles();
            return sortedTiles;
        }
        public ReadOnlyCollection<TileObject> GetTileObjects()
        {
            List<TileObject> tileObjects = new(_tileObjects);
            if (_tsumoTile != null)
            {
                tileObjects.Add(_tsumoTile);
            };

            return tileObjects.AsReadOnly();
        }

        // この関数がbutton のOnclick に指定できない
        public void CheckWinningHand()
        {
            WinningHandChecker checker = new();
            bool isWinning = checker.CheckWinningHand(GetTiles());

            Debug.Log("tag" + ":" + isWinning);

        }
    }
}
