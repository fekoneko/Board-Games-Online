using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Audio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSrc;
    float musicVolume = 0f;
    
    void Update()
    {
        audioSrc.volume = musicVolume;
    }
}
