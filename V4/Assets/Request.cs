using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Request : MonoBehaviour
{
    public TMP_Text log;
    public Load load;

    string apiKey;
    string taskId;
    string dlink;

    public void GetModel(string a, string t)
    {
        apiKey = a;
        taskId = t;
        StartCoroutine(GetRequest());
    }

    IEnumerator GetRequest()
    {
        string url = "https://api.tripo3d.ai/v2/openapi/task/" + taskId;
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                GetRequestResponse response = JsonUtility.FromJson<GetRequestResponse>(request.downloadHandler.text);
                dlink = response.data.result.pbr_model.url;
                log.text = "Downloading...";
                StartCoroutine(DownloadModel());
                yield break;
            }
            else
            {
                log.text = "Error: Failed sending API requrest.";
                yield break;
            }
        }
    }

    IEnumerator DownloadModel()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(dlink))
        {
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                load.ReceiveData(request.downloadHandler.data);
            }
            else
            {
                log.text = "Error: Failed downloading model";
                yield break;
            }
        }
        log.text = "Download finished";
    }

    [System.Serializable]
    public class GetRequestResponse
    {
        public int code;
        public GetData data;
    }

    [System.Serializable]
    public class GetData
    {
        public string task_id;
        public string type;
        public string status;
        public Input input;
        public Output output;
        public int progress;
        public int create_time;
        public string prompt;
        public string thumbnail;
        public int queuing_num;
        public int running_left_time;
        public Result result;
    }

    [System.Serializable]
    public class Result
    {
        public Model pbr_model;
        public RenderedImage rendered_image;
    }

    [System.Serializable]
    public class Model
    {
        public string type;
        public string url;
    }

    [System.Serializable]
    public class RenderedImage
    {
        public string type;
        public string url;
    }

    [System.Serializable]
    public class Input
    {
        public string prompt;
    }

    [System.Serializable]
    public class Output
    {
        public string model;
        public string pbr_model;
        public string rendered_image;
    }

    [System.Serializable]
    public class RequestResponse
    {
        public int code;
        public Data data;
    }

    [System.Serializable]
    public class Data
    {
        public string task_id;
    }
}
