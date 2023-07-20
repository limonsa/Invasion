using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    private Player player;
    private float horizontal, vertical;
    private Vector2 lookTarget;
    private bool isGunPowerUp;

    //Action to manage order to use a Nuke
    public static UnityAction ExploteNuke;

    void Start()
    {
        player = GetComponent<Player>();
        isGunPowerUp = false;
        PowerUp.PowerUpGun += TurnOnIsGunPowerUp;
        TimerManager.PowerGunDown += TurnOffIsGunPowerUp;
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        lookTarget = Input.mousePosition;

        if (isGunPowerUp)
        {
            if (Input.GetMouseButton(0))
            {
                player.Shoot();
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            player.Shoot();
        }
        else if (Input.GetMouseButtonDown(1))//When the player right-clicks
        {
            //the nuke destroys all the entities in the scene (including any pickups)
            ExploteNuke?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        player.Move(new Vector2(horizontal, vertical), lookTarget);
    }

    public void SetIsGunPowerUp(bool _isGunPowerUp)
    {
        isGunPowerUp = _isGunPowerUp;
    }

    public void TurnOnIsGunPowerUp()
    {
        SetIsGunPowerUp(true);
    }

    public void TurnOffIsGunPowerUp()
    {
        SetIsGunPowerUp(false);
    }

}