using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalPoint : MonoBehaviour
{

    //============STOP TIME AND CHANGE SCENE===========\\
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.StopTimeOnGame();
        SceneManager.LoadScene("SceneWin");
    }
}
