using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommon : MonoBehaviour
{
    public int maxHealth = 100;
    public int health = 100;

    public int colourAffiliation = 0;

    protected Animator animator;
    private bool isAttacking = false;

    private const string attackAnim = "SnakeAttack";
    private const string idleAnim = "SnakeIdle";

    private int angle = 0;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.Instance.DistanceFromPlayer(transform.position) < 10)
        {
            Vector3 player = GameController.Instance.GetPlayerPosition();
            if(Vector3.Distance(player, transform.position) < 16)
            {
                FacePlayer(player);
            }

            transform.position = (- transform.up) + transform.position;

            transform.rotation = Quaternion.Euler(0, 0, angle + 180); // the image is backwards from what I expect.
            RaycastHit2D rc = Physics2D.Raycast(PosAsVec2(transform.position), PosAsVec2(player - transform.position), 10f);
            Debug.DrawRay(transform.position, player - transform.position);
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
