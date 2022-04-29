using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    [SerializeField] Slider slider;

    public bool isOpened = false; //Открыто ли меню
    public float volume = 0; //Громкость
    public AudioMixer audioMixer; //Регулятор громкости
    public Dropdown resolutionDropdown; //Список с разрешениями для игры
    private Resolution[] resolutions; //Список доступных разрешений
    private int currResolutionIndex = 0; //Текущее разрешение


    void Start()
    {
    
    }

    void Update()
    {
    
    } 
}