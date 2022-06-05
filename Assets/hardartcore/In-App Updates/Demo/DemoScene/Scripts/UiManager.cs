using UnityEngine;
using UnityEngine.UI;
using UpdateManager;

namespace DemoUpdateManager
{
    public class UiManager : MonoBehaviour
    {

#if UNITY_ANDROID
        public AndroidUpdateManager androidUpdateManager;
        public Text statusText;

        private void OnEnable()
        {
            // Subscribe for events from AndroidUpdateManager
            AndroidUpdateManager.OnUpdateAvailable += OnUpdateAvailable;
            AndroidUpdateManager.OnUpdateDownloaded += OnUpdateDownloaded;
        }

        private void OnDisable()
        {
            // Unsubscribe for events from AndroidUpdateManager
            AndroidUpdateManager.OnUpdateAvailable -= OnUpdateAvailable;
            AndroidUpdateManager.OnUpdateDownloaded -= OnUpdateDownloaded;
        }

        private void OnUpdateAvailable()
        {
            // Start the process of downloading the update
            androidUpdateManager.StartUpdate();
        }

        private void OnUpdateDownloaded()
        {
            // Update downloaded, install it : )
            statusText.text = "Status: Update downloaded";
            androidUpdateManager.CompleteUpdate();
        }
#endif

    }
}
