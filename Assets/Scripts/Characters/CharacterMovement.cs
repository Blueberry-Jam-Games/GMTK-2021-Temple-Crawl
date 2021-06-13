using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CharacterMovement : MonoBehaviour

{
    public EndScreen EndScreen;
    public LevelLoader transition;

    private readonly string attackAnim = "PlayerAttack";
    private readonly string idleAnim = "Player_Idle";
    private readonly string moveAnim = "PlayerWalk";

    private Rigidbody2D charRigidbody;

    public float speedmultiplier = 3f;

    public Light2D playerLight;

    private Vector2 newLocationStore;

    public bool[] claimedCrystals = new bool[] {false, false, false};

    private Animator playerAnimator;
    private bool isAttackingNow = false;
    private string currentAnimation;

    private int health = 12;

    private AudioManager soundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        currentAnimation = idleAnim;
        charRigidbody = GetComponent<Rigidbody2D>();
        newLocationStore = new Vector2();
        playerAnimator = GetComponent<Animator>();
        soundPlayer = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (!GameController.Instance.endScreen)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!currentAnimation.Equals(attackAnim))
                {
                    Debug.Log("Playing attack");
                    playerAnimator.Play(attackAnim);
                    isAttackingNow = true;
                    UpdateAnimationState(attackAnim);
                    StartCoroutine(FreeAttackLater());
                    soundPlayer.Play("SwordNothing");
                }
            }
        } else
        {
            if (Input.anyKeyDown)
            {
                transition.reloadLevel();
                EndScreen.LevelEnd(false);
                GameController.Instance.endScreen = false;
            }
        }
    }

    private IEnumerator FreeAttackLater()
    {
        yield return new WaitForSeconds(12f / 30f);
        isAttackingNow = false;
        UpdateAnimationState(idleAnim);
        playerAnimator.Play(idleAnim);
    }

    private void UpdateAnimationState(string newstate)
    {
        if (!currentAnimation.Equals(newstate))
        {
            if(isAttackingNow)
            {
                //pass
            }
            else
            {
                playerAnimator.StopPlayback();
                playerAnimator.Play(newstate);
                currentAnimation = newstate;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameController.Instance.endScreen)
        {
            float moveX = Input.GetAxis("Horizontal") * speedmultiplier;
            float moveY = Input.GetAxis("Vertical") * speedmultiplier;
            float newRotation = transform.rotation.eulerAngles.z;

            if (moveX > 0f && moveY == 0f)
            {
                newRotation = -90f;
            }
            else if (moveX < 0f && moveY == 0f)
            {
                newRotation = 90f;
            }
            else if (moveY > 0f && moveX == 0f)
            {
                newRotation = 0f;
            }
            else if (moveY < 0f && moveX == 0f)
            {
                newRotation = 180f;
            }
            else if (moveX > 0f && moveY > 0f)
            {
                newRotation = -45f;
            }
            else if (moveX > 0f && moveY < 0f)
            {
                newRotation = -135f;
            }
            else if (moveX < 0f && moveY > 0f)
            {
                newRotation = 45f;
            }
            else if (moveX < 0f && moveY < 0f)
            {
                newRotation = 135f;
            }
            newLocationStore.x = moveX;
            newLocationStore.y = moveY;
            if(moveX != 0 || moveY != 0)
            {
                UpdateAnimationState(moveAnim);
                // Start walk hook
                soundPlayer.Play("Walking");
            }
            else
            {
                UpdateAnimationState(idleAnim);
                soundPlayer.Stop("Walking");
            }
            charRigidbody.velocity = newLocationStore;
            transform.rotation = Quaternion.Euler(0f, 0f, newRotation);
        }
    }

    private void LateUpdate()
    {
        Color newLightCol = GameController.Instance.GetPlayerLightColor();
        playerLight.color = newLightCol;
        playerLight.pointLightInnerRadius = GameController.Instance.GetPlayerLightRadius();
        playerLight.pointLightOuterRadius = GameController.Instance.GetPlayerLightOuterRadius();
        playerLight.intensity = GameController.Instance.GetPlayerLightIntensity();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Crystal"))
        {
            GameCrystal crystalComponent = other.gameObject.GetComponent<GameCrystal>();
            if(crystalComponent.type == CrystalType.C_WIN)
            {
                //Win level
                Debug.Log("Win Win Win!!!");
                GameController.Instance.endScreen = true;
                EndScreen.LevelEnd(true);
            }
            else
            { 
                claimedCrystals[crystalComponent.GetNumber()] = true;
                soundPlayer.Play("Crystal");
            }
        }
        else if(other.CompareTag("HealthDrop"))
        {
            HealthDrop hd = other.gameObject.GetComponent<HealthDrop>();
            health += hd.healAmount;
            if (health > 12) health = 12;
            GameController.Instance.NotifyHudOfHealthChange(health);
            soundPlayer.Play("HeartPickup");
        }
    }

    public void ReceiveDamageFromEnemy(int damage)
    {
        health -= damage;
        GameController.Instance.NotifyHudOfHealthChange(health);
        charRigidbody.AddForce(-transform.up * (damage / 50f));
        soundPlayer.Play("PlayerHit");
    }
}
