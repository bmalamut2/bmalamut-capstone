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
    public TMP_InputField Polycount;
    public TMP_InputField Path;
    public TMP_InputField Name;
    public TMP_Text log;
    public TMP_Dropdown Version;
    public TMP_Dropdown Style;
    public Request request;
    public Load load;

    public async void OnButtonClick()
    {
        string apikey = ApiKey.text;
        string prompt = Prompt.text;
        string poly = Polycount.text;
        string path = Path.text;
        string name = Name.text;
        string version = Version.options[Version.value].text;
        string style = Style.options[Style.value].text;
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
        if (path != "" && !Directory.Exists(path))
        {
            log.text = "Error: Could not open path";
            return;
        }
        request.ReceiveInput(apikey, prompt, version, poly, style, path, name + ".glb");
        await WaitForDownload();
        await load.LoadModel(path, name + ".glb");
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
