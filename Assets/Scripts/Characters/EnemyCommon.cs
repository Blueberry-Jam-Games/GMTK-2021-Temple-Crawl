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
    private bool isAttacking = false;

    private const string attackAnim = "SnakeAttack";
    private const string idleAnim = "SnakeIdle";

    private int angle = 0;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public bool activeTargeting = false;
    public float distanceToMove = 0f;

    // Update is called once per frame
    void Update()
    {
        //Stops ais outside of loaded areas
        if(GameController.Instance.DistanceFromPlayer(transform.position) < 10)
        {
            if (activeTargeting)
            {

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
                    do
                    {
                        transform.rotation = Quaternion.Euler(0f, 0f, angle);
                        RaycastHit2D rc2d = Physics2D.Raycast(PosAsVec2(transform.position), PosAsVec2(-transform.up), 5f);
                        //If we hit something
                        if (rc2d.collider != null)
                        {
                            Debug.Log("Hit something with tag " + rc2d.collider.tag + " at distance " + rc2d.distance + " with angle " + angle);
                            if(rc2d.distance > 0.5f)
                            {
                                distanceToMove = Random.Range(1f, rc2d.distance);
                                acceptableAngle = true;
                            }
                            else
                            {
                                acceptableAngle = false;
                                angle -= 45;
                                if(angle < -360)
                                {
                                    Debug.LogWarning("This snake is completely trapped");
                                    activeTargeting = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            acceptableAngle = true;
                            distanceToMove = Random.Range(1f, 5f);
                        }
                    } while (!acceptableAngle);
                }
                if(!activeTargeting)
                {
                    //Once we know an angle
                    Vector3 start = transform.position;
                    rb2d.MovePosition(PosAsVec2(transform.position + (-transform.up * 0.05f)));
                    distanceToMove -= Vector3.Distance(start, transform.position);
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
            angle = 0;
        }
        else if(facingY > 0 && facingX > 0)
        {
            angle = -45;
        }
        else if(facingY == 0 && facingX > 0)
        {
            angle = -90;
        }
        else if(facingY < 0 && facingX > 0)
        {
            angle = -135;
        }
        else if(facingY < 0 && facingX == 0)
        {
            angle = -180;
        }
        else if(facingY < 0 && facingX < 0)
        {
            angle = -225;
        }
        else if(facingY == 0 && facingX < 0)
        {
            angle = -270;
        }
        else if(facingY > 0 && facingX < 0)
        {
            angle = -315;
        }
        //else angle stays the same which is allowed
    }

    private void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.Play(attackAnim);
            StartCoroutine(FreeAttackLater());
        }
    }

    private IEnumerator FreeAttackLater()
    {
        yield return new WaitForSeconds(10f / 30f);
        isAttacking = false;
        animator.Play(idleAnim);
    }

    public void PlayerInflictDamage()
    {
        float multiplier = GameController.Instance.GetColourBrightness(colourAffiliation);
        int damage = Mathf.RoundToInt(100f * multiplier);
        if (damage < 34) damage = 34;
        health -= damage;
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
