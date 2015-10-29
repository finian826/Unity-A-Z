#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

/// <summary>
/// Screenshot exporter to multi platforms such that android,ios and others
/// </summary>
public class ScreenshotExporter : MonoBehaviour
{
		public static event Action ScreenshotFinishedSaving;
		public static event Action ImageFinishedSaving;
	
	#if UNITY_IPHONE  || UNITY_ANDROID
	[DllImport("__Internal")]
    private static extern bool saveToGallery( string path );
	#endif
	
		public static IEnumerator Save (string screenshotFilename, string albumName, bool callback)
		{
				bool photoSaved = false;

				//Debug.Log("Save screenshot " + screenshotFilename); 
		
				#if UNITY_IPHONE
		
			if(Application.platform == RuntimePlatform.IPhonePlayer) 
			{
				Debug.Log("iOS platform detected");
				
				string iosPath = Application.persistentDataPath + "/" + screenshotFilename;
		
			while(CameraFlash.doingFlash){
				yield return new WaitForSeconds(0);
			}
				Application.CaptureScreenshot(screenshotFilename);
				while(!photoSaved) 
				{
					photoSaved = saveToGallery( iosPath );
					yield return new WaitForSeconds(0);
				}
				iPhone.SetNoBackupFlag( iosPath );
			
			} else {

			while(CameraFlash.doingFlash){
				yield return new WaitForSeconds(0);
			}
			    Debug.Log("photo saved int :"+iosPath);
			 	Application.CaptureScreenshot(screenshotFilename);
			}
			
				#elif UNITY_ANDROID	
				
			if(Application.platform == RuntimePlatform.Android) 
			{
		
			while(CameraFlash.isRunning){//wait until flash finish
				yield return new WaitForSeconds(0);
			}

				string androidPath = "/../../../../DCIM/" + albumName + "/" + screenshotFilename;
				string path = Application.persistentDataPath + androidPath;
				string pathonly = Path.GetDirectoryName(path);//returns directory path
			    if(!Directory.Exists(pathonly)){
				  Directory.CreateDirectory(pathonly);
			   }

				Application.CaptureScreenshot(androidPath);
				AndroidJavaClass obj = new AndroidJavaClass("com.ryanwebb.androidscreenshot.MainActivity");
				
				while(!photoSaved) 
				{
					photoSaved = obj.CallStatic<bool>("scanMedia", path);
					yield return new WaitForSeconds(0);
				}
			     Debug.Log("photo saved int :"+path);
			} else {

			  while(CameraFlash.isRunning){//wait until flash finish
				 yield return new WaitForSeconds(0);
			}
				Application.CaptureScreenshot(screenshotFilename);
			Debug.Log(screenshotFilename+" saved");
			}
				#else
			
				while (!photoSaved) {
						yield return new WaitForSeconds (0);
						//Debug.Log("Screenshots only available in iOS/Android mode!");
						photoSaved = true;
				}
		
				#endif
		
				if (callback) {
						if (ScreenshotFinishedSaving != null)
								ScreenshotFinishedSaving ();
				}
		}
	
		public static IEnumerator SaveExisting (string filePath, bool callback = false)
		{
				bool photoSaved = false;
				//Debug.Log ("Save existing file to gallery " + filePath);

				#if UNITY_IPHONE
		
			if(Application.platform == RuntimePlatform.IPhonePlayer) 
			{
				//Debug.Log("iOS platform detected");
				
				while(!photoSaved) 
				{
					photoSaved = saveToGallery( filePath );
					yield return new WaitForSeconds(0);
				}
			
				iPhone.SetNoBackupFlag( filePath );
			}
			
				#elif UNITY_ANDROID	
				
			if(Application.platform == RuntimePlatform.Android) 
			{
				//Debug.Log("Android platform detected");
				AndroidJavaClass obj = new AndroidJavaClass("com.ryanwebb.androidscreenshot.MainActivity");
				while(!photoSaved) 
				{
					photoSaved = obj.CallStatic<bool>("scanMedia", filePath);
					yield return new WaitForSeconds(0);
				}
			
			}
		
				#else
			
				while (!photoSaved) {
						yield return new WaitForSeconds (0);
						//Debug.Log("Save existing file only available in iOS/Android mode!");
						photoSaved = true;
				}
		
				#endif
				if (callback) {
						if (ImageFinishedSaving != null)
								ImageFinishedSaving ();
				}
		}
}