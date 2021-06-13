using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedHallway : GeneratedArea
{

    public GameObject wallNE;
    public GameObject wallSE;
    public GameObject wallSW;
    public GameObject wallNW;

    public GameObject trap;

    public Sprite YellowTrapH;
    public Sprite YellowTrapV;

    public Sprite CyanTrapH;
    public Sprite CyanTrapV;

    public Sprite MagentaTrapH;
    public Sprite MagentaTrapV;


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

        if (trapType > 0)
        {
            Debug.Log("Trap  " + transform.position.x / 8 + "  " + transform.position.y / 8);

            trap.SetActive(true);
            if (trapFacing == 0)
            {
                trap.transform.localPosition = new Vector3(0.04f, 1.97f, 0f);
                trap.transform.localScale = new Vector3(1, -1, 1);

                if (trapColour == 0)
                {
                    spriteRenderer.sprite = YellowTrapV;
                } else if (trapColour == 1)
                {
                    spriteRenderer.sprite = CyanTrapV;
                } else
                {
                    spriteRenderer.sprite = MagentaTrapV;
                }

            }
            else if (trapFacing == 1)
            {
                trap.transform.localPosition = new Vector3(0, -2, 0);
                if (trapColour == 0)
                {
                    spriteRenderer.sprite = YellowTrapV;
                }
                else if (trapColour == 1)
                {
                    spriteRenderer.sprite = CyanTrapV;
                }
                else
                {
                    spriteRenderer.sprite = MagentaTrapV;
                }
            }
            else if (trapFacing == 2)
            {
                trap.transform.localPosition = new Vector3(-2, 0, 0);
                if (trapColour == 0)
                {
                    spriteRenderer.sprite = YellowTrapH;
                }
                else if (trapColour == 1)
                {
                    spriteRenderer.sprite = CyanTrapH;
                }
                else
                {
                    spriteRenderer.sprite = MagentaTrapH;
                }
            }
            else if (trapFacing == 3)
            {
                trap.transform.localPosition = new Vector3(2.12f, 0, 0);
                trap.transform.rotation = new Quaternion(0, 0, 180, 0);

                if (trapColour == 0)
                {
                    spriteRenderer.sprite = YellowTrapH;
                }
                else if (trapColour == 1)
                {
                    spriteRenderer.sprite = CyanTrapH;
                }
                else
                {
                    spriteRenderer.sprite = MagentaTrapH;
                }
            }

        }
        
    }
}
