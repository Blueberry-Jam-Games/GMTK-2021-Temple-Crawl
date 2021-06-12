using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int minDamage = 1;
    public int maxDamage = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enemy hit " + collision.tag);
        if (collision.tag.Equals("Player"))
        {
            int damage = Random.Range(minDamage, maxDamage);
            Debug.Log("Damaging player for " + damage);
            CharacterMovement cm = collision.gameObject.GetComponent<CharacterMovement>();
            cm.ReceiveDamageFromEnemy(damage);
        }
    }
}
