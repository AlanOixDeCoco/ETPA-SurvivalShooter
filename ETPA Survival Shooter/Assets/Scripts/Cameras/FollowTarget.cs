using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _target;

    [Header("Parameters")]
    [Range(0.001f, 1f)] [SerializeField] private float _speed;

    private Vector3 _offset;

    private void Start()
    {
        _offset = _target.position - transform.position;
    }

    private void LateUpdate()
    {
        Vector3 destination = _target.position - _offset;
        transform.position = Vector3.Lerp(transform.position, destination, _speed);
    }
}
