using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header ("Speed Settings")]
    public float walkSpeed;
    public float runSpeed;
    private float speed;

    [Header ("Jump Settings")]
    public float jumpForce;
    public ForceMode forceType;
    bool isFalling;

    Rigidbody rb;

    void Start () {
        rb = GetComponent<Rigidbody> ();
    }

    void Update () {
        #region Inputs
        //Sprint
        if (Input.GetButton ("Sprint"))
            speed = runSpeed;
        else if (speed != walkSpeed)
            speed = walkSpeed;

        //Jump
        if (Input.GetButton ("Jump") && !isFalling)
            Jump (jumpForce, forceType);
        #endregion
    }
    void FixedUpdate () {
        #region Motion
        var x = Input.GetAxis ("Horizontal");
        var z = Input.GetAxis ("Vertical");
        rb.MovePosition (transform.position + transform.TransformDirection (x, 0, z) * Time.deltaTime * speed);
        #endregion
    }
    #region Completed Functions
    void Jump (float force, ForceMode type) {
        rb.AddForce (transform.up * force, type);
        isFalling = true;
    }
    #endregion

    #region Collision and Triggers
    private void OnCollisionStay (Collision collision) {
        if (isFalling)
            isFalling = false;
    }
    #endregion
}