using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMusic : MonoBehaviour
{
    [SerializeField] private Slider volume;
    public void ChangeVolume()
    {
        AudioListener.volume = volume.value;
    }
}