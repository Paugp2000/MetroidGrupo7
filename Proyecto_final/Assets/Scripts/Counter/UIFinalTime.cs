using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIFinalTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textFinalTime;
    double finalTime;
    private void Update()
    {

        finalTime = AnaliticsManager.Instance.GetFinalTime();

        int hours = (int)(finalTime / 3600);
        int minutes = (int)((finalTime % 3600) / 60);
        int seconds = (int)(finalTime % 60);
        int milliseconds = (int)((finalTime * 10) % 10);

        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", hours, minutes, seconds, milliseconds);
        textFinalTime.text = formattedTime;
    }
}
