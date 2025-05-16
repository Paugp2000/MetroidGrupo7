using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.StopTimeOnGame();
        //AnaliticsManager.Instance.SaveAnalitics();
        SceneManager.LoadScene("SceneWin");
    }
}
