using System;
using InteractiveObjectManager;
using UnityEngine;

namespace CGC.App
{
    [Serializable]
    public class TileObject : MonoBehaviour, IClickableObject
    {
        private Tile tile;

        public bool Draggable => true;

        public GameObject Me => this.gameObject;

        public string Tag => "Tile";

        private void Start()
        {

        }

        public void OnMouseClick()
        {
            Debug.Log("tag" + ":" + "OnMouseClick");
        }

        public void OnMouseDragging()
        {
            Debug.Log("tag" + ":" + "OnMouseDragging");
        }

        public void OnMouseOnObject()
        {

            Debug.Log("tag" + ":" + "OnMouseOnObject");
        }

        public void OnMouseRelease()
        {
            Debug.Log("tag" + ":" + "OnMouseRelease");
        }
    }
}
