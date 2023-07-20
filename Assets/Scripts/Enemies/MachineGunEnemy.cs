using UnityEngine;

public class MachineGunEnemy : Enemy
{
    [SerializeField] private float weaponDamage = 1;
    [SerializeField] private float bulletSpeed = 10;
    [SerializeField] private Bullet bulletPrefab;
    private float setSpeed = 0;
    private float attackRange = 2f;
    public float shootingTime;
    public float shootingCoolDownTime;
    public float accuracy;
    public Weapon machineGunWeapon;

    public override void Attack(float interval)
    {
        base.Attack(interval);
        if (shootingCoolDownTime > shootingTime)
        {
            shootingCoolDownTime = 0;
            Shoot();
        }
        else
        {
            shootingCoolDownTime += Time.deltaTime;
        }
    }

    public void SetMachineGunEnemy(float _shootingTime, float _shootingCoolDownTime, float _accuracy)
    {
        shootingTime = _shootingTime;
        shootingCoolDownTime = _shootingCoolDownTime;
        accuracy = _accuracy;
    }

    public override void Shoot()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float randomAngle = Random.Range(-accuracy, accuracy);
        angle += randomAngle;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        machineGunWeapon.Shoot(bulletPrefab, this, "Player");
    }

    protected override void Start()
    {
        base.Start();
        health = new Health(1, 0, 1);
        setSpeed = speed;
        weapon = new Weapon("Machine Gun Weapon", weaponDamage, bulletSpeed);
        machineGunWeapon = weapon;
        shootingCoolDownTime = shootingTime + 1;
    }

    protected override void Update()
    {
        base.Update();
        if (target != null)
        { 
            if (Vector2.Distance(transform.position, target.position) < attackRange)
            {
                speed = 0;
                Attack(shootingTime);
            }
            else
            {
                speed = setSpeed;
            }
        }
    }
}
