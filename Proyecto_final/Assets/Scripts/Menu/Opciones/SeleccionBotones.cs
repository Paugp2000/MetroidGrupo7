using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SeleccionBotones : MonoBehaviour
{

    public GameObject defaultButton;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(defaultButton);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }
    }


}
