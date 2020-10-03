using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textGUI;

    private void Start()
    {
        textGUI = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateTimerText(int roundedTime)
    {
        //int roundedTime = (int)timeSinceLastLoop;
        textGUI.SetText("" + (roundedTime / (60 * 10)) % 6 + (roundedTime/60) % 10 + ":" + (roundedTime/10) % 6 + roundedTime % 10);
    }

}
