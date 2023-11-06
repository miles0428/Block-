using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource audioSource ;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Audioevent.AudioEvent.AddListener(PlayAudio);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        
    }

    void PlayAudio(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
