using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    public Image show;
    public Text dialogue;

    public Button nextBttn;

    public TutorialStep[] steps;

    public int currentStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        ConfigureAtCurrentStep();
        nextBttn.onClick.AddListener(OnContinueButtonClicked);
    }

    public void OnContinueButtonClicked()
    {
        currentStep++;
        if(currentStep >= steps.Length)
        {
            //Go to gameplay
        }
        else
        {
            ConfigureAtCurrentStep();
        }
    }

    private void ConfigureAtCurrentStep()
    {
        show.sprite = steps[currentStep].image;
        dialogue.text = steps[currentStep].text;
    }
}

[System.Serializable]
public class TutorialStep
{
    public Sprite image;
    public string text;
}
