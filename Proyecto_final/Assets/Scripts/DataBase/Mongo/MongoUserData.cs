using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MongoUserData : MonoBehaviour
{

    public static MongoUserData Instance;

    [SerializeField] private TMP_InputField playerNameInputField;


    public void SaveUserData()
    {
        Debug.Log(playerNameInputField.text);
        AnaliticsManager.Instance.setPlayerName(playerNameInputField.text);
        AnaliticsManager.Instance.SaveAnalitics();

        SceneManager.LoadScene("Inicio");

    }




}
