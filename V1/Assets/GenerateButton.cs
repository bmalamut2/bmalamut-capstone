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
    public TMP_InputField Path;
    public TMP_InputField FileName;
    public TMP_Text log;
    public Request request;
    public Load load;

    public async void OnButtonClick()
    {
        string apikey = ApiKey.text;
        string prompt = Prompt.text;
        string path = Path.text;
        string filename = FileName.text;
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
        if (path == "")
        {
            log.text = "Error: No path entered";
            return;
        }
        if (!Directory.Exists(path))
        {
            log.text = "Error: Could not open path";
            return;
        }
        if (filename == "")
        {
            if(prompt.Length > 200)
            {
                filename = "model_" + prompt.Substring(0, 200);
            }
            else
            {
                filename = "model_" + prompt;
            }
        }
        path = path + "\\" + filename + ".glb";
        request.ReceiveInput(apikey, prompt, path);
        await WaitForDownload();
        await load.LoadModel(path);
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
