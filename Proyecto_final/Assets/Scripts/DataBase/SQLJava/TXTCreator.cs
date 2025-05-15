using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TXTCreator : MonoBehaviour
{
    string ruta;
    private void Awake()
    {
        crearArchivoTexto();
    }
    void Start()
    {
        double finalTime = AnaliticsManager.Instance.GetFinalTime();
        int hours = (int)(finalTime / 3600);
        int minutes = (int)((finalTime % 3600) / 60);
        int seconds = (int)(finalTime % 60);
        int milliseconds = (int)((finalTime * 10) % 10);
        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", hours, minutes, seconds, milliseconds);
        añadirTexto(formattedTime);
    }
   void crearArchivoTexto()
    {
        string nombreArchivo = "Metroid.txt";
        ruta = Path.Combine(Application.persistentDataPath, nombreArchivo);
        File.Create(ruta).Close();
    }
    void añadirTexto(string formattedTime)
    {
        using (StreamWriter sw = new StreamWriter(ruta, true)) 
        {
            sw.WriteLine(formattedTime);
        }
    }
}
