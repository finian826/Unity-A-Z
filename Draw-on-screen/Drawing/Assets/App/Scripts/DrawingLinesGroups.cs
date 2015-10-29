using UnityEngine;
using System.Collections;

/// <summary>
/// Drawing lines groups.
/// </summary>
public class DrawingLinesGroups : MonoBehaviour
{
		public static DrawingLinesGroups instance;

		void Awake ()
		{
				if (instance == null) {
						instance = this;
						DontDestroyOnLoad (this);
				} else {
						DestroyImmediate (gameObject);//destroy this gameobject if it's already exist
				}
		}

		//disable lines groups
		public void ResetMyLines ()
		{
				int childCount = transform.childCount;
				for (int i = 0; i < childCount; i++) {
						transform.GetChild (i).gameObject.SetActive (false);
				}
		}
}