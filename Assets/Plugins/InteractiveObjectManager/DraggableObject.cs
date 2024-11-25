using UnityEngine;

namespace InteractiveObjectManager
{
    public abstract class DraggableObject : MonoBehaviour, IClickableObject
    {
        // interface
        public bool Draggable => true;
        public bool IsDragging { get; private set; }
        public GameObject Me => gameObject;
        abstract public string Tag { get; }

        bool IClickableObject.IsDragging => IsDragging;

        public void Start()
        {
        }

        public virtual void OnMouseClick()
        {
            IsDragging = true;
        }

        public virtual void OnMouseDragging()
        {
            // currentDraggingObject をMousePositionへ追従させる
            Transform draggableObjectTransform = Me.transform;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(draggableObjectTransform.position).z;
            draggableObjectTransform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }

        public virtual void OnMouseOnObject()
        {
            return;
        }

        public virtual void OnMouseRelease()
        {
            IsDragging = false;
        }

    }

}