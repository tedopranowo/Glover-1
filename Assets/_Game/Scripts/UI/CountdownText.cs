using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Text))]
public class CountdownText : MonoBehaviour
{
    [SerializeField] string m_textString;
    [SerializeField] int m_duration;
    [SerializeField] UnityEvent m_onCountdownFinished;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Countdown(m_duration));
    }

    //Start the timer countdown then invoke the countdown finished event on completion
    private IEnumerator Countdown(int seconds)
    {
        Text m_textComponent = GetComponent<Text>();
        while (seconds > 0)
        {
            m_textComponent.text = m_textString + seconds.ToString();
            yield return new WaitForSeconds(1.0f);
            --seconds;
        }

        m_onCountdownFinished.Invoke();
    }

}