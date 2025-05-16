using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class TXTCreator : MonoBehaviour
{
    [SerializeField] private TMP_InputField saveName;
    private string nameSaved;
    private string rutaO = @"D:\MetroidProyecto\MetroidGrupo7\MetroidGrupo7\Proyecto_final\Metroid.txt";
    private void Awake()
    {
        crearArchivoTexto();
    }

    public void UpdateName()
    {
        Debug.Log("funciona");
        nameSaved = saveName.text;
        colocardatos();
    }

    void colocardatos()
    {
        double finalTime = AnaliticsManager.Instance.GetFinalTime();
        int hours = (int)(finalTime / 3600);
        int minutes = (int)((finalTime % 3600) / 60);
        int seconds = (int)(finalTime % 60);
        int milliseconds = (int)((finalTime * 10) % 10);
        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", hours, minutes, seconds, milliseconds);
        string userName = nameSaved;
        añadirTexto(userName, formattedTime);
    }

    void crearArchivoTexto()
    {
        if(!File.Exists(rutaO)) { 
        string ruta = @"D:\MetroidProyecto\MetroidGrupo7\MetroidGrupo7\Proyecto_final\Metroid.txt";
        File.Create(ruta);
        }
    }
    void añadirTexto(string userName, string formattedTime)
    {
        using (StreamWriter sw = new StreamWriter(rutaO, true)) 
        {
            sw.WriteLine(userName + " " + formattedTime);
        }
    }
}
