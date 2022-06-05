using System;
using UnityEngine;

namespace UpdateManager
{
    enum UpdateType
    {
        // Flexible will let the user 
        // choose if he wants to update the app
        FLEXIBLE = 0,
        // Imeediate will trigger the update right away
        IMMEDIATE = 1
    }

    public class AndroidUpdateManager : MonoBehaviour
    {

#if UNITY_ANDROID

        #region Events
        // Used to get an event when the update is available
        public static event Action OnUpdateAvailable;

        // Used to get an event when the update is downloaded
        public static event Action OnUpdateDownloaded;
        #endregion

        public static AndroidUpdateManager Instance { get; private set; }

        [Header("Unity Main Activity")]
        [Tooltip("Change only if you are using another Launcher Activity")]
        public string unityMainActivity = "com.unity3d.player.UnityPlayer";

        [Header("Update Type")]
        [SerializeField]
        private UpdateType updateType = UpdateType.IMMEDIATE;

        private AndroidJavaObject _currentActivity;
        private AndroidJavaObject _inAppUpdateManager;

        class OnUpdateListener : AndroidJavaProxy
        {
            public OnUpdateListener() : base("com.hardartcore.update.OnUpdateListener") { }

            // Invoked when Google Play Services returns a response 
            public void onUpdate(bool isUpdateAvailable, bool isUpdateTypeAllowed)
            {
                if (isUpdateAvailable && isUpdateTypeAllowed)
                {
                    Debug.Log("There's an update");
                    if (OnUpdateAvailable != null)
                    {
                        OnUpdateAvailable.Invoke();
                    }
                }
            }
            // Invoked when the update is downloaded
            public void onUpdateDownloaded()
            {
                Debug.Log("Start update");
                if (OnUpdateDownloaded != null)
                {
                    OnUpdateDownloaded.Invoke();
                }
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(Instance.gameObject);
                Instance = this;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Start()
        {
            // Get UnityPlayer Activity
            GetCurrentAndroidActivity();

            // Initialize Android App Update Manager
            // By default UpdateType is set to FLEXIBLE
            _inAppUpdateManager = new AndroidJavaObject("com.hardartcore.update.AndroidUpdateManager", _currentActivity);
            if (updateType != UpdateType.FLEXIBLE)
            {
                _inAppUpdateManager.Call("setUpdateTypeImmediate");
            }
            // Add update listener
            _inAppUpdateManager.Call("addOnUpdateListener", new OnUpdateListener());
        }

        // You should call this function after OnUpdateListener.onUpdate(isUpdateAvailable, isUpdateTypeAllowed)
        // and only if both isUpdateAvailable and isUpdateTypeAllowed are true 
        public void StartUpdate()
        {
            _inAppUpdateManager.Call("startUpdate");
        }

        // You should call this function after OnUpdateListener.onUpdateDownloaded()
        // to start the instalation of the update
        public void CompleteUpdate()
        {
            _inAppUpdateManager.Call("completeUpdate");
        }

        private AndroidJavaObject GetCurrentAndroidActivity()
        {
            if (_currentActivity == null)
            {
                _currentActivity = new AndroidJavaClass(unityMainActivity).GetStatic<AndroidJavaObject>("currentActivity");
            }
            return _currentActivity;
        }

#endif

    }

}