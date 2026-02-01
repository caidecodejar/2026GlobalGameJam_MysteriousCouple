using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    public Button exitButton;
    public Button RestartButton;

    public void Exit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneMgr.Instance.LoadScene("StartMenu", null);
    }
    
}
