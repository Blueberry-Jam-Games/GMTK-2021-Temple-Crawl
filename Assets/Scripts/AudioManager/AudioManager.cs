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
            Debug.Log("Playing sound " + name);
            if(s.loop)
            {
                if(s.source.isPlaying)
                {
                    Debug.Log("Did not play sound " + name + " because the source is already playing");
                }
                else
                {
                    Debug.Log("Starting loop " + name);
                    s.source.Play();
                }
            }
            else
            {
                Debug.Log("Unconditional play once " + name);
                s.source.Play();
            }
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        Debug.Log("Stopping sound " + name);
        if (s.source.isPlaying)
        {
            s.source.Stop();
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
    [HideInInspector]
    public AudioSource source;
    [Range(0f, 1f)]
    public float spatialize;
    public bool loop;
}
