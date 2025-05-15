using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ImageMoverAndSceneLoader : MonoBehaviour
{
    public RectTransform imageTransform;
    private Vector2 originalPosition;
    private Vector2 firstPosition = new Vector2(0, -46);
    private Vector2 secondPosition = new Vector2(0, -94);
    private Vector2 thirdPosition = new Vector2(0, -142);  // Cerrar juego
    private Vector2 fourthPosition = new Vector2(0, -193); // Nueva cuarta posición
    private Vector2 fivePosition = new Vector2(0, -242); // Nueva cuarta posición
    private Vector2 sixPosition = new Vector2(0, -289); // Nueva cuarta posición
    private int moveState = 0; // 0 = original, 1 = first, 2 = second, 3 = third, 4 = fourth

    public InputActionAsset inputActionsMapping;
    InputAction up, dawn, enter;

    [SerializeField] private AudioClip selectorSound;  // Aquí agregamos una variable para el sonido
    private AudioSource audioSource;  // Aquí almacenamos el AudioSource

    private void Awake()
    {
        inputActionsMapping.Enable();
        up = inputActionsMapping.FindActionMap("Selec").FindAction("up");
        dawn = inputActionsMapping.FindActionMap("Selec").FindAction("dawn");
        enter = inputActionsMapping.FindActionMap("Selec").FindAction("enter");
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (imageTransform != null)
        {
            originalPosition = imageTransform.anchoredPosition;
            firstPosition.x = originalPosition.x;
            secondPosition.x = originalPosition.x;
            thirdPosition.x = originalPosition.x;
            fourthPosition.x = originalPosition.x;
            fivePosition.x = originalPosition.x;
            sixPosition.x = originalPosition.x;
        }
    }

    void Update()
    {
        if (dawn.triggered)
        {
            if (audioSource != null && selectorSound != null)
            {
                audioSource.PlayOneShot(selectorSound);
            }

            if (moveState < 6)
            {
                moveState++;
                UpdatePosition();
            }
        }

        if (up.triggered)
        {
            if (audioSource != null && selectorSound != null)
            {
                audioSource.PlayOneShot(selectorSound);
            }

            if (moveState > 0)
            {
                moveState--;
                UpdatePosition();
            }
        }

        if (enter.triggered)
        {
            switch (moveState)
            {
                case 0:
                    SceneManager.LoadScene("MainLevel");
                    break;
                case 1:
                    SceneManager.LoadScene("MenuMetroid");
                    break;
                case 2:
                    SceneManager.LoadScene("Opciones");
                    break;
                case 3:
                    SceneManager.LoadScene("LeaderBoard");
                    break;
                case 4:
                    SceneManager.LoadScene("PlayerStats");
                    break;
                case 5:
                    SceneManager.LoadScene("GameAnalitics");
                    break;
                case 6:
                    Application.Quit();
                    UnityEditor.EditorApplication.isPlaying = false;
                    break;
            }
        }
    }

    void UpdatePosition()
    {
        switch (moveState)
        {
            case 0:
                imageTransform.anchoredPosition = originalPosition;
                break;
            case 1:
                imageTransform.anchoredPosition = firstPosition;
                break;
            case 2:
                imageTransform.anchoredPosition = secondPosition;
                break;
            case 3:
                imageTransform.anchoredPosition = thirdPosition;
                break;
            case 4:
                imageTransform.anchoredPosition = fourthPosition;
                break;
            case 5:
                imageTransform.anchoredPosition = fivePosition;
                break;
            case 6:
                imageTransform.anchoredPosition = sixPosition;
                break;
        }
    }
}
