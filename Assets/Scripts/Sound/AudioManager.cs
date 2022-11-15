using UnityEditor.Audio;
using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        //DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
        
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }
    }

    private void Start()
    {
     //  Play("Theme");
    }
    public void PlayWithoutRep(string name)// [shitcode]
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
      
        if (s == null)
        {
            Debug.Log($"Sound \"{name}\" is not found!");
            return;
        }
        if (!s.source.isPlaying)//:(
            s.source.Play();
       
    }
    public void Play(string name)
    {
      Sound s =  Array.Find(sounds, sound => sound.name == name);
       
        if (s == null)
        {
            Debug.Log($"Sound \"{name}\" is not found!");
            return;
        }
            
        s.source.Play();   
        
    }
    public void PlayAFewTimes(string name, string name2, int times)
    {
        StartCoroutine(PlayTimes(name, name2, times));
    }
    IEnumerator PlayTimes(string name, string name2, int times)
    {
            yield return null;
            Sound s = Array.Find(sounds, sound => sound.name == name);
     
        Sound s2 = Array.Find(sounds, sound => sound.name == name2);
       
        for (int i = 0; i < times; i++)
            {
                s.source.Play();
                s2.source.Play();
            while (s.source.isPlaying)
                {
                    yield return null;
                }
            while (s2.source.isPlaying)
            {
                yield return null;
            }


        }
    }

}

