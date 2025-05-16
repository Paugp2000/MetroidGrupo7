using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveNamePlayer : MonoBehaviour
{
    public string playerName;

    public void SaveName (string name)
    {
        playerName = name;
        Debug.Log("Name : " + playerName);
    }
    public string getNamePlayer()
    {
        return playerName;
    }
}
