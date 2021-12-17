using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControllerFPS : MonoBehaviour
{
    private NavMeshAgent playerAI;
    Vector3 movement;
    Vector3 movementInput;
    Vector2 cameraInput;
    public float movementSpeed = 10f;

    public float cameraSensibility = 8f;
    Transform cam;
    float rotX;

    public bool canMove = false;
    // Start is called before the first frame update
    void Start()
    {
        playerAI = GetComponent<NavMeshAgent>();
        cam = GameObject.Find("Main Camera").transform;
        rotX = cam.eulerAngles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            movementInput.x = Input.GetAxis("Horizontal");
            movementInput.z = Input.GetAxis("Vertical");

            cameraInput.x = Input.GetAxis("Mouse X") * cameraSensibility;
            cameraInput.y = Input.GetAxis("Mouse Y") * cameraSensibility;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = 10f;
            }
            else
            {
                movementSpeed = 5f;
            }

            movement = transform.rotation * movementInput * movementSpeed * Time.deltaTime;

            playerAI.Move(movement);
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            transform.rotation *= Quaternion.Euler(0, cameraInput.x, 0);

            rotX -= cameraInput.y;
            rotX = Mathf.Clamp(rotX, -50, 55);
            cam.localRotation = Quaternion.Euler(rotX, 0, 0);
        }
    }
}
