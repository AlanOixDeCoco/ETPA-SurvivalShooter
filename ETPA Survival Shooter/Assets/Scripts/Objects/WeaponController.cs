using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class WeaponSpecs
{
    public int magazineSize = 20;
    public float bulletsPerSecond = 10f;
    public float bulletSpeed = 30f;
    public float reloadTime = 2f;
    public Material weaponMaterial;
    public Material bulletMaterial;
}

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _bulletsOrigin;
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Weapon parameters")]
    [SerializeField] private WeaponSpecs _weaponSpecs;

    // Private variables
    private Animator _animator;
    private bool _shooting = false;
    private bool _canShoot = true;
    private bool _reloading = false;
    private int _ammos;
    private float _lastShotTime = 0;

    public bool CanShoot { get => _canShoot; private set => _canShoot = value; }
    public bool Shooting { get => _shooting; set => _shooting = value; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _ammos = _weaponSpecs.magazineSize;
    }

    private void Update()
    {
        // Wait according to the bullets per second value
        _canShoot = !_reloading && (Time.time >= (_lastShotTime + (1 / _weaponSpecs.bulletsPerSecond)));

        if (Shooting && _canShoot) Shoot();
    }

    private void Shoot()
    {
        // Start anim
        _animator.Play("Shoot", -1, 0);

        // Create the bullet
        _lastShotTime = Time.time;
        _ammos--;
        var newBullet = Instantiate(_bulletPrefab);
        newBullet.transform.position = _bulletsOrigin.position;
        newBullet.transform.rotation = _bulletsOrigin.rotation;
        newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.rotation * Vector3.forward * _weaponSpecs.bulletSpeed;

        if (_ammos <= 0) Reload();
    }

    public async void Reload()
    {
        _reloading = true;
        _animator.speed = 1/_weaponSpecs.reloadTime;
        _animator.Play("Reload");

        await Task.Delay((int)_weaponSpecs.reloadTime * 1000);

        _animator.speed = 1;

        _ammos = _weaponSpecs.magazineSize;
        _reloading = false;
    }
}
