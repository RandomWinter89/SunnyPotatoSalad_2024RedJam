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

[System.Serializable]
public class UpdateAPIPayload
{
    public string collection;
    public string database;
    public string dataSource;
    public object filter;
    public object update;
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

    public static List<string> GetEnumNames<T>() where T : Enum
    {
        // Get the names of the enum values and convert them to a list of strings
        return new List<string>(Enum.GetNames(typeof(T)));
    }
}

public static class MongoUtils
{
    private const string post_insert_collection = "https://ap-southeast-1.aws.data.mongodb-api.com/app/data-jvhmszy/endpoint/data/v1/action/insertOne";
    private const string post_update_collection = "https://ap-southeast-1.aws.data.mongodb-api.com/app/data-jvhmszy/endpoint/data/v1/action/updateOne";
    private const string get_collection = "https://ap-southeast-1.aws.data.mongodb-api.com/app/data-jvhmszy/endpoint/get_collection?id=[ID]&collName=[COLLECTION]";

    private const string referral = "https://ap-southeast-1.aws.data.mongodb-api.com/app/data-jvhmszy/endpoint/referral?code=";

    private const string apiKey = "stqOkVG6TSOCwS0ARQ2lpnQhSWqZbBfQAwpcqaL2FYCh6urL7UdFrGY9LsV9uHQn";

    public static IEnumerator GetData<T>(string collectionName, string primaryKey, Action<T> resultData = null) where T : new()
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
            resultData?.Invoke((T)default);
        }
        else
        {
            string response = request.downloadHandler.text;
            Debug.Log("Response: " + response);

            if (string.IsNullOrEmpty(response))
            {
                resultData?.Invoke((T)default);
            }
            else
            {
                object data = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                resultData?.Invoke((T)data);
            }
        }

        request.Dispose();
    }

    public static IEnumerator InsertData(string collectionName, object document)
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

        UnityWebRequest request = new UnityWebRequest(post_insert_collection, "POST");
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

    public static IEnumerator UpdateData(string collectionName, string id, object document)
    {
        var filter = new
        {
            _id = id
        };

        UpdateAPIPayload payload = new UpdateAPIPayload
        {
            collection = collectionName,
            database = "Database",
            dataSource = "RedGameCluster",
            filter = filter,
            update = document
        };

        string jsonData = JsonConvert.SerializeObject(payload);

        Debug.Log(jsonData);

        UnityWebRequest request = new UnityWebRequest(post_update_collection, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("api-key", apiKey);
        request.SetRequestHeader("Content-Type", "application/json");

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

    public static IEnumerator PostReferral(string code, Action<string> callback)
    {
        string url = $"{referral}{code}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("api-key", apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
            callback?.Invoke(new MongoResult()
            {
                success = false,
                error = request.error,
                message = "Error Occurred"
            });
        }
        else
        {
            var result = JsonConvert.DeserializeObject<MongoResult>(request.downloadHandler.text);

            Debug.Log("Uploaded Referral");
            callback?.Invoke(result);
        }

        request.Dispose();
    }
}
