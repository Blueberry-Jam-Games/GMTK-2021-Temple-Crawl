using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicHandler : MonoBehaviour
{
    public AudioSource intro;
    public AudioSource loop;

    public bool continuePlaying;

    // Start is called before the first frame update
    void Start()
    {
        continuePlaying = true;
        //StartCoroutine(StartLoopLater());
    }

    private IEnumerator StartLoopLater()
    {
        intro.Play();
        yield return new WaitForSeconds(intro.clip.length - 0.05f);
        while (continuePlaying)
        {
            loop.Play();
            yield return new WaitForSeconds(loop.clip.length - 0.05f);
        }
    }

    public void StopMusic()
    {
        continuePlaying = false;
        if (intro.isPlaying)
        {
            intro.Stop();
        }
        if (loop.isPlaying)
        {
            loop.Stop(); 
        }
    }
}
