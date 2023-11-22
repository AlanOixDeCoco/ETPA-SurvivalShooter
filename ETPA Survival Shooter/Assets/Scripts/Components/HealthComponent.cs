using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float _maxHealth;

    public UnityEvent _onHealthAtZero;

    // Private variables
    private float _health;

    private void Start()
    {
        _health = _maxHealth;
    }

    public void SetHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
        _health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0) _onHealthAtZero?.Invoke();
    }

    public void Heal(float heal)
    {
        _health += heal;
        if(_health > _maxHealth) _health = _maxHealth;
    }
}
