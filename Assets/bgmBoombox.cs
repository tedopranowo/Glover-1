using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmBoombox : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("sound check");	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        var audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
    
}
