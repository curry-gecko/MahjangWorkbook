using InteractiveObjectManager;
using UnityEngine;

namespace CGC.App
{
    public class DiscardObject : DroppableObject
    {

        private ObjectTag objectTag = ObjectTag.DiscardObject;
        public override string Tag => objectTag.ToString();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        new public void Start()
        {

        }

        // Update is called once per frame
        public void Update()
        {

        }
    }
}
