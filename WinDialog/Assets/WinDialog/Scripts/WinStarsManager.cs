using UnityEngine;
using System.Collections;

/// <summary>
/// Window stars manager.
/// </summary>
public class WinStarsManager : MonoBehaviour
{
		public Animator[] starsAnimators;

		// Use this for initialization
		void Start ()
		{
				if (starsAnimators [0] == null) {
						starsAnimators [0] = GameObject.Find ("WinDialog/Stars/star1").GetComponent<Animator> ();
				}
				if (starsAnimators [1] == null) {
						starsAnimators [1] = GameObject.Find ("WinDialog/Stars/star2").GetComponent<Animator> ();
				}
				if (starsAnimators [2] == null) {
						starsAnimators [2] = GameObject.Find ("WinDialog/Stars/star3").GetComponent<Animator> ();
				}
		}

		public void PrepareStars (int starsCount)
		{
				StartCoroutine (PreparingStars (starsCount));
		}

		private IEnumerator PreparingStars (int starsCount)
		{
				float starsDelay = 0.7f;
			
				if (starsCount == 1) {//One Star
						starsAnimators [0].SetTrigger ("isRunning");
				} else if (starsCount == 2) {//Two Stars
						starsAnimators [0].SetTrigger ("isRunning");
						yield return new WaitForSeconds (starsDelay);
						starsAnimators [1].SetTrigger ("isRunning");
				} else if (starsCount == 3) {//Three Stars
						starsAnimators [0].SetTrigger ("isRunning");
						yield return new WaitForSeconds (starsDelay);
						starsAnimators [1].SetTrigger ("isRunning");
						yield return new WaitForSeconds (starsDelay);
						starsAnimators [2].SetTrigger ("isRunning");
				}
		}
}