using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController _instance;

    public Transform TargetObject;
    public bool IsMovingToNewTarget = false;
    public float DampTime = 0; // The smoothening factor
    private float OriginalDampTime; // The smoothening factor
    public float DampTimeOnNewTarget = 0.22f; // The smoothening factor

    public Vector3 CameraDistance;

    public float MovementOffset = 0.2f; // Offset of how close the camera must be before it is "close enough" to reduce the damp time

    private Vector3 velocity = Vector3.zero;
    private Camera myCamera;

    public float CameraHeightOffset = 0; // use negative numbers to zoom in and positive numbers to zoom out
    public float CameraHeightOffsetOriginal = 0;

    // follow mouse variables
    public float mouseSpeedH = 2.0f;
    public float mouseSpeedV = 2.0f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;



    private void Awake()
    {
        _instance = this;

        myCamera = gameObject.GetComponent<Camera>();
        OriginalDampTime = DampTime;
        
        CameraDistance = myCamera.WorldToViewportPoint(TargetObject.position);
    }

    private void Update()
    {
        if (!MyGameManager._instance.isKeyboardControls) { return; }
        if (TargetObject)
        {
            Vector3 delta = TargetObject.position - myCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, CameraDistance.z / 1.75f));
            Vector3 destination = transform.position + delta + new Vector3(0, CameraHeightOffset, 0);
            Vector3 distanceToNewObj = (transform.position - destination);

            if (IsMovingToNewTarget)
            {
                if (
                    (distanceToNewObj.x > MovementOffset) || (distanceToNewObj.x < (MovementOffset * -1)) ||
                    (distanceToNewObj.y > MovementOffset) || (distanceToNewObj.y < (MovementOffset * -1)) ||
                    (distanceToNewObj.z > MovementOffset) || (distanceToNewObj.z < (MovementOffset * -1))
                    ) //((transform.position - destination) != Vector3.zero)
                {
                    // Slow down the move speed for the transition
                    DampTime = DampTimeOnNewTarget;
                }
                else
                {
                    IsMovingToNewTarget = false;
                    Debug.Log("Camera is now pointing at new object");
                    DampTime = OriginalDampTime;
                }
            }

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime);
        }

        FollowMouseAngle();

    }

    public void FollowMouseAngle()
    {
        if (!MyGameManager._instance.isKeyboardControls) { return; }

        yaw += mouseSpeedH * Input.GetAxis("Mouse X");
        pitch -= mouseSpeedV * Input.GetAxis("Mouse Y");

        this.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    #region Custom functions

    public void ChangeTarget(Transform newTarget, float heightOffset)
	{
		// Move the camera to to new playerobj
		TargetObject = newTarget;

		// TODO: Add special camera effects if there are any
		Debug.Log("TODO: Add special camera effects if there are any");

		ChangeCameraZoom(heightOffset);

		// Set flag for damping
		IsMovingToNewTarget = true;
	}

	public void ChangeCameraZoom(float heightOffset)
	{
		// Do zoom-in/out
		CameraHeightOffset = CameraHeightOffsetOriginal + heightOffset;

		// Set flag for damping
		IsMovingToNewTarget = true;
	}

	#endregion
}
