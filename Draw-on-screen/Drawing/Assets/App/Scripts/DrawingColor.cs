using UnityEngine;
using System.Collections;

/// <summary>
/// Drawing color.
/// </summary>
public class DrawingColor : MonoBehaviour
{
		public Color color;
		public Material drawingMaterial;//drawing material
		public static Material whiteMaterial;//white color material
		public static float thickness1 = 0.2f;//drawing color low thinkness
		public static float thickness2 = 0.4f;//drawing color middle thinkness
		public static float thickness3 = 0.7f;//drawing color high thinkness
		private AudioSource asource;
		public static bool isReady = false;

		//use this for initialization
		void Awake ()
		{
				drawingMaterial = new Material (Shader.Find ("GUI/Text Shader"));//create drawing color material
				whiteMaterial = new Material (Shader.Find ("GUI/Text Shader"));//create white color material
				whiteMaterial.color = Color.white;//setting up white material color
				drawingMaterial.color = color;//setting up drawing material color
				asource = GetComponent<AudioSource> ();
		}

		//when drwing color became visible
		void OnBecameVisible ()
		{
				if (!isReady) {
						return;
				}

				if (asource == null) {
						return;
				}
				AudioManager.PlayClip (asource);
		}
}