using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CharacterController))]
public class Controller : MonoBehaviour
{
    //player movement
    public float playerSpeed;
    private Rigidbody playerRB;

    JoystickController _joystickController;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    // Use this for initialization
    void Start ()
    {
        playerRB = GetComponent<Rigidbody> ();
        _joystickController = GameObject.FindObjectOfType<JoystickController>();
        characterController = GetComponent<CharacterController> ();
    }

    // Update is called once per frame
    void Update ()
    {
        CharacterController controller = GetComponent<CharacterController> ();

        moveDirection = new Vector3 (_joystickController.Horizontal (), 0, _joystickController.Vertical ());

        float angle = Mathf.Atan2 (_joystickController.Horizontal (), _joystickController.Vertical ()) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (new Vector3 (0, angle, 0)), 180 * Time.deltaTime);

        controller.Move (moveDirection * playerSpeed * Time.deltaTime);

        if (JoystickController.JoystickPressed == true)
        {
            controller.Move (moveDirection * playerSpeed * Time.deltaTime);
        }
    }

}