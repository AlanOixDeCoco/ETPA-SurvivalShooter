using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttackCollisionComponent : MonoBehaviour
{
    [SerializeField] private UnityEvent<float> _onAttackCollision;
    private void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag("EnemyAttack")) return;
        _onAttackCollision?.Invoke(other.GetComponentInParent<EnemyManager>().EnemyStats.damage);
    }
}
