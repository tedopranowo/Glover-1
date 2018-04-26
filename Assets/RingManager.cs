using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviour {

	public bool isMatch = false;
	// Use this for initialization
	void Start () {
//		bool cir_1 = GetComponent<TreeRingController> ().isTurn;
//		var x = GetComponentsInChildren<TreeRingController>();
//		for (int i = 0; i < x.Length; i++) {
//			
//		}
//		Debug.Log (cir_1);
	}
	
	// Update is called once per frame
	void Update () {
		var x = GetComponentsInChildren<TreeRingController>();
		for (int i = 0; i < x.Length; i++) {
			if (Mathf.Abs (x [i].transform.rotation.eulerAngles.x / 90) > 1.2) {
				return;
			}
		}

		isMatch = true;

	}
}
