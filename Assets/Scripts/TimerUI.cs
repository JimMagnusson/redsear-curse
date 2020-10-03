using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textGUI;
    [SerializeField] GameObject rewindIcon;

    private void Start()
    {
        textGUI = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateTimerText(int roundedTime)
    {
        textGUI.SetText("" + (roundedTime / (60 * 10)) % 6 + (roundedTime/60) % 10 + ":" + (roundedTime/10) % 6 + roundedTime % 10);
    }

    public void toggleRewindIcon(bool isActive)
    {
        rewindIcon.SetActive(isActive);
    }

}
