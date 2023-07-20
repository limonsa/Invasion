using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp : Pickup
{
    public static UnityAction PowerUpGun;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            PowerUpGun?.Invoke();
            Destroy(gameObject);
        }
    }

    public void DisplayInConsole(string message) => Debug.Log($"POWER UP SAYS >>> {message}");
}