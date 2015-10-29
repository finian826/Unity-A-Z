using UnityEngine;
using System.Collections;

/// <summary>
/// Screen shot.
/// </summary>
public class ScreenShot : MonoBehaviour
{
		private bool takeShot = false;
		private string albumName = "DrawingDemo";//name of album

		//create screenshot name from current date time
		public static string ScreenShotName ()
		{
				return string.Format ("{0}.png", 
		                      System.DateTime.Now.ToString ("yyyy-MM-dd_HH-mm-ss"));
		}
		
		//Take Screenshot
		public void TakeShot ()
		{
				if (takeShot)
						return;

				takeShot = true;
				StartCoroutine ("TakingScreenShot");
		}
		
		IEnumerator TakingScreenShot ()
		{
				StartCoroutine (ScreenshotExporter.Save (ScreenShotName (), albumName, true));//export screenshot
				takeShot = false;
				yield return 0;
		}
}
