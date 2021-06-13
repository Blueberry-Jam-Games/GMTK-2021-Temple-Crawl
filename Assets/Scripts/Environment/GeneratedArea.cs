using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneratedArea : MonoBehaviour
{
    public GameObject wallEast;
    public GameObject wallNorth;
    public GameObject wallWest;
    public GameObject wallSouth;

    protected bool wallConfigSet = false;
    protected bool wallConfigApplied = false;

    protected bool wallEastEnabled = false;
    protected bool wallNorthEnabled = false;
    protected bool wallWestEnabled = false;
    protected bool wallSouthEnabled = false;

    protected DungeonGenerator.RoomType type;
    public bool monster = false;

    public virtual void SetWallConfig(DungeonGenerator.RoomType type, bool east, bool north, bool west, bool south)
    {
        wallEastEnabled = east;
        wallNorthEnabled = north;
        wallWestEnabled = west;
        wallSouthEnabled = south;
        wallConfigSet = true;
        this.type = type;
    }

    public virtual void HasMonstor()
    {
        monster = true;
    }

    protected virtual void ApplyWallConfig()
    {
        wallEast.SetActive(!wallEastEnabled);
        wallNorth.SetActive(!wallNorthEnabled);
        wallWest.SetActive(!wallWestEnabled);
        wallSouth.SetActive(!wallSouthEnabled);
        wallConfigApplied = true;
    }

    protected virtual void Start()
    {
        if (wallConfigSet)
        {
            ApplyWallConfig();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (wallConfigSet && !wallConfigApplied)
        {
            ApplyWallConfig();
        }
    }
}
