using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Projectile_NANA : Projectile
{
    public override void Attack()
    {
        
    }

    public override void Decay()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyBase>().Hit(Damage);
            this.gameObject.SetActive(false);
        }
    }

}
