using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    private bool damageDealt;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.tag.Equals("EnemyBody") && !damageDealt)
        {
            EnemyCommon ec = collision.gameObject.GetComponent<EnemyCommon>();
            ec.PlayerInflictDamage();
            damageDealt = true;
        }
    }

    private void OnEnable()
    {
        damageDealt = false;
    }
}
