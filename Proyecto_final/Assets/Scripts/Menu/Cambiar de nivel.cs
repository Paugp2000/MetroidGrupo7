using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class Cambiardenivel : MonoBehaviour
{
    Coroutine corutinaCambioEscena;
      public void CambiarEscena(string nomEscena)
    {
        if (corutinaCambioEscena == null)
        {
            corutinaCambioEscena = StartCoroutine(_CambiarEscenaConDelay(nomEscena,1f));
        }
    }
    public void ExitGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }
    IEnumerator _CambiarEscenaConDelay(string nomEscena, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nomEscena);

    }

}
