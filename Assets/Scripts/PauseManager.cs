﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    public UnityEvent onPause = new UnityEvent();
    public UnityEvent onUnpause = new UnityEvent(); 

    public bool paused = false;

    private void Start() {
        
    }

    public void TogglePause()
    {
        if (paused) {
            Unpause();
        } else {
            Pause();
        }

    }

    public void Pause()
    {
        onPause.Invoke();
        paused = true;
    }

    public void Unpause()
    {
        onUnpause.Invoke();
        paused = false;
    }

}
