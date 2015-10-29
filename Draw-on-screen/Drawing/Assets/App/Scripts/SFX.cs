using UnityEngine;
using System.Collections;

/// <summary>
/// You can store SFX refernces in this script
/// audioSources[0] used for music,audioSources[1] used for sounds
/// </summary>
/// 
public class SFX : MonoBehaviour
{
		public AudioClip cameraSFX;
		public AudioClip musicSFX;
		public AudioClip wizzySFX;
		public AudioClip slideSFX;
		public AudioSource[] audioSources;
		public static SFX instance;//static instance

		// Use this for initialization
		void Awake ()
		{
				if (!instance) {
						instance = this;
				}
				audioSources = GetComponents<AudioSource> ();
		}
}
