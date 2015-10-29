using UnityEngine;
using System.Collections;

/// <summary>
/// Camera flash.
/// </summary>
public class CameraFlash : MonoBehaviour
{
		private Color flashColor;//flash color
		public static bool isRunning = false;//is flash running
		public AudioClip cameraCaptuerClip;//camera capture sfx ( sound effect )
		private SpriteRenderer spriteRendererComp;//camera flash sprite render
		
		void Awake ()
		{
				spriteRendererComp = GetComponent<SpriteRenderer> ();
		        flashColor = new Color (1, 1, 1, 0);//set flash color to white with 0 alpha
				spriteRendererComp.color = flashColor;//set current flash color
		}

	    //start the flash
		public void Flash ()
		{
				if (isRunning)
						return;
				isRunning = true;
				AudioSource asource = GetComponent<AudioSource> ();
				asource.clip = cameraCaptuerClip;
				AudioManager.PlayClip (asource);//play flash sound effect
				StartCoroutine ("ShowFlash");//start flashing
		}

		IEnumerator ShowFlash ()
		{
				flashColor = new Color (1, 1, 1, 1);//set flash color to white with 1 alpha
				float targetAlhpa = 0;//target alpha
				float alphaFraction = 0.04f;//alpha decrease fraction
				float delayTime = 0.01f;//delay time

				while (flashColor.a > targetAlhpa) {//while flash color alpha greater than target alpha
						flashColor.a -= alphaFraction;//decrease flash color alpha by alpha fraction
						spriteRendererComp.color = flashColor;
						yield return new WaitForSeconds (delayTime);
				}

				flashColor.a = 0;//set flash color alpha to 0
				spriteRendererComp.color = flashColor;//apply flash color on the flash sprite renderer
				isRunning = false;
				yield return 0;
		}
}
