using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var healthComponent = collision.transform.GetComponent<HealthComponent>();
        healthComponent?.TakeDamage(10);
        Destroy(gameObject);
    }
}
