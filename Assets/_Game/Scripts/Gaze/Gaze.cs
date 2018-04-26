using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaze : MonoBehaviour
{
    private GazeInteraction m_objectBeingGazed;
    private int m_totalGazeTime;

    private void FixedUpdate()
    {
        RaycastHit hitObject;

        //If the raycast hit into an object
        if (Physics.Raycast(transform.position, transform.forward, out hitObject, 200.0f, 8))
        {
            //Get the interaction being hit
            GazeInteraction hitInteraction = hitObject.transform.GetComponent<GazeInteraction>();

            //If the raycast hit is not the same object as previous object
            if (m_objectBeingGazed != hitInteraction)
            {
                m_objectBeingGazed = hitInteraction;
                m_totalGazeTime = 0;
            }
            //If the raycast hit the same object, increase the timer countdown
            else
            {
                ++m_totalGazeTime;
            }
        }
        //If the raycast doesn't hit anything
        else
        {
            //Reset all the counter
            m_objectBeingGazed = null;
            m_totalGazeTime = 0;
        }

        //Check if the required gaze time is fulfilled
        if (m_objectBeingGazed.gazeDuration <= m_totalGazeTime)
        {
            m_objectBeingGazed.TriggerEvent();
            m_totalGazeTime = 0;
        }
    }
}