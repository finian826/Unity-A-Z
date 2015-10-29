using UnityEngine;
using System.Collections;

/// <summary>
/// Audio manager.
/// </summary>
public class AudioManager : MonoBehaviour
{
		//this method plays audio clip of given audio source
		public static void PlayClip (AudioSource source)
		{
				if (source == null) {
						return;
				}
				if (source.clip == null) {
						return;
				}
				
				source.Play ();
		}
	
		//this method stopes audio clip of given audio source
		public static void StopClip (AudioSource source)
		{
				if (source == null) {
						return;
				}
				if (source.clip == null) {
						return;
				}
			
				source.Stop ();
		}

		public void FadeInClip (AudioSource source, float speed, float max)
		{
				if (source == null) {
						return;
				}
				
				StopAllCoroutines ();
				StartCoroutine (FadeInCoroutine (source, speed, max));
		}

		public void FadeOutClip (AudioSource source, float speed, float min)
		{
				if (source == null) {
						return;
				}
				
				StopAllCoroutines ();
				StartCoroutine (FadeOutCoroutine (source, speed, min));
		}

		private IEnumerator FadeInCoroutine (AudioSource source, float speed, float max)
		{
				while (source.volume<max) {
						source.volume += speed * Time.deltaTime;
						yield return new WaitForSeconds (0.001f);
				}
				source.volume = max;
		}

		private IEnumerator FadeOutCoroutine (AudioSource source, float speed, float min)
		{

				while (source.volume>min) {
						source.volume -= speed * Time.deltaTime;
						yield return new WaitForSeconds (0.001f);
				}
				source.volume = min;
		}

		public static void ResetAudioSource (AudioSource audioSource)
		{
				if (audioSource == null) {
						return;
				}
				audioSource.loop = false;
				audioSource.playOnAwake = false;
				audioSource.spread = 181;
		}
}