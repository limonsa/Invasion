using UnityEngine;
using System.Collections;

public class MeleeEnemy : Enemy
{
    [SerializeField] private float attackRange;
    [SerializeField] private float attackTime = 0;
    private float timer = 0;
    private float setSpeed = 0;

    public override void Attack(float interval)
    {
        //base.Attack(interval);
        if (timer <= interval)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            target.GetComponent<IDamageable>().GetDamage(weapon.GetDamage());
            //Debug.Log(weapon.GetDamage());
        }

    }

    public void SetMeleeEnemy(float _attackRange, float _attackTime)
    {
        attackRange = _attackRange;
        attackTime = _attackTime;
    }

    protected override void Start()
    {
        base.Start();
        health = new Health(1, 0, 1);
        setSpeed = speed;
    }

    protected override void Update()
    {
        base.Update();
        /*if (target != null)
        { 
            if (Vector2.Distance(transform.position, target.position) < attackRange)
            {
                speed = 0;
                Attack(attackTime);
            }
            else
            {
                speed = setSpeed;
            }
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks the target
        //Debug.LogWarning($"CHECKING!!!! Bullet sent for {targetTag} DETECTED COLLSION WITH !{collision.gameObject.tag} tag");
        if (!collision.gameObject.CompareTag("Player")) return;

        IDamageable damageable = collision.GetComponent<IDamageable>();
        Damage(damageable);

    }

    void Damage(IDamageable damageable)
    {
        if (damageable != null)
        {
            damageable.GetDamage(30f);

            GameManager.GetInstance().scoreManager.IncrementScore();
            Destroy(gameObject);
        }
    }

}
