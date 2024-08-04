using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using NaughtyAttributes;
using System;

[System.Serializable]
public class DataAPIPayload
{
    public string collection;
    public string database;
    public string dataSource;
    public object document;
}

public static class StringUtils
{
    public static string Replace(this string input, Dictionary<string, string> replacements)
    {
        foreach (var placeholder in replacements)
        {
            input = input.Replace(placeholder.Key, placeholder.Value);
        }
        return input;
    }
}

public class MongoUtils : MonoBehaviour
{
    private string post_collection = "https://ap-southeast-1.aws.data.mongodb-api.com/app/data-jvhmszy/endpoint/data/v1/action/insertOne";
    private string get_collection = "https://ap-southeast-1.aws.data.mongodb-api.com/app/data-jvhmszy/endpoint/get_collection?id=[ID]&collName=[COLLECTION]";
    private string apiKey = "stqOkVG6TSOCwS0ARQ2lpnQhSWqZbBfQAwpcqaL2FYCh6urL7UdFrGY9LsV9uHQn";

    [Button]
    void post_test()
    {
        var user = new
        {
            _id = "jksadhb",
            age = 28
        };

        StartCoroutine(PostData("Referral", user));
    }

    [Button]
    void get_test()
    {
        StartCoroutine(GetData<ReferralCode>("Referral", "jksadhb", data => { Debug.Log(data.count); }));
    }

    IEnumerator GetData<T>(string collectionName, string primaryKey, Action<T> resultData = null)
    {
        var replaceDict = new Dictionary<string, string>()
        {
            { "[COLLECTION]", collectionName },
            { "[ID]", primaryKey }
        };

        string getUrl = get_collection.Replace(replaceDict);

        UnityWebRequest request = new UnityWebRequest(getUrl, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        //request.SetRequestHeader("api-key", apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            object data = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
            resultData?.Invoke((T)data);
        }

        request.Dispose();
    }

    IEnumerator PostData(string collectionName, object document)
    {
        // Construct the payload
        DataAPIPayload payload = new DataAPIPayload
        {
            collection = collectionName,
            database = "Database",
            dataSource = "RedGameCluster",
            document = document
        };

        string jsonData = JsonConvert.SerializeObject(payload);

        UnityWebRequest request = new UnityWebRequest(post_collection, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("api-key", apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }

        request.Dispose();
    }
}
