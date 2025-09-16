using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using Oculus.Interaction;

public class ButtonInteractSS : MonoBehaviour
{
    [SerializeField, Interface(typeof(IInteractableView))]
    private UnityEngine.Object _interactableView;
    private IInteractableView InteractableView;

    private void Awake()
    {
        Debug.LogWarning("Awake");
        InteractableView = _interactableView as IInteractableView;
    }

    private void OnEnable()
    {
        Debug.LogWarning("OnEnable");
        if (InteractableView != null)
        {
            InteractableView.WhenStateChanged += OnStateChanged;
        }
    }

    private void OnDisable()
    {
        Debug.LogWarning("OnDisable");
        if (InteractableView != null)
        {
            InteractableView.WhenStateChanged -= OnStateChanged;
        }
    }

    private void OnStateChanged(InteractableStateChangeArgs args)
    {
        Debug.LogWarning("OnStateChanged");
        if (args.NewState == InteractableState.Select) // triggered on poke/select
        {
            Debug.Log("Interactable selected → taking screenshot...");
            StartCoroutine(CaptureAndUploadScreenshot());
        }
    }

    private IEnumerator CaptureAndUploadScreenshot()
    {
        Debug.LogWarning("CaptureAndUploadScreenshot");
        // Wait until end of frame so the frame is fully rendered
        yield return new WaitForEndOfFrame();

        // Capture current screen
        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();

        byte[] jpgBytes = screenshot.EncodeToJPG();
        Destroy(screenshot); // free memory

        // Create form for upload
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", jpgBytes, "screenshot.jpg", "image/jpeg");

        // Change to your laptop's IP address on the same network as the Quest
        string url = "http://192.168.1.155:5000/upload";

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Upload failed: " + www.error);
            }
            else
            {
                Debug.Log("Screenshot uploaded successfully!");
            }
        }
    }
}
