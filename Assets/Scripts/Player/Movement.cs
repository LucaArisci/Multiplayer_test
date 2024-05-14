using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody _rb;
    //[SerializeField] private float _moveSpeed;
    private float knockbackTimer = 0f;

    private float horizontalMovement;
    private float verticalMovement;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _moveSpeed = 10f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (Time.time < knockbackTimer)
        {
            return;
        }

        _rb.velocity = new Vector3(horizontalMovement, 0f, verticalMovement);
    }

    public void SetHorizontalMovement(float value)
    {
        horizontalMovement = value;
    }

    public void SetVerticalMovement(float value)
    {
        verticalMovement = value;
    }
}
