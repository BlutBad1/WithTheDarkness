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
        if (!s.source.isPlaying)
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
    public void PlayAFewTimes(string[] names,  int times)
    {
        StartCoroutine(PlayTimes(names, times));
    }
  
    IEnumerator PlayTimes(string []names, int times)
    {
            yield return null;
        Sound[] currentSounds = new Sound[name.Length]; 
        for (int i = 0; i < names.Length; i++)
        {
            currentSounds[i] = Array.Find(sounds, sound => sound.name == names[i]);
            if (currentSounds[i] == null)
            {
                Debug.Log($"Sound \"{names[i]}\" is not found!");
            }
        }
     
           

       
        for (int i = 0; i < times; i++)
            {
            for (int j = 0; j < names.Length; j++)
            {
                currentSounds[j].source.Play();
            }
            for (int j = 0; j < names.Length; j++)
            {
                while (currentSounds[j].source.isPlaying)
                {
                    yield return null;
                }
            }

        }
    }

}

