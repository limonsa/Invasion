using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public abstract class Pickup : MonoBehaviour
{
    public enum Types
    {
        Health, 
        Nuke,
        PowerUp
    }

    public virtual void OnPickup()
    {
        Destroy(gameObject);
    }
}