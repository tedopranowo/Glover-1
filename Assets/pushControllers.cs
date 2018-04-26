using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushControllers : MonoBehaviour {
    // Use this for initialization
    public string winningCon;
    void Start () {
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Circle")){
                child.gameObject.AddComponent<BoxCollider>();
                var trigger = child.gameObject.GetComponent<BoxCollider>();
                trigger.isTrigger = true;
                child.gameObject.AddComponent<Rigidbody>();
                var body = child.gameObject.GetComponent<Rigidbody>();
                body.useGravity = false;
                child.gameObject.AddComponent<pushButton>();
            }
            
        }
    }
	
	// Update is called once per frame
	void Update () {
        var total = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf == false)
            {
                Stack myStack = new Stack();

                total += 1;
            }
            if (total >= 6)
            {
                foreach (Transform children in transform)
                {
                    children.gameObject.SetActive(true);
                    winningCon = "";
                }
            }
        }

    }
}
