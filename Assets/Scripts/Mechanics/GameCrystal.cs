using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCrystal : MonoBehaviour
{
    public CrystalType type;

    public float colour;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameController = GameObject.FindWithTag("GameController");
        GameController gc = gameController.GetComponent<GameController>();
        gc.RegisterCrystal(this); //TODO count is not included here
    }

    public bool IsWin()
    {
        return type == CrystalType.C_WIN;
    }

    public int GetNumber()
    {
        switch (type)
        {
            case CrystalType.C_0:
                return 0;
            case CrystalType.C_1:
                return 1;
            case CrystalType.C_2:
                return 2;
            default:
                return -1;
        }
    }
}

public enum CrystalType
{
    C_0, C_1, C_2, C_WIN
}
