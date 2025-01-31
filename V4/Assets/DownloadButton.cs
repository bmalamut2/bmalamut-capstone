using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using TMPro;

public class DownloadButton : MonoBehaviour
{
    public TMP_InputField ApiKey;
    public TMP_InputField TaskID;
    public TMP_Text log;
    public Request request;
    public Load load;

    public async void OnButtonClick()
    {
        string apikey = ApiKey.text;
        string[] tasks = File.ReadAllLines(TaskID.text + "\\task_id.txt");
        foreach (string task in tasks)
        {
            request.GetModel(apikey, task);
            await WaitForDownload();
            await load.LoadModel();
            log.text = "Download not started";
        }
    }
    private async Task WaitForDownload()
    {
        while (log.text != "Download finished")
        {
            await Task.Yield();
        }
    }
}
