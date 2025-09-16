using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GetWebResponse : MonoBehaviour
{
    // URL of your local webpage
    public string url = "http://172.20.10.9:8000/latest/";
    public string responseText;

    void Start()
    {
        StartCoroutine(GetResponse());
    }

    public IEnumerator GetResponse()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                // This is the response from your local web server
                responseText = request.downloadHandler.text;
                Debug.Log("Response: " + responseText);
               
                // You can now use responseText in Unity
            }
        }
    }
}
