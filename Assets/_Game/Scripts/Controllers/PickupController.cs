using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public bool isGrabbed = false;
    public bool canPickedUp = true;
    public bool isPossibleGrab;
    //public int originalLayerMask; // TODO
    
    void Start()
    {
        //originalLayerMask = this.gameObject.layer; // TODO
    }
    
    void Update()
    {
    }

    public virtual void PickMeUp()
    {
        // stop bad hand physics by setting the layer
        isGrabbed = true;
        this.gameObject.layer = LayerMask.NameToLayer("ObjHandHold");
    }
    public virtual void DropMe()
    {
        // stop bad hand physics by setting the layer
        isGrabbed = false;
        this.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Hand") && this.gameObject.layer != LayerMask.NameToLayer("ObjHandHold"))
        {
            // TODO: change color or give a nice looking outline shader here maybe?
            isPossibleGrab = true;
        }
    }

}
