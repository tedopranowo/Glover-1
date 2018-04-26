using UnityEngine;
using UnityEngine.Events;

public class Calibration : MonoBehaviour {

    public UnityEvent showRightHandCalibration;
    public UnityEvent hideRightHandCalibration;
    public UnityEvent calibrateRightHand;

    private bool calibrationOngoing;

    void Start () {
        calibrationOngoing = false;
    }
	
	void Update () {
        if (Input.GetKeyDown("r")) {
            if(!calibrationOngoing) {
                showRightHandCalibration.Invoke();
                calibrationOngoing = true;
            }
            else {
                hideRightHandCalibration.Invoke();
                calibrateRightHand.Invoke();
                calibrationOngoing = false;
            }
            
        }
    }
}
