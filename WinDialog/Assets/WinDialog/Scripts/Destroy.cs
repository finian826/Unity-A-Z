using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour
{
		public float destroyTime = 1;

		// Use this for initialization
		void Start ()
		{
				Destroy (gameObject, destroyTime);
		}
}
