using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedHallway : GeneratedArea
{

    public GameObject wallNE;
    public GameObject wallSE;
    public GameObject wallSW;
    public GameObject wallNW;

    public override void SetWallConfig(DungeonGenerator.RoomType type, bool east, bool north, bool west, bool south)
    {
        wallEastEnabled = east;
        wallNorthEnabled = north;
        wallWestEnabled = west;
        wallSouthEnabled = south;
        wallConfigSet = true;
        this.type = type;
    }

    protected override void ApplyWallConfig()
    {
        wallEast.SetActive(false);
        wallNorth.SetActive(false);
        wallWest.SetActive(false);
        wallSouth.SetActive(false);
        wallNE.SetActive(false);
        wallNW.SetActive(false);
        wallSE.SetActive(false);
        wallSW.SetActive(false);

        if (!wallEastEnabled && !wallNorthEnabled && wallSouthEnabled && wallWestEnabled)
        {
            wallNE.SetActive(true);
        } else if(!wallEastEnabled && !wallSouthEnabled && wallNorthEnabled && wallWestEnabled)
        {
            wallSE.SetActive(true);
        } else if (!wallWestEnabled && !wallSouthEnabled && wallNorthEnabled && wallEastEnabled)
        {
            wallSW.SetActive(true);
        } else if (!wallWestEnabled && !wallNorthEnabled && wallSouthEnabled && wallEastEnabled)
        {
            wallNW.SetActive(true);
        } else
        {
            wallEast.SetActive(!wallEastEnabled);
            wallNorth.SetActive(!wallNorthEnabled);
            wallWest.SetActive(!wallWestEnabled);
            wallSouth.SetActive(!wallSouthEnabled);
        }


        wallConfigApplied = true;
    }
}
