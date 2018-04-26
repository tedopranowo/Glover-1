using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GazeInteraction : MonoBehaviour {

    [Tooltip("Required gaze duration to trigger the event")]
    [SerializeField]
    private int m_gazeDuration;

    [Tooltip("The event being triggered if this object is gazed for the specified duration")]
    [SerializeField]
    private UnityEvent m_onGazeCompleted;

    //Setter and getters for the gaze duration
    public int gazeDuration{
        set { m_gazeDuration = value; }
        get { return m_gazeDuration; }
    }
    
    //Trigger the event on gaze completion
    public void TriggerEvent()
    {
        m_onGazeCompleted.Invoke();
    }
}
