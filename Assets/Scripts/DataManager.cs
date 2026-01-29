using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public Dictionary<string, string> Data = new Dictionary<string, string>();

    public string ExportFileName = "studydata.csv";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Log(string key, string value)
    {
        Data[key] = value;
    }


    public void ExportData()
    {
        if (Data == null || Data.Count == 0)
        {
            Debug.LogWarning("No data to save.");
            return;
        }

        string path = Path.Combine(Application.persistentDataPath, ExportFileName);
        bool fileExists = File.Exists(path);

        // Header = Keys
        string header = string.Join(",", Data.Keys);

        // Values
        List<string> values = new List<string>(Data.Values);
        string line = string.Join(",", values);

        // If file doesn't exist, create it with header
        if (!fileExists)
        {
            string csv = header + "\n" + line;
            File.WriteAllText(path, csv, Encoding.UTF8);
        }
        else
        {
            // File exists, append only the data line
            File.AppendAllText(path, "\n" + line, Encoding.UTF8);
        }

        Debug.Log("Data saved to: " + path);
    }
}
