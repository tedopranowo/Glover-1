using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristIsValidIndicator : MonoBehaviour {

    [SerializeField] private VRfreeGlove m_vrFreeGlove;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (m_vrFreeGlove.isWristPositionValid)
            gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        else
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
	}
}
