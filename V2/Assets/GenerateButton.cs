using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using TMPro;

public class Button : MonoBehaviour
{
    public TMP_InputField ApiKey;
    public TMP_InputField Prompt;
    public TMP_Text log;
    public Request request;
    public Load load;

    public async void OnButtonClick()
    {
        string apikey = ApiKey.text;
        string prompt = Prompt.text;
        if (apikey == "")
        {
            log.text = "Error: No API key entered";
            return;
        }
        if (prompt == "")
        {
            log.text = "Error: No prompt entered";
            return;
        }
        request.ReceiveInput(apikey, prompt);
        await WaitForDownload();
        await load.LoadModel();
        log.text = "Download not started";
    }

    private async Task WaitForDownload()
    {
        while (log.text != "Download finished")
        {
            await Task.Yield();
        }
    }
}
