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
        EventManager.AddListener(EventType.UpdateAllUI, UpdateAllUI);
    }

    private void UpdateAllUI()
    {
        maskNums.text = WorldManager.Instance.limit.ToString();
        senergyNums.text = BuildManager.Instance.privateSenergy.ToString();
        cnergyNums.text = BuildManager.Instance.privateCenergy.ToString();
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(EventType.UpdateAllUI, UpdateAllUI);
    }
}
