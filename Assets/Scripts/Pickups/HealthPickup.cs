using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup, IDamageable
{
    [SerializeField] private int healthMin;
    [SerializeField] private int healthMax;

    public override void OnPickup()
    {
        base.OnPickup();

        float health = Random.Range(healthMin, healthMax);

        var player = GameManager.GetInstance().GetPlayer();

        player.health.AddHealth(health);

        //Debug.Log($"Added {health} to Player");
    }

    public void GetDamage(float damage)
    {
        OnPickup();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"PICKUP SAYS >>> {collision.gameObject.name} detected collision with {collision.otherCollider.gameObject.name}");
        if (collision.gameObject.name.Contains("Player"))
        {
            //Debug.Log($"NUKE.OnCollisionEnter2D() SAYS >>> {collision.gameObject.name} detected collision with {collision.otherCollider.gameObject.name}");
            OnPickup();
        }
    }
}
