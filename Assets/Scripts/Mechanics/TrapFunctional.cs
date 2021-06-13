using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFunctional : MonoBehaviour
{
    private TrapType trapType;
    private int trapColour;
    private int trapOrientation;

    private Material dynamicMaterialInstance;
    private GameController gcRef;

    public GameObject monsterPrefab;

    public GameObject rockwallComponent;

    private int trapState = 0; //0 is untripped, 1 is tripped, 2 is executed

    private void Start()
    {
        dynamicMaterialInstance = GetComponent<SpriteRenderer>().material;
        gcRef = GameController.Instance;
        rockwallComponent.SetActive(false);
    }

    private void Update()
    {
        try
        {
            dynamicMaterialInstance.SetFloat("Yellow", gcRef.GetColourBrightness(0) / 0.5f);
            dynamicMaterialInstance.SetFloat("Cyan", gcRef.GetColourBrightness(1) / 0.5f);
            dynamicMaterialInstance.SetFloat("Magenta", gcRef.GetColourBrightness(2) / 0.5f);
        }
        catch (Exception)
        {
            //pass
        }
        
        if(trapState == 1)
        {
            if (gcRef.DistanceFromPlayer(transform.position) > 2f)
            {
                rockwallComponent.SetActive(true);
                trapState = 2;
            }
        }
    }

    public void SetBasicInformation(int trapType, int trapColour, int trapOrientation)
    {
        this.trapType = trapType == 1 ? TrapType.MONSTER : TrapType.ROCKFALL;
        this.trapColour = trapColour;
        this.trapOrientation = trapOrientation;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Thing has left trap with id " + collision.tag);
        if (collision.CompareTag("Player"))
        {
            //Play Effect
            if (trapType == TrapType.MONSTER && trapState == 0 && false)
            {
                Vector3 monsterSpawn1;
                Vector3 monsterSpawn2;

                if (trapOrientation == 0 || trapOrientation == 1)
                {
                    monsterSpawn1 = transform.position + Vector3.right + Vector3.up * 0.25f;
                    monsterSpawn2 = transform.position - Vector3.right - Vector3.up * 0.25f;
                }
                else
                {
                    monsterSpawn1 = transform.position + Vector3.up + Vector3.right * 0.25f;
                    monsterSpawn2 = transform.position - Vector3.up - Vector3.right * 0.25f; ;
                }
                SpawnMonster(monsterSpawn1);
                SpawnMonster(monsterSpawn2);
                trapState = 2;
            }
            else if (trapType == TrapType.ROCKFALL && trapState == 0)
            {
                trapState = 1;
            }
        }
    }

    private void SpawnMonster(Vector3 position)
    {
        GameObject newMonster = GameObject.Instantiate(monsterPrefab, position, Quaternion.identity);
        EnemyCommon ec = newMonster.GetComponent<EnemyCommon>();
        ec.activeTargeting = true;
    }
}

public enum TrapType
{
    MONSTER, ROCKFALL
}
