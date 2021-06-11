using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject worldRoot;

    //Prefabs
    public GameObject hallPrefab;
    public GameObject roomPrefab;

    //Tuning Varaibles
    [Header("Tuning Varaibles")]
    public int tileSize = 8;
    public int gameplayWidth = 16, gameplayHeight = 16;
    public int minWinDistanceFromWall = 1;
    public int maxWinDistanceFromWall = 2;

    // Start is called before the first frame update
    void Start()
    {
        DoWorldGeneration();
        GeneratePrefabs();
    }

    PreGenArea[,] world;

    private void DoWorldGeneration()
    {
        world = new PreGenArea[gameplayWidth, gameplayHeight];

        //Initialize
        for(int x = 0; x < gameplayWidth; x++)
        {
            for(int y = 0; y < gameplayHeight; y++)
            {
                world[x, y] = new PreGenArea()
                {
                    type = RoomType.HALLWAY,
                    connectedUp = true,
                    connectedDown = true,
                    connectedLeft = true,
                    connectedRight = true
                };
            }
        }

        //Step 1: Place special rooms
        PlaceWinRoom();
    }

    private void PlaceWinRoom()
    {
        // Room 1, end room
        int wallGeneratedOn = Random.Range(0, 3); // 0 = north, 1 = west, 2 = south, 3 = east
        int fromWall = Random.Range(minWinDistanceFromWall, maxWinDistanceFromWall);
        int maxForWall = (wallGeneratedOn == 0 || wallGeneratedOn == 2) ? gameplayHeight : gameplayWidth;
        int alongWall = Random.Range(0, maxForWall);
        int startRoomX;
        int startRoomY;
        if (wallGeneratedOn == 0 || wallGeneratedOn == 2)
        {
            startRoomY = wallGeneratedOn == 2 ? fromWall : gameplayHeight - fromWall;
            startRoomX = alongWall;
        }
        else
        {
            startRoomX = wallGeneratedOn == 1 ? fromWall : gameplayHeight - fromWall;
            startRoomY = alongWall;
        }

        SetRoomData(startRoomX, startRoomY, RoomType.PLAYER_START, false, false, false, false);

        //Room 2, start room

    }

    private void GeneratePrefabs()
    {
        Transform rootTransform = worldRoot.transform;
        for (int x = 0; x < gameplayWidth; x++)
        {
            for(int y = 0; y < gameplayHeight; y++)
            {
                GameObject creating = GameObject.Instantiate(GetPrefabforRoomType(x, y), new Vector3(x * 8, y * 8, 0), Quaternion.identity, rootTransform);
                GeneratedArea area = creating.GetComponent<GeneratedArea>();
                PreGenArea pga = world[x, y];
                area.SetWallConfig(pga.connectedRight, pga.connectedUp, pga.connectedLeft, pga.connectedDown);
            }
        }
    }

    private GameObject GetPrefabforRoomType(int x, int y)
    {
        switch(world[x, y].type)
        {
            case RoomType.HALLWAY:
                return hallPrefab;
            case RoomType.PLAYER_START:
            case RoomType.VICTORY:
            case RoomType.CRYSTAL_1:
            case RoomType.CRYSTAL_2:
            case RoomType.CRYSTAL_3:
            case RoomType.ROOM:
                return roomPrefab;
            default:
                return hallPrefab;
        }
    }

    private void SetRoomData(int x, int y, RoomType type, bool north, bool south, bool east, bool west)
    {
        world[x, y].type = type;
        world[x, y].connectedUp = north;
        world[x, y].connectedDown = south;
        world[x, y].connectedRight = east;
        world[x, y].connectedLeft = west;
    }

    public struct PreGenArea
    {
        public RoomType type;

        public bool connectedUp;
        public bool connectedDown;
        public bool connectedLeft;
        public bool connectedRight;
    }

    public enum RoomType
    {
        HALLWAY, ROOM, PLAYER_START, VICTORY, CRYSTAL_1, CRYSTAL_2, CRYSTAL_3
    }
}
