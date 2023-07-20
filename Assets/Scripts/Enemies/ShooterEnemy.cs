using UnityEngine;

public class ShooterEnemy : Enemy
{
    [SerializeField] private float shootingRate = 3f;
    [SerializeField] private float accuracy = 0.1f;
    [SerializeField] private float safeDistance = 5f;
    [SerializeField] private float weaponDamage = 1;
    [SerializeField] private float bulletSpeed = 10;
    [SerializeField] private Bullet bulletPrefab;
    private Weapon shooterWeapon;
    private float shootingCoolDownTime = 3f;
    private Vector2 direction;
    //private LineRenderer lineRenderer;
    private LineRenderer line;
    private Transform[] targetLinePoints = new Transform[2];
    private TargetLineController shooterTargetLine;
    public Color c1 = Color.red;
    public Color c2 = Color.blue;
    public int lengthofLineRenderer = 20;

    public override void Attack(float interval)
    {
        base.Attack(interval);
        if (shootingCoolDownTime > shootingRate)
        {
            shootingCoolDownTime = 0;
            Shoot();
        }
        else
        {
            shootingCoolDownTime += Time.deltaTime;
        }
    }

    private void RotateToFace(Vector2 targetPosition)
    {
        direction = targetPosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetShooterEnemy(float _shootingRate, float _shootingCoolDownTime, float _accuracy)
    {
        shootingRate = _shootingRate;
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

        shooterWeapon.Shoot(bulletPrefab, this, "Player");
    }

    protected override void Start()
    {
        base.Start();
        health = new Health(1, 0, 1);

        weapon = new Weapon("Shooter Weapon", weaponDamage, bulletSpeed);
        shooterWeapon = weapon;

        shootingCoolDownTime = shootingRate + 1;

        line = GetComponent<LineRenderer>();

        /*
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(0, direction);

        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        lineRenderer.colorGradient = gradient;*/
    }

    protected override void Update()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                target = player.transform;
            }
        }

        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.position) >= safeDistance)
            {
                Move(speed);
            }

            RotateToFace(target.position);

            targetLinePoints[0] = gameObject.transform;
            targetLinePoints[1] = target.transform;
            line.positionCount = targetLinePoints.Length;
            DrawTargetLine();

            Attack(shootingRate);
        

            /*LineRenderer lineRenderer = GetComponent<LineRenderer>();
            var t = Time.time;
            for (int i = 0; i < lengthofLineRenderer; i++)
            {
                lineRenderer.SetPosition(i, new Vector3(i * 50f, 0, 0));
            }
            */
        }
    }


    private void DrawTargetLine()
    {
        for (int i = 0; i < targetLinePoints.Length; i++)
        {
            line.SetPosition(i, targetLinePoints[i].position);
        }
    }
}
