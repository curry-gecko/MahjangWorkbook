using System;
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
    }
}
