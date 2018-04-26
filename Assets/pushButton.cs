using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class pushButton : MonoBehaviour {
    string locked = "false";
    public int won = 0;
    // Use this for initialization
    void Start () {
    
    }
	// Update is called once per frame
	void Update () {
        if (this.gameObject.GetComponentInParent<pushControllers>().winningCon == "6")
        {
            for (float count = 0; count < 100; count += 0.01f)
            {
                GameObject go = GameObject.Find("solodoor");
                go.transform.position = new Vector3(go.transform.position.x + 0.0001f, go.transform.position.y, go.transform.position.z);
                GameObject go2 = GameObject.Find("solodoor2");
                go2.transform.position = new Vector3(go2.transform.position.x - 0.0001f, go2.transform.position.y, go2.transform.position.z);
            }
            this.gameObject.GetComponentInParent<pushControllers>().winningCon = "";
        }
        


    }
    void OnTriggerEnter(Collider other)
    {
        if (this.locked == "false")
        {
            this.gameObject.SetActive(false);
            this.gameObject.GetComponentInParent<pushControllers>().winningCon += this.gameObject.name[this.gameObject.name.Length - 1];
            Debug.Log(this.gameObject.GetComponentInParent<pushControllers>().winningCon);
            //Debug.Log("6");
            //Debug.Log(this.gameObject.GetComponentInParent<pushControllers>().winningCon == "6");
            //if (this.gameObject.GetComponentInParent<pushControllers>().winningCon == "6")
            //{
            //    Debug.Log("hello");
            //    won = 1;
            //    this.locked = "forever";
            //    return;
            //}
            this.locked = "true";

            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (this.locked == "true")
        {
            this.locked = "false";
        }
        
    }
}
