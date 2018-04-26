using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour {

	public float smooth = 2.0F;
	public bool isTurnOn = false;
	private bool isLocked = false;

	void OnTriggerEnter(){
		Debug.Log (tag);
		if (isLocked){return;}

		if (tag == "UpDownSwitch") {
			var rotationVector = transform.rotation.eulerAngles;

			if (!isTurnOn) {
				rotationVector.z = -130;
				isTurnOn = true;
			} else {
				rotationVector.z = -60;
				isTurnOn = false;
			}

			transform.rotation = Quaternion.Euler (rotationVector);

		} else if (tag == "LeftRightSwitch"){
			var rotationVector = transform.rotation.eulerAngles;

			if (!isTurnOn) {
				rotationVector.z = -30;
				isTurnOn = true;
			} else {
				rotationVector.z = 30;
				isTurnOn = false;
			}
			transform.rotation = Quaternion.Euler (rotationVector);
		}

		isLocked = true;
	}

	void OnTriggerExit(){
		Debug.Log ("Exit lock");
		isLocked = false;
	}

}
