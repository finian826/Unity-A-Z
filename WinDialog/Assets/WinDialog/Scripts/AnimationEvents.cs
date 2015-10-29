using UnityEngine;
using System.Collections;

/// <summary>
/// Animation events.
/// </summary>
public class AnimationEvents : MonoBehaviour
{
		private static WinStarsManager winStarManagerComp;
		private static int starsSFXIndex = 0;//refernce to star sfx index in the array

		// Use this for initialization
		void Start ()
		{
				if (winStarManagerComp == null) {
						winStarManagerComp = GameObject.Find ("WinDialog").GetComponent<WinStarsManager> ();
				}
				starsSFXIndex = 0;
		}

		private void WinDialogEvent ()
		{
				winStarManagerComp.PrepareStars (Menu.starsNumber);//prepare stars animations and effects
		}

		private void CameraShakingStart ()
		{
				Camera.main.GetComponent<Animator> ().SetBool ("isRunning", true);//play camera shaking animation
				SFX.instance.audioSources [0].clip = SFX.instance.starsSFX [starsSFXIndex];//get current star sfx
				SFX.instance.audioSources [0].Play ();//play current star sfx
				starsSFXIndex++;
				if (starsSFXIndex == SFX.instance.starsSFX.Length) {
						starsSFXIndex = 0;//reset index
				}
		}

		private void CameraShakingEnd ()
		{
				Camera.main.GetComponent<Animator> ().SetBool ("isRunning", false);//stop shaking animation
		}
}
