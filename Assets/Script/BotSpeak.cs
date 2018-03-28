using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class BotSpeak : MonoBehaviour
{
    [SerializeField]
    private GameObject text;

    private Queue<string> sentence;

    public void Say(string sent)
    {
        Debug.Log("Say: " + sent);
        GetComponent<MeshRenderer>().enabled = true;
        text.GetComponent<TextMesh>().text = sent;
    }

    public void Show(string sent)
    {
        text.GetComponent<TextMesh>().text = sent;
    }

    private string ProcessString(string input)
    {
        int count = 0;
        string output = string.Empty;
        foreach (char c in input)
        {
            if (count > 18)
            {
                count = 0;
                output += '\n';
            }
            if (c == '\n')
            {
                count = 0;
                output += c;
                continue;
            }
            output += c;
            count++;
        }
        return output;
    }
}
