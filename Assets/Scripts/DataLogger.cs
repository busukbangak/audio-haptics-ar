using UnityEngine;

public class DataLogger : MonoBehaviour
{
    public string Key;
    public string Value;

    public void LogData()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.Log(Key, Value);
            Debug.Log($"Logged: {Key} = {Value}");
        }
        else
        {
            Debug.LogError("DataManager instance not found!");
        }
    }

    public void SetKey(string key) => Key = key;
    
    public void SetValue(string value) => Value = value;
}