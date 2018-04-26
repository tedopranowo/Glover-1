using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Radio : MonoBehaviour {

    [SerializeField] private AudioClip[] m_audioClips;
    [SerializeField] private UnityEvent m_eventOnAllAudioCompleted;

    private void Start()
    {
        StartCoroutine(PlayAllAudiosConsecutively());
    }
   
    private IEnumerator PlayAllAudiosConsecutively()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        for (int i=0; i<m_audioClips.Length; ++i)
        {
            audioSource.clip = m_audioClips[i];
            audioSource.Play();

            yield return new WaitForSeconds(audioSource.clip.length);
        }

        m_eventOnAllAudioCompleted.Invoke();
    }
}