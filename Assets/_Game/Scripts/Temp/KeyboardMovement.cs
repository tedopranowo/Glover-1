using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("w")) { transform.position = transform.position + transform.forward * Time.deltaTime; }
        else if (Input.GetKey("a")){ transform.position = transform.position + transform.right * (-1) * Time.deltaTime; }
        else if (Input.GetKey("d")){ transform.position = transform.position + transform.right * Time.deltaTime; }
        else if (Input.GetKey("s")){ transform.position = transform.position + transform.forward * (-1) * Time.deltaTime; }
        else if (Input.GetKey("q")){ transform.position = transform.position + transform.up * (-1) * Time.deltaTime; }
        else if (Input.GetKey("e")){ transform.position = transform.position + transform.up * Time.deltaTime; }

    }
}
