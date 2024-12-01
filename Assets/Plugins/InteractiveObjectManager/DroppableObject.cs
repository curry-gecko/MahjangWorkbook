using UnityEngine;

namespace InteractiveObjectManager
{
    // Dropだけ対応するオブジェクト
    public abstract class DroppableObject : MonoBehaviour, IClickableObject
    {
        // interface
        public bool Draggable => false;
        public bool IsDragging { get; private set; }
        public GameObject Me => gameObject;
        abstract public string Tag { get; }

        bool IClickableObject.IsDragging => IsDragging;

        public void Start()
        {
        }

        public virtual void OnMouseClick()
        {
            IsDragging = false;
        }

        public virtual void OnMouseDragging()
        {
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