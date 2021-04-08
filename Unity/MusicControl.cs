using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicControl : MonoBehaviour {

    public Slider slider;
    public AudioSource audioSource;
    public void Start()
    {
        audioSource.volume =slider.value;
    }

    public void changeVolume()
    {
        audioSource.volume = slider.value;
    }
}

