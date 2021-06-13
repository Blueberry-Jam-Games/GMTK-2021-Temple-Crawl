using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = s.spatialize;
            s.source.playOnAwake = false;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogError("Sound " + name + " does not exist");
        }
        else
        {
            if(!s.source.isPlaying && s.loop)
            {
                s.source.Play();
            } else if (!s.loop)
            {
                s.source.Play();
            }
            
        }
    }
}

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public string name;
    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    [HideInInspector]
    public AudioSource source;
    [Range(0f, 1f)]
    public float spatialize;
    public bool loop;
}
