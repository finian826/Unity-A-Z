using UnityEngine;
using System.Collections;

///Implement your game events in this script
public class Events : MonoBehaviour
{
		//Load scene1
		public void LoadPicturesWorld (Object ob)
		{
				Application.LoadLevel ("Scene1");
		}

		//Load scene2
		public void LoadWordsWorld (Object ob)
		{
				Application.LoadLevel ("Scene2");
		}

		//Load Home scene
		public void LoadHome (Object ob)
		{
				Application.LoadLevel ("Home");
		}

		//Quit the app
		public void QuitApplication (Object ob)
		{
				Application.Quit ();
		}
}