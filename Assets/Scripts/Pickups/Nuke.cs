using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : Pickup
{
    private GameManager gm;

    /// <summary>
    /// When colliding with the player a nuke is picked up
    /// and the UI is updated showing another nuke ready to be used
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            gm.LoadNuke();
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    /// <summary>
    /// Show the message in the console
    /// </summary>
    /// <param name="message"></param>
    public void DisplayInConsole(string message) => Debug.Log($"NUKE SAYS >>> {message}");

}
