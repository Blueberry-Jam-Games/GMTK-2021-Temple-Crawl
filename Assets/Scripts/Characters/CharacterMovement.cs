using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D charRigidbody;

    public float speedmultiplier = 3f;

    // Start is called before the first frame update
    void Start()
    {
        charRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal") * speedmultiplier;
        float moveY = Input.GetAxis("Vertical") * speedmultiplier;
        float newRotation = transform.rotation.eulerAngles.z;

        if (moveX > 0f && moveY == 0f)
        {
            newRotation = -90f;
        }
        else if(moveX < 0f && moveY == 0f)
        {
            newRotation = 90f;
        }
        else if(moveY > 0f && moveX == 0f)
        {
            newRotation = 0f;
        }
        else if(moveY < 0f && moveX == 0f)
        {
            newRotation = 180f;
        }
        else if(moveX > 0f && moveY > 0f)
        {
            newRotation = -45f;
        }
        else if(moveX > 0f && moveY < 0f)
        {
            newRotation = -135f;
        }
        else if(moveX < 0f && moveY > 0f)
        {
            newRotation = 45f;
        }
        else if(moveX < 0f && moveY < 0f)
        {
            newRotation = 135f;
        }

        charRigidbody.velocity = new Vector2(moveX, moveY);
        transform.rotation = Quaternion.Euler(0f, 0f, newRotation);
    }
}
