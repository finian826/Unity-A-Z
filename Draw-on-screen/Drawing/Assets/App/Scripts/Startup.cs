using UnityEngine;
using System.Collections;

/// <summary>
/// This is the startup script of the game,you have to add LinesGroups in first scene of the game.
/// </summary>
public class Startup : MonoBehaviour
{
		public Animator introAnimator;//intro animator

		// Use this for initialization
		IEnumerator Start ()
		{
				const float delayTime = 5;//declare the delay time
				introAnimator = GameObject.Find ("Intro").GetComponent<Animator> ();//get intro animator
				introAnimator.SetTrigger ("isRunning");//start intro animation
				yield return new WaitForSeconds (delayTime);//wait a period of time
				Application.LoadLevel (Configuration.shapesGridSceneName);//load ShapesGrid Scene
		}
}
