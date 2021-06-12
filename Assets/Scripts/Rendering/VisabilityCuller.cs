using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisabilityCuller : MonoBehaviour
{
    public GeneratedArea[,] rooms;

    public int width;
    public int height;

    public float cullDistance = 10f;
    public float restoreDistance = 8f;

    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (rooms != null)
        {
            Vector3 playerPos = player.transform.position;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (Vector3.Distance(playerPos, rooms[x, y].transform.position) > cullDistance)
                    {
                        rooms[x, y].gameObject.SetActive(false);
                    }
                    else if (Vector3.Distance(playerPos, rooms[x, y].transform.position) < restoreDistance)
                    {
                        rooms[x, y].gameObject.SetActive(true);
                    }

                }
            }
        }
    }
}
