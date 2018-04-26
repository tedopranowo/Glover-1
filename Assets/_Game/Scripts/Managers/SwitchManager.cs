using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour {

	public bool isAllSwitchOn = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var switchList = GetComponentsInChildren<SwitchController>();
		for (int i = 0; i < switchList.Length; i++) {
			if (!switchList[i].isTurnOn) {
				return;
			}
		}

		isAllSwitchOn = true;
	}
}
