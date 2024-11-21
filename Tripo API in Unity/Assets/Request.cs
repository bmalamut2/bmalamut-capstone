using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Request : MonoBehaviour
{
    public TMP_Text log;

    string apiKey;
    string prompt;
    string filePath;

    string taskId;
    string dlink;
    string complete;
    int time;
    bool error = false;

    public void ReceiveInput(string a, string p, string f)
    {
        apiKey = a;
        prompt = p;
        filePath = f;
        dlink = null;
        StartCoroutine(SendRequest());
    }

    IEnumerator SendRequest()
    {
        string url = "https://api.tripo3d.ai/v2/openapi/task";
        string jsonBody = "{\"type\": \"text_to_model\", \"prompt\": \"" + prompt + "\", \"model_version\": \"v2.0-20240919\", \"texture_quality\": \"detailed\"}";
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                RequestResponse response = JsonUtility.FromJson<RequestResponse>(request.downloadHandler.text);
                taskId = response.data.task_id;
            }
            else
            {
                log.text = "Error: failed sending API request";
                yield break;
            }
            while(dlink == null)
            {
                StartCoroutine(GetRequest());
                if(error)
                {
                    yield break;
                }
                yield return new WaitForSecondsRealtime(1);
            }
            log.text = "Download started...";
            StartCoroutine(DownloadModel());
        }
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
                complete = response.data.status;
                dlink = response.data.result.pbr_model.url;
                time = response.data.running_left_time;
                log.text = complete + " " + time;
            }
            else
            {
                log.text = "Error: failed sending API request";
                error = true;
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
                File.WriteAllBytes(filePath, request.downloadHandler.data);
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
