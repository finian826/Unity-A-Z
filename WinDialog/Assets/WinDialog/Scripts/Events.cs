using UnityEngine;
using System.Collections;

/// <summary>
/// Implement you events here with the given signiture : Access-Modifier void MethodName(Object ob)
/// </summary>
/// 
public class Events : MonoBehaviour
{
		// Use this for initialization
		void Start ()
		{
				
		}
	
		private void OnStagesClick (Object obj)
		{
				Debug.Log ("On Stages Click Event");
		}

		private void OnReplayClick (Object obj)
		{
				Debug.Log ("On Replay Click Event");
				StartCoroutine ("ReplayGame");
		}

		private void OnEsacpeClick (Object obj)
		{
				Debug.Log ("On Escape Click Event");
				Application.Quit ();
		}

		private IEnumerator ReplayGame ()
		{
				yield return new WaitForSeconds (0.25f);
				Application.LoadLevel ("Demo");
		}
}