using UnityEngine;
using System.Collections;

/// <summary>
/// Draw resizer.
/// </summary>
public class DrawResizer : MonoBehaviour
{
		private Animator animator;//draw resizer animator
		public Animator toolsAnimator;//tools animator

		// Use this for initialization
		void Start ()
		{
				animator = GetComponent<Animator> ();
				if (toolsAnimator == null) {
						toolsAnimator = GameObject.Find ("Tools").GetComponent<Animator> ();
				}
		}

		//fade in tools animation
		private void FadeInToolsAnimation ()
		{
				toolsAnimator.SetTrigger ("isFadingIn");
		}

		//when tools animation fadingout finish
		private void OnFadingOutFinish ()
		{
				OnScreenDrawing.isRunning = true;
				toolsAnimator.SetBool ("isFadingIn", false);
				animator.SetBool ("isFadingIn", false);
				animator.SetBool ("isFadingOut", false);
		}
}