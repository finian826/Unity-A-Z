using UnityEngine;
using System.Collections;

/// <summary>
/// Drawing level menu window.
/// </summary>
public class DrawingLevelMenuWindow : MonoBehaviour
{
		private Animator animator;//drawing level menu
		// Use this for initialization
		void Start ()
		{
				animator = GetComponent<Animator> ();
		}

		//reset after backward animation
		private void ResetAfterBackWardAnimation ()
		{
				animator.SetBool ("isRunningForward", false);
				animator.SetBool ("isRunningBackword", false);
				OnScreenDrawing.isRunning = true;
		}
}