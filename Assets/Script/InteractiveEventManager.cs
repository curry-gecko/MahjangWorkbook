using System;
using InteractiveObjectManager;
using UniRx;
using UnityEngine;

namespace CGC.App
{
    public class InteractiveEventManager : MonoBehaviour
    {
        [SerializeField] private HandManager _handManager;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            OnObjectReleasedEventManager em = OnObjectReleasedEventManager.Instance;
            em.OnObjectsReleased
                .Subscribe(objects => OnObjectsReleased(objects.Item1, objects.Item2))
                .AddTo(this);
        }

        private void OnObjectsReleased(IClickableObject item1, IClickableObject item2)
        {
            if (item1.Tag == ObjectTag.TileObject.ToString() && item2.Tag == ObjectTag.DiscardObject.ToString())
            {
                if (item1.Me.TryGetComponent<TileObject>(out TileObject tileObject))
                {
                    ReleasedTileToTrash(tileObject);
                }
                return;
            }
        }

        private void ReleasedTileToTrash(TileObject targetTile)
        {
            _handManager.DiscardTile(targetTile);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
