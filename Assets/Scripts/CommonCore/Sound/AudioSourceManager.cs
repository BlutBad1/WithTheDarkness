using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{

    public AudioSource AudioSource;
   public void PlayAudioSoure()
    {
        if (!AudioSource.isPlaying)
            AudioSource.Play();
    }
    public void PlayDelayedAudioSoure(float delay)
    {
        AudioSource.PlayDelayed(delay);
    }
    public void PlayScheduledAudioSoure(double time)
    {
        AudioSource.PlayScheduled(time);
    }
    public void StopAudioSoure()
    {
        AudioSource.Stop();
    }
}
