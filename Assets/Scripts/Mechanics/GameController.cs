using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private bool destroyTag = false; //If this is true an instance already exists

    //Player
    protected CharacterMovement player;
    //List<Crystal> crystals
    protected GameCrystal[] crystals;
    //Win crystal
    protected GameCrystal winCrystal;
    //Camera
    protected ColourCurveControl cameraColours;
    //TODO HUD

    private void Awake()
    {
        //If an instance already exists quit and leave now.
        if (Instance != null)
        {
            destroyTag = true;
            return;
        }
        else
        {
            crystals = new GameCrystal[3];
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (destroyTag)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnNewSceneLoaded;
            CollectReferences();
        }
    }

    private void CollectReferences()
    {
        GameObject camera = GameObject.FindWithTag("MainCamera");
        cameraColours = camera.GetComponent<ColourCurveControl>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.GetComponent<CharacterMovement>();
    }

    public void RegisterCrystal(GameCrystal crystal)
    {
        if (crystal.IsWin())
        {
            winCrystal = crystal;
        }
        else
        {
            crystals[crystal.GetNumber()] = crystal;
        }
    }

    private void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MainGameplayLevel")
        {
            //Destroy all references
            //Reload references
            CollectReferences();
        }
        else
        {
            //If we are in a different scene unsubscribe and quit
            SceneManager.sceneLoaded -= OnNewSceneLoaded;
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
