using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public LevelLoader ll;

    public Image show;
    public Text dialogue;

    public Button nextBttn;

    public TutorialStep[] steps;

    public int currentStep = 0;

    public int MIN_TIME_ON_SCREEN = 60;
    private int TIME_ON_SCREEN = 0;

    // Start is called before the first frame update
    void Start()
    {
        ConfigureAtCurrentStep();
        nextBttn.onClick.AddListener(OnContinueButtonClicked);
    }

    private void Update()
    {
        TIME_ON_SCREEN++;
    }

    public void OnContinueButtonClicked()
    {
        if(TIME_ON_SCREEN > MIN_TIME_ON_SCREEN)
        {
            TIME_ON_SCREEN = 0;
            currentStep++;
            if (currentStep >= steps.Length)
            {
                //Go to gameplay
                ll.RequestLoadLevel(2);
            }
            else
            {
                ConfigureAtCurrentStep();
            }
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
