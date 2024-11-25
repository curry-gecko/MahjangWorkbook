using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;
using UnityEngine.UIElements;

namespace InteractiveObjectManager
{

    public class ClickEventManager : MonoBehaviour
    {
        private IClickableObject currentDraggingObject = null;

        void Start()
        {
            // UpdateAsObservableを使って毎フレームのクリックイベントをチェック
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ => DoMouseClick())
                .AddTo(this);

            // マウスエンターの監視
            this.UpdateAsObservable()
                .Where(_ => !Input.GetMouseButtonDown(0))
                .Subscribe(_ => DoMouseOnObject())
                .AddTo(this);

            // マウスリリースの監視
            this.UpdateAsObservable()
                .Where(_ => currentDraggingObject != null)
                .Where(_ => Input.GetMouseButtonUp(0))
                .Subscribe(_ => DoMouseOnRelease())
                .AddTo(this);

            // マウスドラッグの監視
            this.UpdateAsObservable()
                .Where(_ => currentDraggingObject != null)
                .Where(_ => Input.GetMouseButton(0))
                .Subscribe(_ => FollowMousePosition())
                .AddTo(this);
        }

        void DoMouseClick()
        {
            // マウス位置からのRayを作成
            RaycastHit2D[] hits = GetHits();

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.TryGetComponent<IClickableObject>(out var clickable))
                {

                    clickable.OnMouseClick();
                    // 優先順位の定義などあれば

                    // ドラッグできるオブジェクトであればドラッギング状態に格納する
                    if (currentDraggingObject == null && clickable.Draggable) { }
                    {
                        currentDraggingObject = clickable;
                    }
                    break;
                }
            }


        }

        void DoMouseOnObject()
        {

            // マウス位置からのRayを作成
            RaycastHit2D[] hits = GetHits();

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.TryGetComponent<IClickableObject>(out var clickable))
                {

                    clickable.OnMouseOnObject();
                    // 優先順位の定義などあれば
                    break;
                }
            }

        }

        // ドラッグ操作完了時に呼び出す
        void DoMouseOnRelease()
        {
            // マウス位置からのRayを作成
            RaycastHit2D[] hits = GetHits();

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.TryGetComponent<IClickableObject>(out var clickable))
                {
                    clickable.OnMouseRelease();
                }
            }

            // Release 後にオブジェクトが2つ以上存在し､ドラッグ操作をしていた場合
            if (hits.Length >= 2 && hits[0].collider.gameObject == currentDraggingObject.Me)
            {
                if (hits[1].collider.TryGetComponent<IClickableObject>(out var clickable))
                {
                    OnObjectReleasedEventManager.Instance.OnObjectsReleased.OnNext((currentDraggingObject, clickable));
                }
            }

        }

        // FixMe: 他と関数名違うの嫌だな
        void FollowMousePosition()
        {
            // currentDraggingObject をMousePositionへ追従させる
            Transform draggableObjectTransform = currentDraggingObject.Me.transform;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(draggableObjectTransform.position).z;
            draggableObjectTransform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }

        RaycastHit2D[] GetHits()
        {

            // マウス位置からのRayを作成
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray);

            hits = hits.OrderBy(hit => hit.distance).ToArray();
            return hits;
        }
    }

}