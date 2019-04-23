using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    public AudioSource source;
    public AudioClip backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        source.clip = backgroundMusic;
        source.Play();
    }
}
