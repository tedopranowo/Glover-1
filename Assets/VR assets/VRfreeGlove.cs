using UnityEngine;
using System.Runtime.InteropServices;

public struct HandData {
    public int timeSinceLastDeviceData;
    public int timeSinceLastLeftHandData;
    public int timeSinceLastRightHandData;
    public float isWristPositionValid;
    public Vector3 wristPosition;
    public Quaternion wristRotation;
    public Quaternion handRotation;
    public Quaternion thumb1Rotation;
    public Quaternion thumb2Rotation;
    public Quaternion thumb3Rotation;
    public Quaternion index1Rotation;
    public Quaternion index2Rotation;
    public Quaternion index3Rotation;
    public Quaternion middle1Rotation;
    public Quaternion middle2Rotation;
    public Quaternion middle3Rotation;
    public Quaternion ring1Rotation;
    public Quaternion ring2Rotation;
    public Quaternion ring3Rotation;
    public Quaternion little1Rotation;
    public Quaternion little2Rotation;
    public Quaternion little3Rotation;
}

public class VRfreeGlove : MonoBehaviour {

    //access to the VRfree driver functions
    [DllImport("VRfree")] private static extern void vrfree_start();
    [DllImport("VRfree")] private static extern void vrfree_calibrate(bool leftHand, Vector3 direction);
    [DllImport("VRfree")] private static extern bool vrfree_getHandData(bool leftHand, Vector3 cameraPosition, Quaternion cameraRotation, byte[] result);
    [DllImport("VRfree")] private static extern byte vrfree_statusCode();
    [DllImport("VRfree")] private static extern void vrfree_release();
    private void start() { vrfree_start(); }
    public void calibrate() { vrfree_calibrate(isLeftHand, lastCalibrationDirection); }
    private bool getHandData() {
        bool result = vrfree_getHandData(isLeftHand, cameraTransform.position, cameraTransform.rotation, handDataBuffer);
        handData = (HandData)Marshal.PtrToStructure(handDataHandle.AddrOfPinnedObject(), typeof(HandData));
        return result;
    }
    private byte statusCode() { return vrfree_statusCode(); }
    private void release() { vrfree_release(); }

    //constants
    public static double START_FADE_TIME_IN_SECONDS = 0.5;
    public static double FADE_DURATION_IN_SECONDS = 1.0;

    //editor input
    public bool isLeftHand;             //use this flag to tell this instance which data to fetch
    public Transform cameraTransform;   //the camera transform is required to position and orient the hands in world coordinates
    public Material[] material;         //[OPTIONAL] set materials which will be faded out when connection is lost for too long

    //editot output
    public string deviceStatus;
    public string deviceError;
    public int timeSinceLastDeviceData;
    public int timeSinceLastLeftHandData;
    public int timeSinceLastRightHandData;
    public bool isWristPositionValid;
    public Transform wristTransform;    //this transform will receive position and rotation, while all others just receive rotations
    public Transform handTransform;
    public Transform thumb1Transform;
    public Transform thumb2Transform;
    public Transform thumb3Transform;
    public Transform index1Transform;
    public Transform index2Transform;
    public Transform index3Transform;
    public Transform middle1Transform;
    public Transform middle2Transform;
    public Transform middle3Transform;
    public Transform ring1Transform;
    public Transform ring2Transform;
    public Transform ring3Transform;
    public Transform little1Transform;
    public Transform little2Transform;
    public Transform little3Transform;

    //some vrFree device status flags
    private static byte NOT_CONNECTED       = 0x01;
    private static byte CONNECTING          = 0x02;
    private static byte CONNECTION_FAILED   = 0x04; //will be cleared on reconnect
    private static byte START_STREAMING     = 0x08;
    private static byte STREAMING           = 0x10;
    private static byte READING_FAILED      = 0x20; //will be cleared on next valid read
    private static byte INVALID_ARGUMENTS   = 0x40;	//will be cleared on getHandData with valid input

    //internally used variables to get data from the driver
    private HandData handData;
    private byte[] handDataBuffer;
    private GCHandle handDataHandle;

    //internally used variables to calibrate the hands
    private bool calibrationPoseShown;
    private Vector3 lastCalibrationDirection;
    private Quaternion lastCalibrationRotation;
    private Vector3 calibrationPosition;

    //constructor
    VRfreeGlove() {
        //create some variables used to communicate with the VRfree driver
        handData = new HandData();
        handDataBuffer = new byte[Marshal.SizeOf(handData)];
        handDataHandle = GCHandle.Alloc(handDataBuffer, GCHandleType.Pinned);
        calibrationPoseShown = false;
        lastCalibrationDirection = Vector3.right;
        calibrationPosition = Vector3.zero;
    }

    //start-up function
    void Start () {
        //start the VRfree library to indicate that this instance is interested in VRfree data
        start();
    }

    //shut-down function
    public void OnDestroy() {
        //release the VRfree library to indicate that this instance does not use it anymore
        release();

        //release the handles
        handDataHandle.Free();
    }

    public void showCalibrationPose() {
        if(!calibrationPoseShown) {
            //use the current head direction as direction for the calibration pose
            lastCalibrationDirection = cameraTransform.rotation * Vector3.forward;
            lastCalibrationDirection.y = 0;
            lastCalibrationDirection = lastCalibrationDirection.normalized;
            lastCalibrationRotation = Quaternion.AngleAxis(Mathf.Acos(Vector3.Dot(Vector3.forward, lastCalibrationDirection))*Mathf.Rad2Deg, Vector3.Cross(Vector3.forward, lastCalibrationDirection));

            //compute a position for the calibration pose somewhere in front of the head
            calibrationPosition.x = isLeftHand ? -4.5f : 4.5f;
            calibrationPosition.y = -6.0f;
            calibrationPosition.z = 6.0f;
            calibrationPosition = cameraTransform.position + lastCalibrationRotation * calibrationPosition;

            calibrationPoseShown = true;
        }
    }

    public void hideCalibrationPose() {
        calibrationPoseShown = false;
    }

    //update the output
    void Update () {
        //print a readable status in the editor
        byte code = statusCode();
        if ((code & NOT_CONNECTED) > 0) {
            deviceStatus = "Please plug-in the VRfree device";
            NotificationHandler.instance.Notify(deviceStatus);
        } else if ((code & CONNECTING) > 0) {
            deviceStatus = "Connecting to VRfree device...";
            NotificationHandler.instance.Notify(deviceStatus);
        } else if ((code & START_STREAMING) > 0) {
            deviceStatus = "Starting VRfree data stream...";
            NotificationHandler.instance.Notify(deviceStatus);
        } else if ((code & STREAMING) > 0) {
            deviceStatus = "Streaming VRfree data...";
            NotificationHandler.instance.CloseNotification();
        } else {
            deviceStatus = "unknown";
        }

        //check for errors and give them out
        if ((code & CONNECTION_FAILED) > 0) {
            deviceError = "Connection failed, please re-connect the device";
        } else if ((code & READING_FAILED) > 0) {
            deviceError = "Reading failed, please restart the device";
        }  else if ((code & INVALID_ARGUMENTS) > 0) {
            deviceError = "Invalid arguments, please pass correct data to the driver";
        } else {
            deviceError = "none";
        }

        //get the current hand data
        if (!getHandData()) {
            //something went wrong and we will read the error in the next frame
            return;
        }

        //assign the data to the output
        timeSinceLastDeviceData = handData.timeSinceLastDeviceData;
        timeSinceLastLeftHandData = handData.timeSinceLastLeftHandData;
        timeSinceLastRightHandData = handData.timeSinceLastRightHandData;
        isWristPositionValid = handData.isWristPositionValid > 0;

        if(calibrationPoseShown) {
            if (wristTransform != null) { wristTransform.position = calibrationPosition; wristTransform.rotation = lastCalibrationRotation; }
            if (handTransform != null) { handTransform.rotation = lastCalibrationRotation; }
			if (thumb1Transform != null) { thumb1Transform.rotation = lastCalibrationRotation*Quaternion.Euler(9.823001f, -22.385f, 69.16901f); }
            if (thumb2Transform != null) { thumb2Transform.rotation = lastCalibrationRotation; }
            if (thumb3Transform != null) { thumb3Transform.rotation = lastCalibrationRotation; }
            if (index1Transform != null) { index1Transform.rotation = lastCalibrationRotation; }
            if (index2Transform != null) { index2Transform.rotation = lastCalibrationRotation; }
            if (index3Transform != null) { index3Transform.rotation = lastCalibrationRotation; }
            if (middle1Transform != null) { middle1Transform.rotation = lastCalibrationRotation; }
            if (middle2Transform != null) { middle2Transform.rotation = lastCalibrationRotation; }
            if (middle3Transform != null) { middle3Transform.rotation = lastCalibrationRotation; }
            if (ring1Transform != null) { ring1Transform.rotation = lastCalibrationRotation; }
            if (ring2Transform != null) { ring2Transform.rotation = lastCalibrationRotation; }
            if (ring3Transform != null) { ring3Transform.rotation = lastCalibrationRotation; }
            if (little1Transform != null) { little1Transform.rotation = lastCalibrationRotation; }
            if (little2Transform != null) { little2Transform.rotation = lastCalibrationRotation; }
            if (little3Transform != null) { little3Transform.rotation = lastCalibrationRotation; }

            //make the hand visible during calibration
            if (material != null)
            {
                double elapsedTime = (isLeftHand ? (double)timeSinceLastLeftHandData : (double)timeSinceLastRightHandData) / 1000.0;
                for (int i = 0; i < material.Length; i++)
                {
                    if (material[i] == null) { continue; }
                    material[i].SetFloat("_ZWrite", 1.0f);//keep writing to the z-buffer even if it is in fade mode
                    material[i].SetFloat("_Mode", 0.0f);//set opaque mode
                    material[i].color = new Color(material[i].color.r, material[i].color.g, material[i].color.b, 1.0f);
                }
            }
        }
        else {
            if (wristTransform != null) { wristTransform.position = handData.wristPosition; wristTransform.rotation = handData.wristRotation; wristTransform.position += wristTransform.forward * 7.0f; }
            if (handTransform != null) { handTransform.rotation = handData.handRotation; }
            if (thumb1Transform != null) { thumb1Transform.rotation = handData.thumb1Rotation; }
            if (thumb2Transform != null) { thumb2Transform.rotation = handData.thumb2Rotation; }
            if (thumb3Transform != null) { thumb3Transform.rotation = handData.thumb3Rotation; }
            if (index1Transform != null) { index1Transform.rotation = handData.index1Rotation; }
            if (index2Transform != null) { index2Transform.rotation = handData.index2Rotation; }
            if (index3Transform != null) { index3Transform.rotation = handData.index3Rotation; }
            if (middle1Transform != null) { middle1Transform.rotation = handData.middle1Rotation; }
            if (middle2Transform != null) { middle2Transform.rotation = handData.middle2Rotation; }
            if (middle3Transform != null) { middle3Transform.rotation = handData.middle3Rotation; }
            if (ring1Transform != null) { ring1Transform.rotation = handData.ring1Rotation; }
            if (ring2Transform != null) { ring2Transform.rotation = handData.ring2Rotation; }
            if (ring3Transform != null) { ring3Transform.rotation = handData.ring3Rotation; }
            if (little1Transform != null) { little1Transform.rotation = handData.little1Rotation; }
            if (little2Transform != null) { little2Transform.rotation = handData.little2Rotation; }
            if (little3Transform != null) { little3Transform.rotation = handData.little3Rotation; }

            //update the material transparency
            if (material != null)
            {
                double elapsedTime = (isLeftHand ? (double)timeSinceLastLeftHandData : (double)timeSinceLastRightHandData) / 1000.0;
                for (int i = 0; i < material.Length; i++)
                {
                    if (material[i] == null) { continue; }
                    material[i].SetFloat("_ZWrite", 1.0f);//keep writing to the z-buffer even if it is in fade mode
                    if (elapsedTime >= START_FADE_TIME_IN_SECONDS + FADE_DURATION_IN_SECONDS)
                    {
                        material[i].SetFloat("_Mode", 2.0f);//set fade mode
                        material[i].color = new Color(material[i].color.r, material[i].color.g, material[i].color.b, 0.0f);
                    }
                    else if (elapsedTime < START_FADE_TIME_IN_SECONDS)
                    {
                        material[i].SetFloat("_Mode", 0.0f);//set opaque mode
                        material[i].color = new Color(material[i].color.r, material[i].color.g, material[i].color.b, 1.0f);
                    }
                    else
                    {
                        material[i].SetFloat("_Mode", 2.0f);//set fade mode
                        material[i].color = new Color(material[i].color.r, material[i].color.g, material[i].color.b, 1.0f - (float)((elapsedTime - START_FADE_TIME_IN_SECONDS) / FADE_DURATION_IN_SECONDS));
                    }
                }
            }
        }
    }

    //reduce latency by updating positions in the late update too
    void LateUpdate() {
        Update();
    }
}