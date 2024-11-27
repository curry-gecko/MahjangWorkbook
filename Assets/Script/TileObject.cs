using System;
using DG.Tweening;
using InteractiveObjectManager;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace CGC.App
{
    public class TileObject : DraggableObject
    {
        //
        [SerializeField]
        public Tile tile;
        private TextOnGameObject textOnGameObject;

        public override string Tag => "TileObject";

        //
        public Tween CurrentPositionTween = null;

        public override void OnMouseDragging()
        {
            // base.OnMouseDragging();
            return;
        }

        new void Start()
        {

            textOnGameObject = GetComponent<TextOnGameObject>();

            if (textOnGameObject != null)
            {
                textOnGameObject.SetTextToDisplay(tile.TileToString());
            }
        }


    }
}
