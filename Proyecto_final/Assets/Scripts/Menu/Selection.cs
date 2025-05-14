using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ImageMoverAndSceneLoader : MonoBehaviour
{
    public RectTransform imageTransform; // Asigna tu imagen aquí desde el Inspector
    private Vector2 originalPosition;
    private Vector2 midPosition = new Vector2(0, -46);
    private Vector2 finalPosition = new Vector2(0, -94);
    private int moveState = 0; // 0 = original, 1 = -79, 2 = -94
    public InputActionAsset inputActionsMapping;
    InputAction up, dawn, enter;

    private void Awake()
    {
        inputActionsMapping.Enable();
        up = inputActionsMapping.FindActionMap("Selec").FindAction("up");
        dawn = inputActionsMapping.FindActionMap("Selec").FindAction("dawn");
        enter = inputActionsMapping.FindActionMap("Selec").FindAction("enter");
    }

    void Start()
    {
        if (imageTransform != null)
        {
            originalPosition = imageTransform.anchoredPosition;
            midPosition.x = originalPosition.x;   // Asegurar que mantenga la misma X
            finalPosition.x = originalPosition.x;
        }
    }

    void Update()
    {
        // Mover hacia abajo con S
        if (dawn.triggered)
        {
            if (moveState == 0)
            {
                imageTransform.anchoredPosition = midPosition;
                moveState = 1;
            }
            else if (moveState == 1)
            {
                imageTransform.anchoredPosition = finalPosition;
                moveState = 2;
            }
        }

        // Mover hacia arriba con W
        if (up.triggered)
        {
            if (moveState == 2)
            {
                imageTransform.anchoredPosition = midPosition;
                moveState = 1;
            }
            else if (moveState == 1)
            {
                imageTransform.anchoredPosition = originalPosition;
                moveState = 0;
            }
        }

        // Cambiar de escena según posición actual
        if (enter.triggered)
        {
            float posY = imageTransform.anchoredPosition.y;

            if (Mathf.Approximately(posY, originalPosition.y))
            {
                SceneManager.LoadScene("MainLevel"); // Reemplaza con la escena para Y = original
            }
            else if (Mathf.Approximately(posY, midPosition.y))
            {
                SceneManager.LoadScene("MenuMetroid"); // Reemplaza con la escena para Y = -79
            }
            else if (Mathf.Approximately(posY, finalPosition.y))
            {
                SceneManager.LoadScene("Opciones"); // Reemplaza con la escena para Y = -94
            }
        }
    }
}
