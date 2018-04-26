using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : PickupController
{
    public bool isPuzzleFinished = false;

    public override void PickMeUp()
    {
        // stop bad hand physics by setting the layer
        isGrabbed = true;
        this.gameObject.layer = LayerMask.NameToLayer("ObjHandHold");
    }
    public override void DropMe()
    {
        // stop bad hand physics by setting the layer
        isGrabbed = false;
        this.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    void Update()
    {
        if (isGrabbed)
        {
            RotateFollowPlayer();
        }
    }

    private void RotateFollowPlayer()
    {
        Quaternion targetRotation = Quaternion.Euler(this.transform.localRotation.x, 90, -180);
        this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, targetRotation, Time.deltaTime * 2.0f);
        if (this.transform.localRotation.z <= (targetRotation.z+20))
        {
            isPuzzleFinished = true;
        }
    }
}
