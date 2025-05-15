using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetTimeText : MonoBehaviour
{

    [SerializeField] private TMP_Text playerTimeText;

    double finalTime;

    void Start()
    {
        finalTime = AnaliticsManager.Instance.GetFinalTime();
        int hours = (int)(finalTime / 3600);
        int minutes = (int)((finalTime % 3600) / 60);
        int seconds = (int)(finalTime % 60);
        int milliseconds = (int)((finalTime * 10) % 10);
        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", hours, minutes, seconds, milliseconds);

        playerTimeText.text = formattedTime.ToString();
    }

}
