using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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

        Camera mainCamera = Camera.main;
        Vector3 camRot = mainCamera.transform.eulerAngles;
        Debug.Log(camRot);

        movement = Quaternion.Euler(0, camRot.y, 0) * movement;

        _characterController.SimpleMove(movement);
        transform.GetChild(0).Rotate(0, camRot.y - transform.GetChild(0).eulerAngles.y, 0);

        //RaycastHit hit;
        //Physics.Raycast(transform.position - (Vector3.up * (_characterController.height/2)), -Vector3.up, out hit);

        //float distanceToGround = hit.distance;
        //Debug.Log(distanceToGround);

        //if(distanceToGround > 0.8 && distanceToGround < 1f)
        //{
        //    Vector3 newPos = transform.position;
        //    newPos.y = hit.point.y + (_characterController.height / 2) + 0.7f;
        //    transform.position = newPos;
        //}
    }
}