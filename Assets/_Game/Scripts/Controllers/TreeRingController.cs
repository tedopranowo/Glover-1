using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRingController : MonoBehaviour {

//	public GameObject innerCir_0;
//	public GameObject innerCir_1;
//	public GameObject innerCir_2;
//	public GameObject innerCir_3;
//	public GameObject innerCir_4;

	public bool isTurn = false;

	// Use this for initialization
	void Start () {
		
		transform.Rotate(Vector3.up, Random.Range(0,360));
//		RandRotation (innerCir_0);
//		RandRotation (innerCir_1);
//		RandRotation (innerCir_2);
//		RandRotation (innerCir_3);
//		RandRotation (innerCir_4);
	}

	void Update (){
		if(isTurn){transform.Rotate (Vector3.up, Time.deltaTime * 50);}
	}
    void OnTriggerEnter(Collider other){
		if (other.gameObject.layer == 8) {
			isTurn = true;
		}
	}

	void OnTriggerExit(){
		isTurn = false;
	}
	
	// Update is called once per frame
//	void Update () {
//		innerCir_1.transform.Rotate(Vector3.up, Time.deltaTime * Random.Range(10,100));
//		innerCir_2.transform.Rotate(Vector3.up, Time.deltaTime * Random.Range(10,100));
//		innerCir_3.transform.Rotate(Vector3.up, Time.deltaTime * Random.Range(10,100));
//	}

//	void RandRotation(GameObject innerCir) {

//		var rotationVector = innerCir.transform.rotation.eulerAngles;
//		var x = Random.Range(0,360);
//		Debug.Log (x);
//		innerCir.transform.Rotate(Vector3.up, x);
//		rotationVector.x = Random.rotation.x;
//		rotationVector.y = 0;
//		rotationVector.z = 0;

//		innerCir.transform.rotation = Quaternion.Euler (rotationVector);
//		innerCir.transform.rotation = Quaternion.Euler (Random.rotation);
//		float x = Random.rotation.x * 100f;
//		Debug.Log (x);
//		innerCir.transform.Rotate(x, 0, 0);


//	}
}
