using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCache : MonoBehaviour
{
    Dictionary<string, string> data;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        data = new Dictionary<string, string>();
    }

    public void Add(string key, string value)
    {
        data[key] = value;
    }

    public string Find(string key)
    {
        if (data.ContainsKey(key))
        {
            return data[key];
        }
        else
        {
            return string.Empty;
        }
    }
}
