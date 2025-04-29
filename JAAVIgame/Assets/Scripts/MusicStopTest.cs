using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStopTest : MonoBehaviour
{
    void Start()
    {
        GameObject music = GameObject.Find("MusicPlayer");
        if (music != null)
        {
            Destroy(music);
        }
    }
}