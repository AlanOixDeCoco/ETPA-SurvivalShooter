using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, 0.2f);
        if (!collision.gameObject.CompareTag("Enemy")) return;
        var healthComponent = collision.transform.GetComponent<HealthComponent>();
        healthComponent?.TakeDamage(10);
    }
}
