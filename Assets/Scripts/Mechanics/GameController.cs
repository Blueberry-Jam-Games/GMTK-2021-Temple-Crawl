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
    //Life HUD
    public HealthManagement hud;
    public float minSaturation = 0.05f;

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
            Instance = this;
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
        GameObject hudObj = GameObject.FindWithTag("HealthManager");
        hud = hudObj.GetComponent<HealthManagement>();
    }

    public void NotifyHudOfHealthChange(int newhealth)
    {
        hud.UpdateHealthDisplay(newhealth);
    }

    public float DistanceFromPlayer(Vector3 test)
    {
        return Vector3.Distance(test, player.transform.position);
    }

    public Vector3 GetPlayerPosition()
    {
        return player.transform.position;
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

    public float GetColourBrightness(int colourIndex)
    {
        if(player.claimedCrystals[colourIndex])
        {
            return 0.5f;
        }

        float linearDistance = Vector3.Distance(player.transform.position, crystals[colourIndex].transform.position);
        return -(0.5f-minSaturation) / 96f * linearDistance + minSaturation + 0.5f; //static value of normal saturation
    }

    public Color GetPlayerLightColor()
    {
        float brightness1 = Square(GetColourBrightness(0));
        Color c1 = cameraColours.GetColorAtKeyframe(0); //TODO get from camera
        float brightness2 = Square(GetColourBrightness(1));
        Color c2 = cameraColours.GetColorAtKeyframe(1); //TODO
        float brightness3 = Square(GetColourBrightness(2));
        Color c3 = cameraColours.GetColorAtKeyframe(2);

        //Average
        float red = Square(c1.r) * brightness1 + Square(c2.r) * brightness2 + Square(c3.r) * brightness3;
        float green = Square(c1.g) * brightness1 + Square(c2.g) * brightness2 + Square(c3.g) * brightness3;
        float blue = Square(c1.b) * brightness1 + Square(c2.b) * brightness2 + Square(c3.b) * brightness3;

        float totalDistance = brightness1 + brightness2 + brightness3;

        return new Color(red / totalDistance, green / totalDistance, blue / totalDistance);
    }

    public float GetPlayerLightRadius()
    {
        if(winCrystal != null)
        {
            float distance = Vector3.Distance(player.transform.position, winCrystal.transform.position);
            return Mathf.Lerp(4f, 1f, distance / 96);
        }
        else
        {
            return 1f;
        }
    }

    public float GetPlayerLightOuterRadius()
    {
        if (winCrystal != null)
        {
            float distance = Vector3.Distance(player.transform.position, winCrystal.transform.position);
            return Mathf.Lerp(10f, 6f, distance / 128);
        }
        else
        {
            return 6f;
        }
    }

    public float GetPlayerLightIntensity()
    {
        if (winCrystal != null)
        {
            float distance = Vector3.Distance(player.transform.position, winCrystal.transform.position);
            return Mathf.Lerp(1f, 0.7f, distance / 196);
        }
        else
        {
            return 0.5f;
        }
    }

    private float Square(float root)
    {
        return root * root;
    }
}
