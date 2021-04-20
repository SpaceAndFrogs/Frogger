using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    int indexOfCurrentSong = 0;
    public AudioSource[] audioClips;
    int amountOfClips;
    void Start()
    {
        amountOfClips = audioClips.Length;
        indexOfCurrentSong = Random.Range(0, amountOfClips);
        audioClips[indexOfCurrentSong].Play();
    }

   
    void Update()
    {
        if(!audioClips[indexOfCurrentSong].isPlaying)
        {
            indexOfCurrentSong++;
            if(indexOfCurrentSong == amountOfClips)
            {
                indexOfCurrentSong = 0;
            }
            audioClips[indexOfCurrentSong].Play();
        }
    }
}
