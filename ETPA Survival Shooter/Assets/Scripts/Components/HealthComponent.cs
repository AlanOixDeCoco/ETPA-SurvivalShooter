using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _invincibleDelay;

    public UnityEvent<float, float> _onHealthChange;
    public UnityEvent _onHealthAtZero;

    // Private variables
    private float _health;
    private float _invincibleTime = 0;

    private void Start()
    {
        _health = _maxHealth;
        _onHealthChange?.Invoke(_health, _maxHealth);
    }

    public void SetHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
        _health = maxHealth;
        _onHealthChange?.Invoke(_health, _maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if(Time.time > _invincibleTime) {
            _invincibleTime = Time.time + _invincibleDelay;
            _health -= damage;
            _onHealthChange?.Invoke(_health, _maxHealth);
            if (_health <= 0) _onHealthAtZero?.Invoke();
        }
    }

    public void Heal(float heal)
    {
        _health += heal;
        _onHealthChange?.Invoke(_health, _maxHealth);
        if (_health > _maxHealth) _health = _maxHealth;
    }
}
