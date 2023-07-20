using UnityEngine;

public class Weapon
{
    private string name;
    private float damage;
    private float bulletSpeed;
    private float bulletSpeedCache;


    public Weapon(string _name, float _damage, float _bulletSpeed)
    {
        name = _name;
        damage = _damage;
        bulletSpeed = _bulletSpeed;
        bulletSpeedCache = _bulletSpeed;
    }
    public Weapon() { }

    public void Shoot(Bullet _bullet, PlayableObject _player, string _targetTag, float _timeToDie = 5)
    {
        Bullet tempBullet = GameObject.Instantiate(_bullet, _player.transform.position, _player.transform.rotation);
        tempBullet.SetBullet(damage, _targetTag, bulletSpeed);
        Debug.Log($"Shooting from Weapon from {_player.tag} to kill {_targetTag}");

        tempBullet.transform.rotation = _player.transform.rotation;
        GameObject.Destroy(tempBullet.gameObject, _timeToDie);


    }

    public float GetDamage()
    {
        return damage;
    }

    /// <summary>
    /// Set bulletSpeed to the original value,
    /// the one specified when the object was first instaciated
    /// 
    /// </summary>
    public void ResetBulletSpeed()
    {
        bulletSpeed = bulletSpeedCache;
    }

    /// <summary>
    /// Sets the bullet speed to the value received as parameter
    /// </summary>
    /// <param name="bulletPowerUpSpeed"></param>
    public void PowerUpBulletSpeed(float bulletPowerUpSpeed)
    {
        bulletSpeed = bulletPowerUpSpeed;
    }

    /// <summary>
    /// Retruns the current bullet speed
    /// </summary>
    /// <returns>bulletSpeed</returns>
    public float GetBulletSpeed()
    {
        return bulletSpeed;
    }

}
