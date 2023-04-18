using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    AudioSource audioPlay;
    private void Awake()
    {
        audioPlay = GetComponent<AudioSource>();
    }

    public void play()
    {
        audioPlay.Play();
    }
}
