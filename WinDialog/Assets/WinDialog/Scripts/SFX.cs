using UnityEngine;
using System.Collections;
/// <summary>
/// Put your sound effects refernces here
/// </summary>
public class SFX : MonoBehaviour
{
		public AudioClip[] starsSFX;
		public AudioSource[] audioSources;
		public static SFX instance;

		// Use this for initialization
		void Start ()
		{
				instance = this;
				audioSources = GetComponents<AudioSource> ();
		}
}