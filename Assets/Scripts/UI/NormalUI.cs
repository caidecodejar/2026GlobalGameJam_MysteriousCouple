using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NormalUI : MonoBehaviour
{
    public TextMeshProUGUI maskNums;
    public TextMeshProUGUI senergyNums;
    public TextMeshProUGUI cnergyNums;

    private void Start()
    {
        UpdateAllUI();
        EventManager.AddListener(EventType.UpdateAllUI, UpdateAllUI);
        EventManager.AddListener(EventType.ResetAllUI, ResetAllUI);
    }

    private void ResetAllUI()
    {
        BuildManager.Instance.cenergy = BuildManager.Instance.privateCenergy;
        BuildManager.Instance.senery = BuildManager.Instance.privateSenergy;
        maskNums.text = WorldManager.Instance.limit.ToString();
        senergyNums.text = BuildManager.Instance.privateSenergy.ToString();
        cnergyNums.text = BuildManager.Instance.privateCenergy.ToString();
    }

    private void UpdateAllUI()
    {
        maskNums.text = WorldManager.Instance.limit.ToString();
        senergyNums.text = BuildManager.Instance.senery.ToString();
        cnergyNums.text = BuildManager.Instance.cenergy.ToString();
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(EventType.UpdateAllUI, UpdateAllUI);
        EventManager.RemoveListener(EventType.ResetAllUI, ResetAllUI);
    }
}
