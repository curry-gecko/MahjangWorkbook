using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace CGC.App
{
    public class HandManager : MonoBehaviour
    {
        public List<TileObject> tiles = new();
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

        // 牌を引く処理
        public void ReceiveTile(GameObject receiveObject)
        {
            receiveObject.transform.SetParent(transform);
            if (receiveObject.TryGetComponent<TileObject>(out var tileObject))
            {
                tiles.Add(tileObject);
            }
        }

        // 牌を切る処理
        public void DiscardTile()
        {

        }

        // 牌の自動整理
        public void SortTile()
        {

            // ソート基準: Suit → Number → 赤ドラ
            tiles.Sort((a, b) =>
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

        // 手札のUI更新 
        private void UpdateHandLayout()
        {
            //
            int countOfPending = 0;
            for (int i = 0; i < tiles.Count; i++)
            {
                TileObject tile = tiles[i];
                // countOfPending += tile.IsPending.Value ? 1 : 0;

                if (tile.IsDragging)
                {
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                    Vector3 newPos = Camera.main.ScreenToWorldPoint(mousePos);
                    // ドラッグ中は常に対象を追尾するTweenで上書きする
                    if (tile.CurrentPositionTween != null && tile.CurrentPositionTween.IsActive())
                    {
                        tile.CurrentPositionTween.Kill();
                    }
                    tile.CurrentPositionTween = tile.transform.DOMove(newPos, 0.1f)
                            .OnComplete(() => tile.CurrentPositionTween = null);
                }
                else if (tile.CurrentPositionTween == null && !tile.CurrentPositionTween.IsActive())
                {
                    // 手札に存在する状態
                    float xPosition = (i - countOfPending) * xPadding;
                    if (i == tiles.Count - 1)
                    {
                        xPosition += xPadding;
                    }
                    float yPosition = yPadding;
                    float zPosition = i * zPadding + 1;
                    Vector3 pos = new Vector3(xPosition, yPosition, zPosition);
                    if (tile.transform.position != pos && tile.CurrentPositionTween == null)
                    {
                        tile.CurrentPositionTween = tile.transform.DOLocalMove(pos, 0.1f)
                                .OnComplete(() => tile.CurrentPositionTween = null);
                    }
                }
            }
        }
    }
}
