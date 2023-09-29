using EffectsNS.PlayerEffects;
using UnityEngine;
namespace PlayerScriptsNS
{
    public class HeadBob : ShakingTranformByPlayer
    {
        [SerializeField]
        private Camera playerCam;
        public bool CanUseHeadBob = true;
        protected override void Awake()
        {
            if (!playerCam)
                playerCam = GetComponent<Camera>();
            if (!whichTransformIsModifying)
                whichTransformIsModifying = playerCam.transform;
            defaultPosition = whichTransformIsModifying.localPosition;
        }
        protected override void Update()
        {
            if (CanUseHeadBob)
                base.Update();
        }
    }
}