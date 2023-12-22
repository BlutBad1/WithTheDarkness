using SoundNS;
using UnityEngine;

namespace EnvironmentEffects.MatEffect.Dissolve
{
    public class DissolveWithSound : Dissolve
    {
        [Header("Sounds")]
        public AudioSourcesManager DissolveSound;
        public AudioSourcesManager EmergeSound;
        public override void StartDissolving(float dissolveTime)
        {
            base.StartDissolving(dissolveTime);
            DissolveSound?.CreateNewAudioSourceAndPlay(DissolveSound?.GetRandomSound());
        }
        public override void StartEmerging(float dissolveTime)
        {
            base.StartEmerging(dissolveTime);
            EmergeSound?.CreateNewAudioSourceAndPlay(EmergeSound?.GetRandomSound());
        }
    }
}