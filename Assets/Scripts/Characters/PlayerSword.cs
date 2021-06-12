using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.tag.Equals("EnemyBody"))
        {
            EnemyCommon ec = collision.gameObject.GetComponent<EnemyCommon>();
            ec.PlayerInflictDamage();
        }
    }
}
