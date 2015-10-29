using UnityEngine;
using System.Collections;

/// <summary>
/// Black screen.
/// </summary>
public class BlackScreen : MonoBehaviour
{
		private Animator animator;//black screen animator
		// Use this for initialization
		void Start ()
		{
				animator = GetComponent<Animator> ();
		}

		//Reset paramters fater fadng out animation
		private void ResetAfterFadingOutAnimation ()
		{
				animator.SetBool ("isFadingIn", false);
				animator.SetBool ("isFadingOut", false);
		}
}
