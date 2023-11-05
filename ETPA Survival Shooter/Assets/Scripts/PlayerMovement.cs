using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _mesh;
    [SerializeField] private Animator _animator;

    [Header("Parameters")]
    [SerializeField] private float _moveSpeed = 6f;
    

    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 movement = Vector3.zero;
        movement.x = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
        movement.z = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0);
        movement.Normalize();
        movement *= _moveSpeed;

        movement = Quaternion.Euler(0, -45, 0) * movement;

        // Rotate mesh
        _mesh.LookAt(transform.position + movement);

        // Apply movement
        _characterController.SimpleMove(movement);


        // Update animator parameters
        _animator.SetFloat("velocity", movement.magnitude);
    }
}