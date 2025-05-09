using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject GrupoPausa;
    public bool Pausa = false;
    [SerializeField]
    public InputActionAsset inputActionsMapping;

    InputAction pausa;
    private void Awake()
    {
        inputActionsMapping.Enable();
        pausa = inputActionsMapping.FindActionMap("Pausar").FindAction("pausar");
    }

    // Update is called once per frame
    void Update()
    {
        if (pausa.triggered)
        {
            if (Pausa == false)
            {
                GrupoPausa.SetActive(true);
                Pausa = true;

                Time.timeScale = 0;
            }
            else if (Pausa == true)
            {
                Resumir(); 
            }
        }
    }

    public void Resumir()
    {
        GrupoPausa.SetActive(false);
        Pausa = false;

        Time.timeScale = 1;
    }

    public void Menu(string nomScene)
    {
        SceneManager.LoadScene(nomScene);
        Resumir();
    }
    public void Opciones(string nomScene)
    {
        SceneManager.LoadScene(nomScene);
        Resumir();
    }
}
