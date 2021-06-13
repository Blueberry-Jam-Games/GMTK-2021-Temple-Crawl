using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{

    public Animator transition;
    public float transitionTime;

    // Update is called once per frame
    void Update()
    {

    }

    public void LevelEnd(bool fadeIn)
    {
        StartCoroutine(Fade(fadeIn));
    }

    IEnumerator Fade(bool fadeIn)
    {
        if (fadeIn)
        {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
        }
        else
        {
            transition.SetTrigger("End");
            yield return new WaitForSeconds(0);
        }
        
    }
}
