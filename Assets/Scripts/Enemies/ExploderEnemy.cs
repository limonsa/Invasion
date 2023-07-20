using UnityEngine;

public class ExploderEnemy : Enemy
{
    [SerializeField] private float explodeDamage;
    private float setSpeed = 0;
    public float explodeRange = 40;

    public void Explode()
    {
        if (target.GetComponent<IDamageable>() != null)
        {
            target.GetComponent<IDamageable>().GetDamage(explodeDamage);
            Destroy(gameObject);
        }

    }

    public void SetExploderEnemy(float _explodeRange)
    {
        explodeRange = _explodeRange;
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
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.position) < explodeRange)
            {
                speed = 0;
                Explode();
            }
        }
    }
}
