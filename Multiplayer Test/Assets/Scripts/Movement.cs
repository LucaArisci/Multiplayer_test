using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] private float _moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _moveSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        var vectorMovement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized * _moveSpeed;

        vectorMovement.y = _rb.velocity.y;
        _rb.velocity = vectorMovement;
    }
}
