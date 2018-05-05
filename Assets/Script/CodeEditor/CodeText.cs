using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;

public class CodeText : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Text textOut;

    private ulong fenceValue;
    private uint errorTime;

    public void CodeSync()
    {
        textOut.text = inputField.text;
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.K))
        {
            Format();
        }
    }

    public async void Format()
    {
        ulong curFence = fenceValue;

        using (HttpClient http = new HttpClient())
        {
            using (StringContent stringContect = new StringContent(""))
            {
                var response = await http.PostAsync("", stringContect);
                if (curFence < fenceValue) return;
                if (response.StatusCode != System.Net.HttpStatusCode.OK) errorTime++;
                errorTime = 0;
                string formatted = await response.Content.ReadAsStringAsync();
                fenceValue++;
                textOut.text = formatted;
            }
        }
    }

    struct FormatRpc
    {
        string uri;
        string languageId;
        string version;
        string text;
    }
}
