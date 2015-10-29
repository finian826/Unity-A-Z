//Attach to main camera
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture backgroundTexture;
	public Texture button;

	void OnGUI(){

		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), backgroundTexture);

		if(GUI.Button(new Rect(Screen.width*0.35f, Screen.height*0.45f, Screen.width*0.3f, Screen.height*0.3f),button ,"")){
			Application.LoadLevel("wone");
		}
	}


}
