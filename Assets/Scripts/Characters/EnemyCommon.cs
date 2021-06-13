using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommon : MonoBehaviour
{
    public int maxHealth = 100;
    public int health = 100;

    public int colourAffiliation = 0;

    protected Animator animator;
    protected Rigidbody2D rb2d;
    Collider2D ownedCollider;

    //private bool isAttacking = false;
    private const int IDLE = 0;
    private const int MOVE = 1;
    private const int STRIKE = 2;

    protected int animationMode = IDLE;

    private const string attackAnim = "SnakeAttack";
    private const string moveAnim = "SnakeMoving";
    private const string idleAnim = "SnakeIdle";

    public float slowMovementSpeed = 0.03f;
    public float fastMovementSpeed = 0.05f;

    public GameObject healthDropPrefab;

    private int angle = 0;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        ownedCollider = GetComponent<Collider2D>();
    }

    public bool activeTargeting = false;
    public float distanceToMove = 0f;
    public Vector3 movementStartedAt;

    // Update is called once per frame
    void Update()
    {
        //Stops ai's outside of loaded areas
        if(GameController.Instance.DistanceFromPlayer(transform.position) < 10)
        {
            if (activeTargeting)
            {
                //Always face the player
                FacePlayer(GameController.Instance.GetPlayerPosition());
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
                //Debug.Log("Active targeting enabled, facing player with angle " + angle);
                //Check for walls
                ownedCollider.enabled = false;
                RaycastHit2D rc2d = Physics2D.Raycast(PosAsVec2(transform.position), PosAsVec2(-transform.up), 5f);
                ownedCollider.enabled = true;
                //If we see the player and are close enough to attack then attack
                if(animationMode != STRIKE)
                {
                    //Debug.Log("Not currently attacking");
                    if (rc2d.collider != null)
                    {
                        //Debug.Log("Did collide with something");
                        if (rc2d.collider.CompareTag("Player") || rc2d.collider.CompareTag("PlayerSword"))
                        {
                            Debug.Log("Hit player with distance " + rc2d.distance);
                            if (rc2d.distance < 0.6f)
                            {
                                Attack();
                            }
                            else
                            {
                                //Otherwise if there is enough distance to move always move forwards
                                //Move towards player
                                rb2d.MovePosition(PosAsVec2(transform.position + (-transform.up * fastMovementSpeed)));
                                UpdateAnimationMode(MOVE);
                            }
                        }
                        else
                        {
                            //Debug.Log("Hit not player with tag " + rc2d.collider.tag + " distance "+ rc2d.distance);
                            if(rc2d.distance > 1.0f)
                            {
                                rb2d.MovePosition(PosAsVec2(transform.position + (-transform.up * fastMovementSpeed)));
                                UpdateAnimationMode(MOVE);
                            }
                            else if(rc2d.collider.CompareTag("Untagged"))
                            {
                                //Otherwise break off pursuit
                                activeTargeting = false;
                            }
                        }
                    }
                    else
                    {
                        rb2d.MovePosition(PosAsVec2(transform.position + (-transform.up * fastMovementSpeed)));
                        UpdateAnimationMode(MOVE);
                    }
                }
            }
            else
            {
                //Passive mode - Move around randomly
                //If we have already moved somewhere
                if(distanceToMove <= 0)
                {
                    bool acceptableAngle = false;
                    //Pick a new, wall free rotation
                    angle = Random.Range(0, 7) * -45;
                    ownedCollider.enabled = false;
                    do
                    {
                        transform.rotation = Quaternion.Euler(0f, 0f, angle);
                        RaycastHit2D rc2d = Physics2D.Raycast(PosAsVec2(transform.position), PosAsVec2(-transform.up), 5f, layerMask: 1 << 2);
                        //If we hit something
                        if (rc2d.collider != null)
                        {
                            //Debug.Log("Hit something with tag " + rc2d.collider.tag + " at distance " + rc2d.distance + " with angle " + angle);
                            if(rc2d.distance > 1f)
                            {
                                distanceToMove = Random.Range(1f, rc2d.distance);
                                movementStartedAt = transform.position;
                                acceptableAngle = true;
                            }
                            else
                            {
                                acceptableAngle = false;
                                angle -= 45;
                                if(angle < -360)
                                {
                                    //Debug.LogWarning("This snake is completely trapped");
                                    activeTargeting = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            acceptableAngle = true;
                            distanceToMove = Random.Range(1f, 5f);
                            movementStartedAt = transform.position;
                        }
                    } while (!acceptableAngle);
                    ownedCollider.enabled = true;
                }
                if(!activeTargeting)
                {
                    //Check for walls apparently
                    ownedCollider.enabled = false;
                    RaycastHit2D rc2d = Physics2D.Raycast(PosAsVec2(transform.position), PosAsVec2(-transform.up), 5f);
                    ownedCollider.enabled = true;
                    if (rc2d.collider != null && rc2d.distance <= 0.75f)
                    {
                        distanceToMove = 0;
                    }
                    else
                    {
                        rb2d.MovePosition(PosAsVec2(transform.position + (-transform.up * slowMovementSpeed)));
                        UpdateAnimationMode(MOVE);
                    }
                    //Once we know an angle
                    if (Vector3.Distance(movementStartedAt, transform.position) > distanceToMove)
                    {
                        distanceToMove = 0;
                    }
                }
            }
        }
    }

    private Vector2 PosAsVec2(Vector3 pos)
    {
        return new Vector2(pos.x, pos.y);
    }

    private void FacePlayer(Vector3 player)
    {
        float deltaX = player.x - transform.position.x;
        float deltaY = player.y - transform.position.y;

        int facingX = deltaX < -1f ? -1 : deltaX > 1f ? 1 : 0;
        int facingY = deltaY < -1f ? -1 : deltaY > 1f ? 1 : 0;

        if(facingY > 0 && facingX == 0)
        {
            angle = 0 - 180;
        }
        else if(facingY > 0 && facingX > 0)
        {
            angle = -45 - 180;
        }
        else if(facingY == 0 && facingX > 0)
        {
            angle = -90 - 180;
        }
        else if(facingY < 0 && facingX > 0)
        {
            angle = -135 - 180;
        }
        else if(facingY < 0 && facingX == 0)
        {
            angle = -180 - 180;
        }
        else if(facingY < 0 && facingX < 0)
        {
            angle = -225 - 180;
        }
        else if(facingY == 0 && facingX < 0)
        {
            angle = -270 - 180;
        }
        else if(facingY > 0 && facingX < 0)
        {
            angle = -315 - 180;
        }
        //else angle stays the same which is allowed
    }

    private void Attack()
    {
        if (animationMode != STRIKE)
        {
            UpdateAnimationMode(STRIKE);
            StartCoroutine(FreeAttackLater());
        }
    }

    private IEnumerator FreeAttackLater()
    {
        yield return new WaitForSeconds(10f / 30f);
        UpdateAnimationMode(IDLE);
    }

    public void UpdateAnimationMode(int newMode)
    {
        if(animationMode != newMode)
        {
            animator.StopPlayback();
            string animation = newMode == IDLE ? idleAnim : newMode == STRIKE ? attackAnim : moveAnim;
            animator.Play(animation);
            animationMode = newMode;
        }
    }

    public void PlayerInflictDamage()
    {
        float multiplier = GameController.Instance.GetColourBrightness(colourAffiliation);
        int damage = Mathf.RoundToInt(100f * multiplier);
        if (damage < 34) damage = 34;
        health -= damage;
        if(health <= 0)
        {
            //Try spawn health drop
            if(Random.Range(0, 100) > 75) // 25% chance to spawn
            {
                GameObject healthDrop = GameObject.Instantiate(healthDropPrefab);
                healthDrop.transform.position = transform.position;
            }
            Destroy(this.gameObject);
        }
        activeTargeting = true;
        ApplyKnockback(damage);
    }

    private void ApplyKnockback(int damage)
    {
        Debug.Log("Snake hit applying knockback");
        rb2d.AddForce(transform.up * (damage / 50f));
    }
}
