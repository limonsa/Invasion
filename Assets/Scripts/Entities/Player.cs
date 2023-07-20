using UnityEngine;
using System;
//using UnityEngine.SceneManagement;

public class Player : PlayableObject
{
    [SerializeField] private Camera cam;
    [SerializeField] private float speed;

    [SerializeField] private float weaponDamage = 1;
    [SerializeField] private float bulletSpeed = 10;
    [SerializeField] private float bulletPowerUpSpeed = 20f;
    [SerializeField] private Bullet bulletPrefab;
    //adding the score to the player
    //public float Score { get; set; }

    private Rigidbody2D playerRB;

    Action OnDeath;
   
    //public Health health = new Health(100);

    public void Awake()
    {
        health = new Health(100, 0.05f, 100);
        playerRB = GetComponent<Rigidbody2D>();

        //Set player weapon
        weapon = new Weapon("Player Weapon", weaponDamage, bulletSpeed);

    }

    private void Update()
    {
        health.RegenHealth();
    }

    public override void Move(Vector2 direction, Vector2 target)
    {
        playerRB.velocity = speed * Time.deltaTime * direction;

        var playerScreenPos = cam.WorldToScreenPoint(transform.position);
        target.x -= playerScreenPos.x;
        target.y -= playerScreenPos.y; //constantly reduce position based on player position

        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public override void Shoot()
    {
        weapon.Shoot(bulletPrefab, this, "Enemy");
    }

    public override void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);

    }

    public override void Attack(float interval)
    {

    }

    public override void GetDamage(float damage)
    {
        health.DeductHealth(damage);
        if (health.GetHealth() < 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Powers up the weapon to shoot bullets at a high rate
    /// </summary>
    public void PowerUpGun()
    {
        weapon.PowerUpBulletSpeed(bulletPowerUpSpeed);
    }

    /// <summary>
    /// Sets the weapon to shoot bullets at a regular rate
    /// </summary>
    public void PowerDownGun()
    {
        weapon.ResetBulletSpeed();
    }

}