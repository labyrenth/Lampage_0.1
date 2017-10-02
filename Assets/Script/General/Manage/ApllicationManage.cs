﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ApllicationManage : MonoBehaviour {

    private void Awake()
    {
        Application.targetFrameRate =60;
    }

    private void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif
    }

    private void OnApplicationQuit()
    {
        //auth.SignOut();
    }
}
