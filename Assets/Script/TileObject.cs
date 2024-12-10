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

        private readonly ObjectTag objectTag = ObjectTag.TileObject;
        public override string Tag => objectTag.ToString();

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

        //
        public void SetTile(Tile _tile)
        {
            tile = _tile;
            textOnGameObject?.SetTextToDisplay(_tile.TileToString());
            gameObject.name = _tile.TileToString();
        }

        private void OnDestroy()
        {
            CurrentPositionTween.Kill();
        }
    }
}
