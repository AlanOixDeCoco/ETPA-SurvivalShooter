using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private float _bulletsPerSecond = 10;

    // Private variables


    private bool _canShoot = true;

    public async void Shoot()
    {
        if (!_canShoot) return;
        
    }
}
