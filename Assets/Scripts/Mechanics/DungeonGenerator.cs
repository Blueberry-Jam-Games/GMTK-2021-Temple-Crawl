using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject worldRoot;

    //Prefabs
    public GameObject hallPrefab;
    public GameObject roomPrefab;

    public VisabilityCuller visabilityCuller;

    //Tuning Varaibles
    [Header("Tuning Varaibles")]
    public int tileSize = 8;
    public int gameplayWidth = 16, gameplayHeight = 16;
    public int minWinDistanceFromWall = 1;
    public int maxWinDistanceFromWall = 2;

    public float crystalMinDistance = 1f;
    public float crystalMaxDistance = 5f;

    public int minExtraRooms = 5;
    public int maxExtraRooms = 10;

    public int monsterRoomPercent = 70;
    public int monsterHallPercent = 1;

    private int startingRoomX;
    private int startingRoomY;

    public float percentRemoveConnection = 0.6f;

    private int visibleTiles;

    // Start is called before the first frame update
    void Start()
    {
        crystalMinDistance = gameplayWidth / 2f - 2f;
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
        PlaceCrystalRooms();
        PlaceExtraRooms();
        RemoveConnections();
        CheckConnections();
        CheckRooms();
        CheckMap();
        ExtraFeatures();
    }

    private void PlaceWinRoom()
    {
        // Room 1, end room
        int wallGeneratedOn = Random.Range(0, 3); // 0 = north, 1 = west, 2 = south, 3 = east
        int fromWall = Random.Range(minWinDistanceFromWall, maxWinDistanceFromWall);
        int maxForWall = (wallGeneratedOn == 0 || wallGeneratedOn == 2) ? gameplayHeight : gameplayWidth;
        int alongWall = Random.Range(0, maxForWall);
        int winRoomX;
        int winRoomY;
        if (wallGeneratedOn == 0 || wallGeneratedOn == 2)
        {
            winRoomY = wallGeneratedOn == 2 ? fromWall : gameplayHeight - fromWall;
            winRoomX = alongWall;
        }
        else
        {
            winRoomX = wallGeneratedOn == 1 ? fromWall : gameplayHeight - fromWall;
            winRoomY = alongWall;
        }

        SetRoomData(winRoomX, winRoomY, RoomType.VICTORY, false, false, false, false);

        //Room 2, start room
        int startRoomX = gameplayWidth - winRoomX - 1;
        int startRoomY = gameplayHeight - winRoomY - 1;
        SetRoomData(startRoomX, startRoomY, RoomType.PLAYER_START, false, false, false, false);
        startingRoomX = startRoomX;
        startingRoomY = startRoomY;
    }

    private void PlaceCrystalRooms()
    {
        float angle1 = Random.Range(0f, 120f) * Mathf.Deg2Rad;
        float angle2 = Random.Range(120f, 240f) * Mathf.Deg2Rad;
        float angle3 = Random.Range(240f, 360f) * Mathf.Deg2Rad;

        PlaceRoom(angle1, RoomType.CRYSTAL_1);
        PlaceRoom(angle2, RoomType.CRYSTAL_2);
        PlaceRoom(angle3, RoomType.CRYSTAL_3);

        //local function
        void PlaceRoom(float angle, RoomType number)
        {
            float distance = Random.Range(crystalMinDistance, crystalMaxDistance);
            float offsetX = Mathf.Cos(angle) * distance;
            float offsetY = Mathf.Sin(angle) * distance;

            int finalRoomX = gameplayWidth / 2 + Mathf.RoundToInt(offsetX);
            int finalRoomY = gameplayHeight / 2 + Mathf.RoundToInt(offsetY);

            //Debug.Log("Crystal xy " + finalRoomX + ", " + finalRoomY);

            SetRoomData(finalRoomX, finalRoomY, number, false, false, false, false);
        }
    }

    private void PlaceExtraRooms()
    {
        int count = Random.Range(minExtraRooms, maxExtraRooms);
        int roomsPlaced = 0;
        for (int i = 0; i < count || roomsPlaced < minExtraRooms; i++)
        {
            int randomX = Random.Range(0, gameplayWidth);
            int randomY = Random.Range(0, gameplayHeight);

            if(world[randomX, randomY].type == RoomType.HALLWAY)
            {
                SetRoomData(randomX, randomY, RoomType.ROOM, false, false, false, false);
                roomsPlaced++;
            }
        }
    }

    private void RemoveConnections()
    {
        for(int x = 0; x < gameplayWidth; x++)
        {
            for(int y = 0; y < gameplayHeight; y++)
            {
                if(world[x, y].type == RoomType.HALLWAY)
                {
                    world[x, y].connectedUp = Random.Range(0f, 1f) > percentRemoveConnection;
                    world[x, y].connectedDown = Random.Range(0f, 1f) > percentRemoveConnection;
                    world[x, y].connectedLeft = Random.Range(0f, 1f) > percentRemoveConnection;
                    world[x, y].connectedRight = Random.Range(0f, 1f) > percentRemoveConnection;
                }
            }
        }
    }

    private void CheckConnections()
    {
        for (int x = 0; x < gameplayWidth; x++)
        {
            for (int y = 0; y < gameplayHeight; y++)
            {
                //Check Up
                if (y + 1 < gameplayHeight)
                {
                    if (world[x, y + 1].connectedDown == true && world[x, y].connectedUp == false)
                    {
                        world[x, y].connectedUp = true;
                    }
                }

                //Check Down
                if (y - 1 >= 0)
                {
                    if (world[x, y - 1].connectedUp == true && world[x, y].connectedDown == false)
                    {
                        world[x, y].connectedDown = true;
                    }
                }

                //Check left
                if (x > 0)
                {
                    if (world[x - 1, y].connectedRight == true && world[x, y].connectedLeft == false)
                    {
                        world[x, y].connectedLeft = true;
                    }
                }

                //Check Right
                if (x + 1 < gameplayWidth)
                {
                    if (world[x + 1, y].connectedLeft == true && world[x, y].connectedRight == false)
                    {
                        world[x, y].connectedRight = true;
                    }
                }

                if(x == gameplayWidth - 1)
                {
                    world[x, y].connectedRight = false;
                }

                if (x == 0)
                {
                    world[x, y].connectedLeft = false;
                }

                if (y == gameplayHeight - 1)
                {
                    world[x, y].connectedUp = false;
                }

                if (y == 0)
                {
                    world[x, y].connectedDown = false;
                }
            }
        }
    }

    private void CheckRooms()
    {
        for (int x = 0; x < gameplayWidth; x++)
        {
            for (int y = 0; y < gameplayHeight; y++)
            {
                if (world[x, y].type != RoomType.HALLWAY) //world[x, y].type == RoomType.ROOM
                {
                    //Debug.Log("Room: " + ConnectionCount(x, y));
                    if(ConnectionCount(x, y) == 0)
                    {
                        if(y > 0)
                        {
                            world[x, y].connectedDown = true;
                            world[x, y - 1].connectedUp = true;
                        } else if (y + 1< gameplayHeight)
                        {
                            world[x, y].connectedUp = true;
                            world[x, y + 1].connectedDown = true;
                        }

                    }

                    while(ConnectionCount(x, y) > 1)
                    {
                        if (Random.Range(0, 100) >= 50)
                        {
                            if (world[x, y].connectedDown)
                            {
                                world[x, y].connectedDown = false;
                                world[x, y - 1].connectedUp = false;
                            }
                            else if (world[x, y].connectedUp)
                            {
                                world[x, y].connectedUp = false;
                                world[x, y + 1].connectedDown = false;
                            }
                        }
                        else
                        {
                            if (world[x, y].connectedLeft)
                            {
                                world[x, y].connectedLeft = false;
                                world[x - 1, y].connectedRight = false;
                            }
                            else if (world[x, y].connectedRight)
                            {
                                world[x, y].connectedRight = false;
                                world[x + 1, y].connectedLeft = false;
                            }
                        }
                    }

                }
            }
        }
    }

    private void CheckMap()
    {
        int x = startingRoomX;
        int y = startingRoomY;
        
        world[x,y].accessable = true;
        visibleTiles = 0;
        do
        {
            CheckMapRecursive(x, y);

            bool exitLoop = false;

            for (int checkX = 0; checkX < gameplayWidth; checkX++)
            {
                for (int checkY = 0; checkY < gameplayHeight; checkY++)
                {
                    if(world[checkX, checkY].accessable == true && checkY < gameplayHeight - 1 && world[checkX, checkY + 1].accessable == false && world[checkX, checkY + 1].type == RoomType.HALLWAY && world[checkX, checkY].type == RoomType.HALLWAY)
                    {
                        world[checkX, checkY].connectedUp = true;
                        world[checkX, checkY + 1].connectedDown = true;
                        x = checkX;
                        y = checkY + 1;
                        exitLoop = true;
                        break;
                    } 
                    else if (world[checkX, checkY].accessable == true && checkY > 0 && world[checkX, checkY - 1].accessable == false && world[checkX, checkY - 1].type == RoomType.HALLWAY && world[checkX, checkY].type == RoomType.HALLWAY)
                    {
                        world[checkX, checkY].connectedDown = true;
                        world[checkX, checkY - 1].connectedUp = true;
                        x = checkX;
                        y = checkY - 1;
                        exitLoop = true;
                        break;
                    }
                    else if (world[checkX, checkY].accessable == true && checkX > 0 && world[checkX - 1, checkY].accessable == false && world[checkX - 1, checkY].type == RoomType.HALLWAY && world[checkX, checkY].type == RoomType.HALLWAY)
                    {
                        world[checkX, checkY].connectedLeft = true;
                        world[checkX - 1, checkY].connectedRight = true;
                        x = checkX - 1;
                        y = checkY;
                        exitLoop = true;
                        break;
                    }
                    if (world[checkX, checkY].accessable == true && checkX < gameplayWidth - 1 && world[checkX + 1, checkY].accessable == false && world[checkX + 1, checkY].type == RoomType.HALLWAY && world[checkX, checkY].type == RoomType.HALLWAY)
                    {
                        world[checkX, checkY].connectedRight = true;
                        world[checkX + 1, checkY].connectedLeft = true;
                        x = checkX + 1;
                        y = checkY;
                        exitLoop = true;
                        break;
                    }
                }
                if (exitLoop)
                {
                    break;
                }
            }

        } while (visibleTiles < gameplayWidth * gameplayHeight);
    }

    private void ExtraFeatures()
    {
        for (int checkX = 0; checkX < gameplayWidth; checkX++)
        {
            for (int checkY = 0; checkY < gameplayHeight; checkY++)
            {
                if(world[checkX, checkY].type == RoomType.ROOM)
                {
                    if(Random.Range(0, 100) < monsterRoomPercent)
                    {
                        world[checkX, checkY].monstor = true;
                    }
                } else if(world[checkX, checkY].type == RoomType.HALLWAY)
                {
                    if (Random.Range(0, 100) < monsterHallPercent)
                    {
                        world[checkX, checkY].monstor = true;
                    }
                }

            }
        }
    }

    private void CheckMapRecursive(int x, int y)
    {
        bool[] connections = IsHallConnected(x, y);
        world[x, y].accessable = true;
        visibleTiles++;

        if (connections[0] && world[x, y + 1].accessable == false)
        {
            CheckMapRecursive(x, y + 1);
        }

        if (connections[1] && world[x, y - 1].accessable == false)
        {
            CheckMapRecursive(x, y - 1);
        }

        if (connections[2] && world[x - 1, y].accessable == false)
        {
            CheckMapRecursive(x - 1, y);
        }

        if (connections[3] && world[x + 1, y].accessable == false)
        {
            CheckMapRecursive(x + 1, y);
        }
    }

    private void GeneratePrefabs()
    {
        Transform rootTransform = worldRoot.transform;
        visabilityCuller.rooms = new GeneratedArea[gameplayWidth, gameplayHeight];
        visabilityCuller.width = gameplayWidth;
        visabilityCuller.height = gameplayHeight;

        for (int x = 0; x < gameplayWidth; x++)
        {
            for(int y = 0; y < gameplayHeight; y++)
            {
                GameObject creating = GameObject.Instantiate(GetPrefabforRoomType(x, y), new Vector3(x * 8, y * 8, 0), Quaternion.identity, rootTransform);
                GeneratedArea area = creating.GetComponent<GeneratedArea>();
                PreGenArea pga = world[x, y];
                area.SetWallConfig(pga.type, pga.connectedRight, pga.connectedUp, pga.connectedLeft, pga.connectedDown);
                area.monster = world[x, y].monstor;
                visabilityCuller.rooms[x, y] = area;
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
        //Debug.Log("Set room data " + type + " at " + x + ", " + y);

        world[x, y].type = type;
        world[x, y].connectedUp = north;
        world[x, y].connectedDown = south;
        world[x, y].connectedRight = east;
        world[x, y].connectedLeft = west;
    }

    private bool[] IsHallConnected(int x, int y)
    {

        bool[] output = new bool[] { false, false, false, false };
        //Check Up
        if (y + 1 < gameplayHeight)
        {
            output[0] = world[x, y + 1].connectedDown;
        }

        //Check Down
        if (y - 1 >= 0)
        {
            output[1] = world[x, y - 1].connectedUp;
        }

        //Check left
        if (x > 0)
        {
            output[2] = world[x - 1, y].connectedRight;
        }

        //Check Right
        if (x + 1 < gameplayWidth)
        {
            output[3] = world[x + 1, y].connectedLeft;
        }

        return output;
    }

    private int ConnectionCount(int x, int y)
    {
        int count = 0;
        if (world[x, y].connectedUp)
        {
            count++;
        }

        if (world[x, y].connectedDown)
        {
            count++;
        }

        if (world[x, y].connectedLeft)
        {
            count++;
        }

        if (world[x, y].connectedRight)
        {
            count++;
        }

        return count;
    }

    public struct PreGenArea
    {
        public RoomType type;

        public bool connectedUp;
        public bool connectedDown;
        public bool connectedLeft;
        public bool connectedRight;
        public bool accessable;
        public bool monstor;
    }

    public enum RoomType
    {
        HALLWAY, ROOM, PLAYER_START, VICTORY, CRYSTAL_1, CRYSTAL_2, CRYSTAL_3
    }
}
