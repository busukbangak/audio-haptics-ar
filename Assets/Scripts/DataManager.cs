using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public Dictionary<string, string> Data = new Dictionary<string, string>();

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


    public void ExportData(string filename = "study_data.csv")
    {
        if (Data == null || Data.Count == 0)
        {
            Debug.LogWarning("No data to save.");
            return;
        }

        // Header = Keys
        string header = string.Join(",", Data.Keys);

        // Values
        List<string> values = new List<string>(Data.Values);
        string line = string.Join(",", values);

        // CSV Text
        string csv = header + "\n" + line;

        // Save to file
        string path = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllText(path, csv, Encoding.UTF8);

        Debug.Log("Data saved to: " + path);
    }

    // TODO: Move this to corresponding View
    public void LogDataConsent()
    {
        Log("isDataConsentGiven:value", "YES");
        Log("isDataConsentGiven:timestamp", System.DateTime.Now.ToString());
        ExportData();
    }
}
