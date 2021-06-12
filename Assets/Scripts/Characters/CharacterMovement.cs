using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D charRigidbody;

    private Vector2 velocityStore = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        charRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        velocityStore.x = moveX;
        velocityStore.y = moveY;

        charRigidbody.velocity = velocityStore;
    }
}
