using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    public bool grounded = false;
    new private Rigidbody2D rigidbody;
    public LayerMask map;
    public Transform backWheel1;
    public Transform backWheel2;
    public Transform frontWheel;
    private float contactRad = 0.58f;
    private float jumpForce = 100.0F;

    private void Jump()
    {
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Update()
    {
        grounded = Physics2D.OverlapCircle(backWheel1.transform.position, contactRad, map) || Physics2D.OverlapCircle(backWheel1.transform.position, contactRad, map) || Physics2D.OverlapCircle(backWheel1.transform.position, contactRad, map);
    }

    void Start()
    {
        rigidbody = rigidbody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (grounded == true && Input.GetKey(KeyCode.F))
        {
          Jump();
        }
    }
}
