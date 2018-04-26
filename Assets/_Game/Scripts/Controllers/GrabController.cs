using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    public static GrabController _instance;

    public bool isGrab = false;
    public bool isAnyMainFourFingersInHoldPosition;
    public Transform grabPosition;

    public bool isThumbInHoldPosition;
    public bool isIndexInHoldPosition;
    public bool isMiddleInHoldPosition;
    public bool isRingInHoldPosition;
    public bool isPinkyInHoldPosition;

    public bool isHoldingObj = false;
    public GameObject objToHold;

    public float minFingerGrabRotation = 55f;
    public float maxFingerGrabRotation = 200f;
    public float minThumbGrabRotation = 25f;
    public float maxThumbGrabRotation = 200f;

    public bool canPlayerGrabBuffer = true;
    public float timeUntilCanGrab = 0.5f;   // HACK: we need a buffer so we can call the object's Drop function
    private float timeUntilCanGrabCounter = 0;

    public VRfreeGlove myVRfreeGloveScript;
    public GameObject objToFollow;

    // Use this for initialization
    void Start()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Check rotations of each finger depanding on which hardware we use (keyboard/controller vs VRfeeeGlove)
        if (MyGameManager._instance.isKeyboardControls) {
            CheckKeyboardGrab();
        }
        else
        {
            CheckVRfreeGloveGrab();
            this.transform.position = objToFollow.transform.position;
        }

        // Check if any of the main four fingers (excludes thumb) is in holding position
        if (isIndexInHoldPosition || isMiddleInHoldPosition || isRingInHoldPosition || isPinkyInHoldPosition)
        {
            isAnyMainFourFingersInHoldPosition = true;
        }
        else
        {
            isAnyMainFourFingersInHoldPosition = false;
        }

        // Set the isGrab bool
        if (isThumbInHoldPosition && isAnyMainFourFingersInHoldPosition)
        {
            isGrab = true;
        }
        else
        {
            isGrab = false;
        }

        // Grab object
        if (isGrab && objToHold != null)
        {
            isHoldingObj = true;

            Rigidbody objToHoldRb = objToHold.GetComponent<Rigidbody>();
            PickupController objToHoldPickupCtrlr = objToHold.GetComponent<PickupController>();

            // Remove all force from objToHold else we'll have wonky physics
            objToHoldRb.velocity = Vector3.zero;
            objToHoldRb.angularVelocity = Vector3.zero;
            
            if (objToHoldPickupCtrlr != null)
            {
                if (objToHoldPickupCtrlr.canPickedUp)
                {
                    objToHold.transform.position = grabPosition.position;
                }

                objToHoldPickupCtrlr.PickMeUp();
            }
        }
        else if (objToHold != null)
        {
            // Let go
            isHoldingObj = false;
            canPlayerGrabBuffer = false;
        }
        else {
            // Just not holding anything
            isHoldingObj = false;
        }


        // Grab buffer counter (Basically the time between when a player lets go and the object is dropped)
        if (!canPlayerGrabBuffer)
        {
            // Perform drop script for the obj
            if (objToHold != null)
            {
                if (objToHold.gameObject.GetComponent<PickupController>() != null)
                {
                    objToHold.gameObject.GetComponent<PickupController>().DropMe();
                }
                objToHold = null; // Done doing obj-drop specific script
            }

            // Do the counter 
            timeUntilCanGrabCounter += Time.deltaTime;

            if (timeUntilCanGrabCounter >= timeUntilCanGrab) // Time up, can grab now
            {
                canPlayerGrabBuffer = true; // important
                timeUntilCanGrabCounter = 0;
            }
        }
    }

    private void CheckKeyboardGrab()
    {
        // for Keyboard grab we check the rotation of the R-model (joints)
        isThumbInHoldPosition = IsFingerIsInHoldPositionZ(KeyboardGloveController._instance.thumb2Rotation, minThumbGrabRotation, maxThumbGrabRotation);
        isIndexInHoldPosition = IsFingerIsInHoldPositionZ(KeyboardGloveController._instance.index2Rotation, minFingerGrabRotation, maxFingerGrabRotation);
        isMiddleInHoldPosition = IsFingerIsInHoldPositionZ(KeyboardGloveController._instance.middle2Rotation, minFingerGrabRotation, maxFingerGrabRotation);
        isRingInHoldPosition = IsFingerIsInHoldPositionZ(KeyboardGloveController._instance.ring2Rotation, minFingerGrabRotation, maxFingerGrabRotation);
        isPinkyInHoldPosition = IsFingerIsInHoldPositionZ(KeyboardGloveController._instance.little2Rotation, minFingerGrabRotation, maxFingerGrabRotation);
    }
    private void CheckVRfreeGloveGrab()
    {
        // For VRfeeGloves we need to check the rotation of Z-finger-segments because the glove doesn't use the r-model (joint) rotations
        isThumbInHoldPosition = IsFingerIsInHoldPositionX(myVRfreeGloveScript.thumb2Transform, minThumbGrabRotation, maxThumbGrabRotation);
        isIndexInHoldPosition = IsFingerIsInHoldPositionZ(myVRfreeGloveScript.index1Transform, minFingerGrabRotation, maxFingerGrabRotation);
        isMiddleInHoldPosition = IsFingerIsInHoldPositionZ(myVRfreeGloveScript.middle1Transform, minFingerGrabRotation, maxFingerGrabRotation);
        isRingInHoldPosition = IsFingerIsInHoldPositionZ(myVRfreeGloveScript.ring1Transform, minFingerGrabRotation, maxFingerGrabRotation);
        isPinkyInHoldPosition = IsFingerIsInHoldPositionZ(myVRfreeGloveScript.little1Transform, minFingerGrabRotation, maxFingerGrabRotation);
    }

    private bool IsFingerIsInHoldPositionZ(Transform fingerTransform, float minAngle, float maxAngle)
    {
        if (IsBetween(fingerTransform.localEulerAngles.z, minAngle, maxAngle))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool IsFingerIsInHoldPositionX(Transform fingerTransform, float minAngle, float maxAngle)
    {
        if (IsBetween(fingerTransform.localEulerAngles.x, minAngle, maxAngle))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsBetween(float value, float min, float max)
    {
        return (value >= min && value <= max);
    }


    void OnTriggerEnter(Collider col)
    {
        if (isHoldingObj || !canPlayerGrabBuffer) { return; }
        objToHold = col.gameObject;
    }
    void OnTriggerStay(Collider col)
    {
        if (isHoldingObj || !canPlayerGrabBuffer) { return; }
        objToHold = col.gameObject;
    }
}
