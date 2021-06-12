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

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    int time = 500;
    // Update is called once per frame
    void Update()
    {
        time--;
        if(time == 0)
        {
            Attack();
            time = 500;
        }
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
        health -= Mathf.RoundToInt(100f * multiplier); //TODO fix
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
