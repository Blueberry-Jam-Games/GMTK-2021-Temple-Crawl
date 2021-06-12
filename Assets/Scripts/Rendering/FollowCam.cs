using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public GameObject player;

    public float maxXoffset;
    public float maxYoffset;

    Vector3 replacementPostion;

    // Start is called before the first frame update
    void Start()
    {
        replacementPostion = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Debug.Log("Follow cam late update");
        float thisX = transform.position.x;
        float playerX = player.transform.position.x;
        float newX = thisX;

        float xOffset = thisX - playerX;
        //Debug.Log("Follow cam X " + thisX + " player " + playerX + " offset " + xOffset);

        if (Mathf.Abs(xOffset) > maxXoffset)
        {
            //Debug.Log("Far enough");
            if(thisX > playerX)
            {
                newX = playerX + maxXoffset;
            }
            else
            {
                newX = playerX - maxXoffset;
            }
        }
        //Debug.Log("New X is " + newX);

        float thisY = transform.position.y;
        float playerY = player.transform.position.y;
        float newY = thisY;

        float yOffset = thisY - playerY;
        if (Mathf.Abs(yOffset) > maxYoffset)
        {
            if (thisY > playerY)
            {
                newY = playerY + maxYoffset;
            }
            else
            {
                newY = playerY - maxYoffset;
            }
        }

        replacementPostion.Set(newX, newY, replacementPostion.z);

        transform.SetPositionAndRotation(replacementPostion, Quaternion.identity);
    }
}
