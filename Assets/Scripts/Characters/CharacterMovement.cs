using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CharacterMovement : MonoBehaviour
{
    private readonly string attackAnim = "PlayerAttack";
    private readonly string idleAnim = "Player_Idle";

    private Rigidbody2D charRigidbody;

    public float speedmultiplier = 3f;

    public Light2D playerLight;

    private Vector2 newLocationStore;

    public bool[] claimedCrystals = new bool[] {false, false, false};

    private Animator playerAnimator;
    private bool isAttackingNow = false;

    // Start is called before the first frame update
    void Start()
    {
        charRigidbody = GetComponent<Rigidbody2D>();
        newLocationStore = new Vector2();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isAttackingNow)
            {
                Debug.Log("Playing attack");
                playerAnimator.Play(attackAnim);
                isAttackingNow = true;
                StartCoroutine(FreeAttackLater());
            }
        }
    }

    private IEnumerator FreeAttackLater()
    {
        yield return new WaitForSeconds(12f / 30f);
        isAttackingNow = false;
        playerAnimator.Play(idleAnim);
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
        newLocationStore.x = moveX;
        newLocationStore.y = moveY;
        charRigidbody.velocity = newLocationStore;
        transform.rotation = Quaternion.Euler(0f, 0f, newRotation);
    }

    private void LateUpdate()
    {
        Color newLightCol = GameController.Instance.GetPlayerLightColor();
        playerLight.color = newLightCol;
        playerLight.pointLightOuterRadius = GameController.Instance.GetPlayerLightRadius(); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("Crystal"))
        {
            GameCrystal crystalComponent = other.gameObject.GetComponent<GameCrystal>();
            if(crystalComponent.type == CrystalType.C_WIN)
            {
                //Win level
                Debug.Log("Win Win Win!!!");
            }
            else
            { 
                claimedCrystals[crystalComponent.GetNumber()] = true;
            }
        }
    }
}
