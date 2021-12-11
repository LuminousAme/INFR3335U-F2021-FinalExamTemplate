using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMovement : MonoBehaviour
{
    [Header("Movememnt")]
    [SerializeField] private Joystick moveJoyStick; //joystick for moving the player
    [SerializeField] private float moveSpeed = 1f;


    [Space]
    [Header("Camera")]
    [SerializeField] private Joystick lookJoyStick; //joystick for looking around 
    [SerializeField] private Transform lookAt; //the lookat position
    [SerializeField] private float lookSpeed = 1f;
    [SerializeField] private float verticalLookSpeed = 0.1f;
    [SerializeField] private float pitchLimit = 0.5f;
    private Vector3 basePos;

    //the pitch and yaw of the head
    private float yaw = 0, pitch = 0;

    //the built in character controller
    private CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        //get the character controller attached to this gameobject
        cc = this.GetComponent<CharacterController>();

        basePos = lookAt.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //the direction the player should move
        float rightLeft = moveJoyStick.Horizontal;
        float forwardBack = moveJoyStick.Vertical;

        //get the direction of movement 
        Vector3 newMove = (transform.forward * forwardBack) + (transform.right * rightLeft);
        newMove = newMove.normalized;

        //apply that movement
        cc.Move(newMove * moveSpeed * Time.deltaTime);

        //the direction the neck and camera should rotate
        yaw = lookJoyStick.Horizontal * lookSpeed;
        pitch -= lookJoyStick.Vertical * verticalLookSpeed;
        pitch = Mathf.Clamp(pitch, -pitchLimit, pitchLimit);

        //rotate the character

        transform.Rotate(new Vector3(0.0f, yaw, 0.0f));

        //rotate the camera
        lookAt.localPosition = basePos;
        lookAt.localPosition += new Vector3(pitch, 0.0f, 0.0f);
    }
}
