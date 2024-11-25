using System;
using DG.Tweening;
using InteractiveObjectManager;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace CGC.App
{
    [Serializable]
    public class TileObject : DraggableObject
    {
        //
        private Tile tile;

        public override string Tag => "TileObject";

        //
        public Tween CurrentPositionTween = null;

        public override void OnMouseDragging()
        {
            // base.OnMouseDragging();
            return;
        }

    }
}
