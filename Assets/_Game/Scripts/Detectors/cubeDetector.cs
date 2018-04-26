using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeDetector : MonoBehaviour {

	Rigidbody rb;
	public GameObject light;
	public GameObject holder;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
//		rb.transform.position = holder.transform.position;
	}


	//void OnTriggerEnter(Collider other){
	//	if (other.gameObject.CompareTag ("Holder")) {
	//		light.GetComponent<Renderer> ().material.color = Color.green;
	//	}
 //   }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Holder"))
        {
            light.GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
