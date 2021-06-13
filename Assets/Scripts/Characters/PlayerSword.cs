using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    private bool damageDealt;
    private AudioManager soundPlayer;

    private void Start()
    {
        soundPlayer = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.CompareTag("EnemyBody") && !damageDealt)
        {
            EnemyCommon ec = collision.gameObject.GetComponent<EnemyCommon>();
            ec.PlayerInflictDamage();
            damageDealt = true;
            soundPlayer.Play("SnakeHit");
        }
        else if (collision.CompareTag("Untagged"))
        {
            soundPlayer.Play("WallHit");
        }
    }

    private void OnEnable()
    {
        damageDealt = false;
    }
}
