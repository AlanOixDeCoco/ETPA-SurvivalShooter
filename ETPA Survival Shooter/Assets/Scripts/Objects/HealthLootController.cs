using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLootController : MonoBehaviour
{
    [SerializeField] private float _heal = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<HealthComponent>().Heal(_heal);
            Destroy(gameObject);
        }
    }
}
