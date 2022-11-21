using System.Collections;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] allsounds;   //activeSounds
    public static AudioManager instance;
    
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in allsounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }    

    public Sound Play (string soundName)
    {
        Sound sound = Array.Find(allsounds, sound => sound.name == soundName);
        if (sound != null)
        {
            sound.source.Play();
            return sound;
        }
        else
        {
            //Debug.Log("не найден аудиоклип в менеджере " + soundName);
            return null;
        }
    }
    public void Stop (Sound _sound)
    {
        _sound.source.Stop();
    }
}
