

  _____ _   _    _    _   _ _  ______    _____ ___  ____     
 |_   _| | | |  / \  | \ | | |/ / ___|  |  ___/ _ \|  _ \    
   | | | |_| | / _ \ |  \| | ' /\___ \  | |_ | | | | |_) |   
   | | |  _  |/ ___ \| |\  | . \ ___) | |  _|| |_| |  _ <    
  _|_| |_| |_/_/__ \_\_|_\_|_|\_\____/  |_|_ _\___/|_| \_\__ 
 |  _ \| | | |  _ \ / ___| | | |  / \  / ___|_ _| \ | |/ ___|
 | |_) | | | | |_) | |   | |_| | / _ \ \___ \| ||  \| | |  _ 
 |  __/| |_| |  _ <| |___|  _  |/ ___ \ ___) | || |\  | |_| |
 |_|___ \___/|_|_\_\\____|_| |_/_/___\_\____/___|_|_\_|\____|
 |_   _| | | |_ _/ ___|     / \  / ___/ ___|| ____|_   _| | |
   | | | |_| || |\___ \    / _ \ \___ \___ \|  _|   | |   | |
   | | |  _  || | ___) |  / ___ \ ___) |__) | |___  | |   |_|
   |_| |_| |_|___|____/  /_/   \_\____/____/|_____| |_|   (_)



# INTRODUCTION

This asset provides you a way to integrate Google's solution for in-app updates in Android.
You can learn more about it here: https://developer.android.com/guide/app-bundle/in-app-updates




# HOW TO USE

1. Import the package (which you already did if you are able to read this :P )
2. Add UpdateManager prefab from Assets/hardartcore/Prefabs folder to your Scene.
3. That's all.




# HOW TO TEST

To be able to test it your app / game should be available in Google Play Store.
The easiest way to test it to decrease your version number in Player Settings and build the game using the same keystore which 
you used to sign it for Play Store and use the same package name!
If everything is correct you should see a dialog which informs you that there is an update.




# CUSTOMIZATION

You can customize the behaviour of AndroidUpdateManager in a few steps.
It has two events which are fired when the system detects that there is a new version of the game and it's available for an update
and when the update is downloaded.

The first one: AndroidUpdateManager.OnUpdateAvailable will let you customize the way which you tell your users that there is an update.
You can show your custom dialog or anything else, but don't forget to call AndroidUpdateManager.Instance.StartUpdate() to start the downloading
process of your app's new version.

The second event: AndroidUpdateManager.OnUpdateDownloaded will give you a way to notify the user that the update is downloaded and 
he can start the update process. You can show a dialog or any other Ui elements to inform him and let him choose if it should update now or later.
Do not forget to call AndroidUpdateManager.Instance.CompleteUpdate()l to install the downloaded update.


You can check Assets/hardartcore/Demo/DemoScene/Scripts/UiManager.cs to check the example how to use it.




# FAQ / KNOWN PROBLEMS

It is possible at first that the Play Core library won't show that there is an update even if you setup everything correctly.
This is happening because the detection depends on Play Store app's cache. Clearing the cache from Android's Settings can fix this issue,
but this is not 100% fix. Sometimes it can take a time so the library starts sending an event that there is an available update.

If the user chooses not to update the app it may happen that the next time he opens the app Play Core library won't send an event that there is 
an update. There is no way for now to force the check again. They decide when to do it.

This asset uses Google Play Core library version 1.6.4 . If you have this library already imported you can delete it from :
Assets/hardartcore/Plugins/Android/core-1.6.4.aar so it won't conflict or fail your builds.

If you have any other questions or you encountered any problems isung this asset, please don't hesitate to contact me at: support@hardartcore.com


THANKS AGAIN AND HAPPY CODING AND UPDATING!


  _                   _            _                     
 | |__   __ _ _ __ __| | __ _ _ __| |_ ___ ___  _ __ ___ 
 | '_ \ / _` | '__/ _` |/ _` | '__| __/ __/ _ \| '__/ _ \
 | | | | (_| | | | (_| | (_| | |  | || (_| (_) | | |  __/
 |_| |_|\__,_|_|  \__,_|\__,_|_|   \__\___\___/|_|  \___|




