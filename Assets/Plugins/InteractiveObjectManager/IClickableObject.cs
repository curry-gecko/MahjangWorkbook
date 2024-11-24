using UnityEngine;

namespace InteractiveObjectManager
{
    public interface IClickableObject
    {
        void OnMouseClick();
        void OnMouseDragging();
        void OnMouseRelease();
        void OnMouseOnObject();
        // 
        bool Draggable { get; }
        GameObject Me { get; }
        string Tag { get; }
    }
}