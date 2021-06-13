using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{

    public Animator transition;
    public float transitionTime;
    public Image Cyan;
    public Image Magenta;
    public Image Yellow;

    public Sprite gsCyan;
    public Sprite gsMagenta;
    public Sprite gsYellow;

    public Sprite sCyan;
    public Sprite sMagenta;
    public Sprite sYellow;

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
        if (!GameController.Instance.crystals[1])
        {
            Cyan.sprite = sCyan;
        } else
        {
            Cyan.sprite = gsCyan;
        }

        if (!GameController.Instance.crystals[0])
        {
            Yellow.sprite = sYellow;
        }
        else
        {
            Yellow.sprite = gsYellow;
        }

        if (!GameController.Instance.crystals[2])
        {
            Magenta.sprite = sMagenta;
        }
        else
        {
            Magenta.sprite = gsMagenta;
        }

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
