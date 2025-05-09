using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelecctionMod : MonoBehaviour
{
    public RectTransform imageTransform; // Asigna tu imagen aquí desde el Inspector
    public Vector2 movedPosition = new Vector2(0, -79); // Posición a la que se moverá hacia abajo
    private Vector2 originalPosition;
    private bool isMoved = false; // Para saber si la imagen ya está movida

    void Start()
    {
        if (imageTransform != null)
        {
            originalPosition = imageTransform.anchoredPosition; // Guardamos la posición original
        }
    }

    void Update()
    {
        // Mover la imagen hacia abajo, pero limitar que no baje de -79
        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector2 targetPosition = movedPosition;

            // Limitar que no sea menor que -79 en Y
            if (targetPosition.y < -79)
            {
                targetPosition.y = -79;
            }

            // Mueve la imagen hacia abajo, pero no más allá de -79
            imageTransform.anchoredPosition = targetPosition;
            isMoved = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            // Si ya se movió, la imagen vuelve a la posición original
            imageTransform.anchoredPosition = originalPosition;
            isMoved = false;
        }

        // Cambiar de escena dependiendo de la posición Y de la imagen
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Si la posición Y de la imagen es 0, carga una escena
            if (imageTransform.anchoredPosition.y == 0)
            {
                SceneManager.LoadScene("Inicio"); // Reemplaza con el nombre real de la escena
            }
            // Si la posición Y de la imagen es -79, carga otra escena
            else if (imageTransform.anchoredPosition.y == -79)
            {
                SceneManager.LoadScene("MenuMetroid"); // Reemplaza con el nombre real de la escena
            }
        }
    }
}