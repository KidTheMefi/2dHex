using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRotateHandler : MonoBehaviour
{
    private Vector3 _pressPoint;
    private Quaternion _startRotation;
    [SerializeField]
    private float RotationSpeed;
    [SerializeField]
    private Rigidbody _diceRigidBody;

    public void SetDiceToRotate(Rigidbody diceRigidbody)
    {
        _diceRigidBody = diceRigidbody;
    }
    
    private void OnMouseDown()
    {
        _diceRigidBody.angularVelocity = Vector3.zero;
    }
    private void OnMouseDrag()
    {
        if (_diceRigidBody.angularVelocity.sqrMagnitude > 100)
        {
            return;
        }
        _diceRigidBody.AddTorque(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X")) * 0.2f, ForceMode.Impulse);
    }
}
