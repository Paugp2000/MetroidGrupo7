using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveNamePlayer : MonoBehaviour
{
    string playerName;

    public void SaveName (string name)
    {
        playerName = name;
        Debug.Log("Name : " + playerName);
    }
}
