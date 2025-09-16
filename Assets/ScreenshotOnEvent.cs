using UnityEngine;
using Oculus.Interaction; // gives you IInteractableView, InteractableStateChangeArgs
using UnityEngine.Networking;
using System.Collections;

public class ScreenshotOnEvent : MonoBehaviour
{
    [SerializeField, Interface(typeof(IInteractableView))]
    private MonoBehaviour _interactableView;  // drag your interactable here
    private IInteractableView _view;
    

    public string serverURL = "http://192.168.1.155:5000/upload";
    public int jpgQuality = 50;

    void Awake()
    {
        Debug.LogWarning("Awake");
        _view = _interactableView as IInteractableView;
    }

    void OnEnable()
    {
        Debug.LogWarning("OnEnable");
        if (_view != null)
            _view.WhenStateChanged += OnStateChanged;
    }

    void OnDisable()
    {
        Debug.LogWarning("OnDisable");
        if (_view != null)
            _view.WhenStateChanged -= OnStateChanged;
    }

    private void OnStateChanged(InteractableStateChangeArgs args)
    {
        Debug.LogWarning("OnStateChanged");
        // Only screenshot when state becomes "Select" (user pokes/presses)
        if (args.NewState == InteractableState.Select)
        {
            StartCoroutine(CaptureAndSendRoutine());
        }
    }

    IEnumerator CaptureAndSendRoutine()
    {
        Debug.LogWarning("CaptureAndSendRoutine");
        yield return new WaitForEndOfFrame();

        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();

        byte[] jpgBytes = tex.EncodeToJPG(jpgQuality);
        Destroy(tex);

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", jpgBytes, "frame.jpg", "image/jpeg");

        using (UnityWebRequest www = UnityWebRequest.Post(serverURL, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log("Upload error: " + www.error);
        }
    }
}
