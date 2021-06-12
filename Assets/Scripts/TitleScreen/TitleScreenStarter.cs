using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenStarter : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //TODO transition
            SceneManager.LoadScene("MainGameplayLevel");
        }
    }
}
