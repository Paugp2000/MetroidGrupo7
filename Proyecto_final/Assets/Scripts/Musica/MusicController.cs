using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    // Lista de nombres de escenas donde debe sonar la música
    [SerializeField] private List<string> allowedScenes;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Suscribirse al evento de cambio de escena
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Si la escena actual no está en la lista permitida, destruir este objeto
        if (!allowedScenes.Contains(scene.name))
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Asegurarse de desuscribirse del evento para evitar errores
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
