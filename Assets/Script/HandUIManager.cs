using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Schema;
using DG.Tweening;
using UnityEngine;

namespace CGC.App
{
    public class HandUIManager : MonoBehaviour
    {
        [SerializeField]
        HandManager _handManager;
        private float xPadding = 1.2f;
        private float yPadding = 0.2f;
        private float zPadding = 1.0f;

        private int maxCount = 14;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateHandLayout();

        }
        // 手札のUI更新 
        protected void UpdateHandLayout()
        {
            if (_handManager == null)
            {
                return;
            }
            ReadOnlyCollection<TileObject> tileObjects = _handManager.GetTileObjects();
            //
            for (int i = 0; i < tileObjects.Count; i++)
            {
                TileObject tile = tileObjects[i];

                if (!tile.didStart)
                {
                    continue;
                }

                if (tile.IsDragging)
                {
                    SetTweenMoveTileOnDragging(tile);
                }
                else if (tile.CurrentPositionTween == null && !tile.CurrentPositionTween.IsActive())
                {
                    bool isTsumoTile = (maxCount - 1) == i;// TODO ツモ牌がかならず14番目とは限らない
                    Vector3 pos = CalculateTilePosition(i, isTsumoTile);
                    SetTweenMoveTile(tile, pos);
                }
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
        private Vector3 CalculateTilePosition(int index, bool isTsumoTile = false)
        {

            float xPosition = index * xPadding;
            if (isTsumoTile)
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
