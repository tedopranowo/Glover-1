using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerHelper
{
    public Transform FingerTransform;
    public Quaternion OriginalRotation;
}

public class KeyboardGloveController : MonoBehaviour
{
    public static KeyboardGloveController _instance;

    private VRfreeGlove myVRGloveScript;
    private Vector3 movement;

    public float playerSpeed = 1.0f;
    public GameObject originalPos;
    public float speedBackToOriginalPos = 3.0f;
    public float timeBufferMoveToOriginalPos = 1.5f;
    private float timeBufferToOriginalCounter = 0;

    private bool isPlayerMovementInactive = true;
    private bool isLeftClickDown = false;
    private bool isRightClickDown = false;
    private bool wasMiddleClicked = false;
    private bool isMiddleMouseScroll = false;

    public float fingerRotateSpeed = 5.0f;

    private KeyCode allFingersKey = KeyCode.T;
    private KeyCode thumbFingerKey = KeyCode.Space;
    private KeyCode indexFingerKey = KeyCode.F;
    private KeyCode middleFingerKey = KeyCode.D;
    private KeyCode ringFingerKey = KeyCode.S;
    private KeyCode pinkyFingerKey = KeyCode.A;
    
    private List<FingerHelper> indexFingerSegments;
    private List<FingerHelper> middleFingerSegments;
    private List<FingerHelper> ringFingerSegments;
    private List<FingerHelper> pinkyFingerSegments;
    private List<FingerHelper> thumbSegments;
    public float holdAngle = 65f;
    public float thumbHoldAngle = 30f;

    public Transform wristRotation;
    public Transform handRotation;
    public Transform thumb1Rotation;
    public Transform thumb2Rotation;
    public Transform thumb3Rotation;
    public Transform index1Rotation;
    public Transform index2Rotation;
    public Transform index3Rotation;
    public Transform middle1Rotation;
    public Transform middle2Rotation;
    public Transform middle3Rotation;
    public Transform ring1Rotation;
    public Transform ring2Rotation;
    public Transform ring3Rotation;
    public Transform little1Rotation;
    public Transform little2Rotation;
    public Transform little3Rotation;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }


    void Start()
    {
        myVRGloveScript = this.GetComponent<VRfreeGlove>();
        // Use keyboard instead of VR glove
        if (MyGameManager._instance.isKeyboardControls && myVRGloveScript != null)
        {
            myVRGloveScript.enabled = false;
            this.transform.parent = Camera.main.transform;
            this.transform.rotation = originalPos.transform.rotation;
        }
        else if (myVRGloveScript != null)
        {
            myVRGloveScript.enabled = true;
        }
        
        thumbSegments = new List<FingerHelper>();
        thumbSegments.Add(new FingerHelper() { FingerTransform = thumb1Rotation, OriginalRotation = thumb1Rotation.localRotation });
        thumbSegments.Add(new FingerHelper() { FingerTransform = thumb2Rotation, OriginalRotation = thumb2Rotation.localRotation });
        thumbSegments.Add(new FingerHelper() { FingerTransform = thumb3Rotation, OriginalRotation = thumb3Rotation.localRotation });

        indexFingerSegments = new List<FingerHelper>();
        indexFingerSegments.Add(new FingerHelper() { FingerTransform = index1Rotation, OriginalRotation = index1Rotation.localRotation });
        indexFingerSegments.Add(new FingerHelper() { FingerTransform = index2Rotation, OriginalRotation = index2Rotation.localRotation });
        indexFingerSegments.Add(new FingerHelper() { FingerTransform = index3Rotation, OriginalRotation = index3Rotation.localRotation });

        middleFingerSegments = new List<FingerHelper>();
        middleFingerSegments.Add(new FingerHelper() { FingerTransform = middle1Rotation, OriginalRotation = middle1Rotation.localRotation });
        middleFingerSegments.Add(new FingerHelper() { FingerTransform = middle2Rotation, OriginalRotation = middle2Rotation.localRotation });
        middleFingerSegments.Add(new FingerHelper() { FingerTransform = middle3Rotation, OriginalRotation = middle3Rotation.localRotation });

        ringFingerSegments = new List<FingerHelper>();
        ringFingerSegments.Add(new FingerHelper() { FingerTransform = ring1Rotation, OriginalRotation = ring1Rotation.localRotation });
        ringFingerSegments.Add(new FingerHelper() { FingerTransform = ring2Rotation, OriginalRotation = ring2Rotation.localRotation });
        ringFingerSegments.Add(new FingerHelper() { FingerTransform = ring3Rotation, OriginalRotation = ring3Rotation.localRotation });

        pinkyFingerSegments = new List<FingerHelper>();
        pinkyFingerSegments.Add(new FingerHelper() { FingerTransform = little1Rotation, OriginalRotation = little1Rotation.localRotation });
        pinkyFingerSegments.Add(new FingerHelper() { FingerTransform = little2Rotation, OriginalRotation = little2Rotation.localRotation });
        pinkyFingerSegments.Add(new FingerHelper() { FingerTransform = little3Rotation, OriginalRotation = little3Rotation.localRotation });
        
    }
    
    void Update()
    {
        if (!MyGameManager._instance.isKeyboardControls) { return; }

        float inputZ = 0;
        float inputY = 0;

        // Left Click
        if (Input.GetMouseButton(0)) 
        {
            inputY = -1; // move downwards
            isLeftClickDown = true;
        }
        else
        {
            isLeftClickDown = false;
        }

        // Right Click
        if (Input.GetMouseButton(1))
        {
            inputZ = 1;
            isRightClickDown = true;
        }
        else
        {
            isRightClickDown = false;
        }

        // Middle Click
        if (Input.GetMouseButton(2)) 
        {
            wasMiddleClicked = true;
            timeBufferToOriginalCounter = 0;
        }

        //// Scroll Wheel
        //if ((this.transform.position.z >= originalPos.position.z) && Input.GetAxis("Mouse ScrollWheel") != 0)
        //{
        //    isMiddleMouseScroll = true;
        //    this.transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel"));
        //}

        // Rotate fingers on key press
        CheckFingerKeyDownAndRotate(indexFingerKey, indexFingerSegments, holdAngle);
        CheckFingerKeyDownAndRotate(middleFingerKey, middleFingerSegments, holdAngle);
        CheckFingerKeyDownAndRotate(ringFingerKey, ringFingerSegments, holdAngle);
        CheckFingerKeyDownAndRotate(pinkyFingerKey, pinkyFingerSegments, holdAngle);
        CheckFingerKeyDownAndRotate(thumbFingerKey, thumbSegments, thumbHoldAngle);
        
        // Move back to orignal position
        if (!isRightClickDown && !isLeftClickDown && !isMiddleMouseScroll)
        {
            isPlayerMovementInactive = true;
            timeBufferToOriginalCounter += Time.deltaTime;

            if (wasMiddleClicked || (this.transform.position != originalPos.transform.position && timeBufferToOriginalCounter >= timeBufferMoveToOriginalPos))
            {
                this.transform.position = Vector3.MoveTowards(transform.position, originalPos.transform.position, speedBackToOriginalPos * Time.deltaTime);
            }
        }
        else
        {
            isPlayerMovementInactive = false;
            timeBufferToOriginalCounter = 0;
            wasMiddleClicked = false;
            isMiddleMouseScroll = false;
        }

        // Move
        Move(inputZ, inputY);
    }


    private void CheckFingerKeyDownAndRotate(KeyCode fingerKey, List<FingerHelper> fingerSegments, float myHoldAngle)
    {
        if (Input.GetKey(fingerKey) || Input.GetKey(allFingersKey))
        {
            RotateFinger(fingerSegments, false, myHoldAngle);
        }
        else
        {
            RotateFinger(fingerSegments, true, myHoldAngle);
        }
    }

    private void RotateFinger(List<FingerHelper> myFingerSegments, bool toOriginalRotation, float myHoldAngle)
    {
        // rotate fingers
        foreach (FingerHelper fingerSegment in myFingerSegments)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 90, (myHoldAngle));
            
            if (toOriginalRotation)
            {
                targetRotation = fingerSegment.OriginalRotation;
            }
            //fingerSegment.localRotation = Quaternion.LookRotation(newDir);
            //Quaternion.RotateTowards(fingerSegment.localRotation, Quaternion.Euler(0, (-1 * holdAngle), fingerSegment.localRotation.z), fingerRotateSpeed * Time.deltaTime);

            fingerSegment.FingerTransform.localRotation = Quaternion.Slerp(fingerSegment.FingerTransform.localRotation, targetRotation, Time.deltaTime * fingerRotateSpeed);
        }
    }


    private void Move(float z, float y)
    {
        // Set the movement vector based on the axis input.
        movement.Set(0f, y, z);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * playerSpeed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        this.transform.localPosition += (movement);
    }


}
