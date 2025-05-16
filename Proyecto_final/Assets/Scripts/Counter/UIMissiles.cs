using UnityEngine;
using TMPro;

public class UIMissiles : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoMisiles;

    void Update()
    {
        int misilesActuales = GameManager.Instance.GetMissiles();
        misilesActuales = Mathf.Max(misilesActuales, 0); // Evita números negativos
        textoMisiles.text = misilesActuales.ToString(); 
    }
}