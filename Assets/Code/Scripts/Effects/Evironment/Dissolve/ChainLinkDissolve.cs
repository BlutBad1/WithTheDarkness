using System.Collections.Generic;
using UnityEngine;
namespace EnvironmentEffects.MatEffect.Dissolve
{
    public class ChainLinkDissolve : Dissolve
    {
        public GameObject ChainRoot;
        protected List<GameObject> chainLinks;
        protected override void Start()
        {
            GameObject chainLink;
            chainLinks = new List<GameObject>();
            for (int i = 0; i < ChainRoot.transform.childCount; i++)
            {
                chainLink = ChainRoot.transform.GetChild(i).gameObject;
                chainLinks.Add(chainLink);
                meshRenderers.Add(chainLink.GetComponent<MeshRenderer>());
            }
            base.Start();
        }
        private void Update()
        {
            if (currentDissolve == 1)
                ChainRoot.SetActive(false);
        }
        public override void StartDissolving(float dissolveTime)
        {
            foreach (var chainLink in chainLinks)
                chainLink.GetComponent<Rigidbody>().isKinematic = true;
            base.StartDissolving(dissolveTime);
        }
    }
}
