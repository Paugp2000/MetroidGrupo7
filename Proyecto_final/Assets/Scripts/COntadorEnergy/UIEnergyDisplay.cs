using TMPro;
using UnityEngine;

public class UIEnergyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoEnergia;

    void Update()
    {
        int energiaActual = GameManager.Instance.GetEnergy();
        energiaActual = Mathf.Max(energiaActual, 0); // Asegura que no baje de 0
        textoEnergia.text = energiaActual.ToString();
    }
}