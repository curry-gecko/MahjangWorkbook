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
                    float yPosition = yPadding;
                    float zPosition = i * zPadding + 1;
                    Vector3 pos = new Vector3(xPosition, yPosition, zPosition);
                    tile.CurrentPositionTween = tile.transform.DOLocalMove(pos, 0.1f)
                            .OnComplete(() => tile.CurrentPositionTween = null);
                }
            }
        }
    }
}
