using UnityEngine;

namespace InteractiveObjectManager
{
    public abstract class DraggableObject : MonoBehaviour, IClickableObject
    {
        // interface
        public bool Draggable => true;
        private bool _isDragging = false;
        public GameObject Me => gameObject;
        abstract public string Tag { get; }

        bool IClickableObject.IsDragging => _isDragging;

        public void Start()
        {
        }

        public virtual void OnMouseClick()
        {
            _isDragging = true;
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
            _isDragging = false;
        }

    }

}