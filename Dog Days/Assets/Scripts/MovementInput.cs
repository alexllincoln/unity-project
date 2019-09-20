using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInput : MonoBehaviour {
    public float InputX;
    public float InputZ;
    public float desiredRotationSpeed;
    public float runSpeed = 5f;
    public float inputLevel; //rename?
    public float allowPlayerRotation;
    public bool blockRotationPlayer;
    public bool isGrounded;
    public Vector3 desiredMoveDirection;
    public Animator anim;
    public Camera cam;
    public CharacterController controller;
    private float verticalVel;
    private Vector3 moveVector;

    // Start is called before the first frame update
    void Start () {
        anim = this.GetComponent<Animator> ();
        cam = Camera.main;
        controller = this.GetComponent<CharacterController> ();
    }

    // Update is called once per frame
    void Update () {
        InputMagnitude ();

        //If you don't need the character grounded then get rid of this part.
        isGrounded = controller.isGrounded;
        if (isGrounded) {
            verticalVel -= 0;
        } else {
            verticalVel -= 2;
        }
        moveVector = new Vector3 (0, verticalVel, 0);
        controller.Move (moveVector);
        //
    }
    void PlayerMoveAndRotation () {
        InputX = Input.GetAxis ("Horizontal");
        InputZ = Input.GetAxis ("Vertical");

        var camera = Camera.main; // Isn't this already in the start method?
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize ();
        right.Normalize ();

        desiredMoveDirection = forward * InputZ + right * InputX;

        if (blockRotationPlayer == false) {
            transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), desiredRotationSpeed);
            controller.Move (desiredMoveDirection * Time.deltaTime * runSpeed);
        }
    }
    void InputMagnitude () {
        //Calculate the input vectors
        InputX = Input.GetAxis ("Horizontal");
        InputZ = Input.GetAxis ("Vertical");

        anim.SetFloat ("InputZ", InputZ, 0.0f, Time.deltaTime * runSpeed);
        anim.SetFloat ("InputX", InputX, 0.0f, Time.deltaTime * runSpeed);

        //Calculate the Input Magnitude
        inputLevel = new Vector2 (InputX, InputZ).sqrMagnitude;

        //Physically move player
        if (inputLevel > allowPlayerRotation) {
            anim.SetFloat ("InputMagnitude", inputLevel, 0.0f, Time.deltaTime);
            PlayerMoveAndRotation ();
        } else if (inputLevel < allowPlayerRotation) {
            anim.SetFloat ("InputMagnitude", inputLevel, 0.0f, Time.deltaTime);
        }
    }
}