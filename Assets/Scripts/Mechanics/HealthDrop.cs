using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    public int healAmount;

    private void Start()
    {
        if(healAmount == 0)
        {
            healAmount = Random.Range(1, 4);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(KillNextFrame());
        }
    }

    private IEnumerator KillNextFrame()
    {
        yield return null;
        Destroy(this.gameObject);
    }
}
