using UnityEngine;
using System.Collections;

/// <summary>
/// Tools Component.
/// </summary>
public class Tools : MonoBehaviour
{
		private Animator animator;//tools animator
		public Animator drawResizerAnimator;//draw resizer animator
		public OnScreenDrawing onScreenDrawingComp;//OnScreenDrawing component

		// Use this for initialization
		void Start ()
		{
				animator = GetComponent<Animator> ();//get tools animator
				if (drawResizerAnimator == null) {
						drawResizerAnimator = GameObject.Find ("draw-resizer-background (Drawing Level)").GetComponent<Animator> ();
				}

				if (onScreenDrawingComp == null) {
						onScreenDrawingComp = Camera.main.GetComponent<OnScreenDrawing> ();
				}
		}

		//Fade In Color Resizer
		private void FadeInColorResizer ()
		{
				drawResizerAnimator.SetTrigger ("isFadingIn");
		}

		//After FadingIn Tools Animation
		private void AfterFadingInToolsAnimationEvent ()
		{
				animator.enabled = false;//disable tools animator
				onScreenDrawingComp.SetSelectedTool ();//set selected tool
				OnScreenDrawing.isRunning = true;//set running true
		}


		//Reset isFadingOut flag
		private void ResetIsFadingOutFlag ()
		{
				animator.SetBool ("isFadingOut", false);//set isFadingOut to false
		}
}
