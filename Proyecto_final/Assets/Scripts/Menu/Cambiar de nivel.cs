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
            corutinaCambioEscena = StartCoroutine(_CambiarEscenaConDelay(nomEscena));
        }
    }
    IEnumerator _CambiarEscenaConDelay(string nomEscena)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nomEscena);

    }

}
