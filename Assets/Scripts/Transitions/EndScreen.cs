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
    public Image Win;

    public Sprite gsCyan;
    public Sprite gsMagenta;
    public Sprite gsYellow;

    public Sprite sCyan;
    public Sprite sMagenta;
    public Sprite sYellow;

    public Image[] loose;
    public Text[] looseT;
    public Text[] winT;

    public void LevelEnd(bool fadeIn, bool winCondition)
    {
        StartCoroutine(Fade(fadeIn, winCondition));
    }

    IEnumerator Fade(bool fadeIn, bool winCondition)
    {
        if (fadeIn)
        {
            if (winCondition)
            {
                Cyan.gameObject.SetActive(true);
                Magenta.gameObject.SetActive(true);
                Yellow.gameObject.SetActive(true);
                Win.gameObject.SetActive(true);

                foreach (Image i in loose)
                {
                    i.gameObject.SetActive(false);
                }

                foreach (Text i in looseT)
                {
                    i.gameObject.SetActive(false);
                }

                foreach (Text i in winT)
                {
                    i.gameObject.SetActive(true);
                }

                if (!GameController.Instance.crystals[1])
                {
                    Cyan.sprite = sCyan;
                }
                else
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
            }
            else
            {
                Cyan.gameObject.SetActive(false);
                Magenta.gameObject.SetActive(false);
                Yellow.gameObject.SetActive(false);
                Win.gameObject.SetActive(false);
                foreach(Image i in loose)
                {
                    i.gameObject.SetActive(true);
                }

                foreach (Text i in looseT)
                {
                    i.gameObject.SetActive(true);
                }

                foreach (Text i in winT)
                {
                    i.gameObject.SetActive(false);
                }

            }
        }
        

        if (fadeIn)
        {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
        }
        else
        {
            transition.SetTrigger("End");
            yield return new WaitForSeconds(1);
        }
        
    }
}
