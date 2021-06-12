using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedRoom : GeneratedArea
{
    public GameObject crystalPrefabRef;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void ApplyWallConfig()
    {
        base.ApplyWallConfig();
        if(type == DungeonGenerator.RoomType.VICTORY || type == DungeonGenerator.RoomType.CRYSTAL_1 || type == DungeonGenerator.RoomType.CRYSTAL_2 || type == DungeonGenerator.RoomType.CRYSTAL_3)
        {
            GameObject newCrystal = GameObject.Instantiate(crystalPrefabRef);
            GameCrystal crystal = newCrystal.GetComponent<GameCrystal>();
            float randXOffset = Random.Range(0.5f, 5.5f) - 3;
            float randYOffset = Random.Range(0.5f, 5.5f) - 3;
            crystal.transform.position = new Vector3(transform.position.x + randXOffset, transform.position.y + randYOffset);
            crystal.SetupWithType(type);
        }
        else if(type == DungeonGenerator.RoomType.PLAYER_START)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = new Vector3(transform.position.x, transform.position.y);
        }
    }
}
