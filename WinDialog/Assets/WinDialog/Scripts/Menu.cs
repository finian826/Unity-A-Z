using UnityEngine;
using System.Collections;

/// <summary>
/// Menu UI
/// </summary>
public class Menu : MonoBehaviour
{
		public Rect oneStarButtonRect;//one star rect
		public Rect twoStarsButtonRect;//two star rect
		public Rect threeStarsButtonRect;//three star rect
		public Rect background1ButtonRect;//background1 rect
		public Rect background2ButtonRect;//background2 rect
		public Rect background3ButtonRect;//background3 rect
		public Animator winDialogAnimator;//win dialog Animator component
		private SpriteRenderer topBackgroundSp;//top background SpriteRenderer component
		private bool showMenu = true;//show menu flag
		public Sprite[] backgrounds;//game backgrounds
		public static int starsNumber;//stars number
		public Font font;
		
		void Start ()
		{
				if (winDialogAnimator == null) {
						winDialogAnimator = GameObject.Find ("WinDialog").GetComponent<Animator> ();
				}
				if (topBackgroundSp == null) {
						topBackgroundSp = GameObject.Find ("top-background").GetComponent<SpriteRenderer> ();
				}
		}

		void OnGUI ()
		{
				if (!showMenu) {
						return;
				}
		       
				GUI.skin.font = font;
				GUI.skin.label.fontSize = Mathf.CeilToInt (Screen.width * 0.06f);
				GUI.skin.button.fontSize = Mathf.CeilToInt (Screen.width * 0.06f);

				if (GUI.Button (getRelativeScreenRect (oneStarButtonRect), "One Star")) {
						showMenu = false;
						starsNumber = 1;
						winDialogAnimator.SetBool ("isFadingIn", true);
				} else if (GUI.Button (getRelativeScreenRect (twoStarsButtonRect), "Two Stars")) {
						showMenu = false;
						starsNumber = 2;
						winDialogAnimator.SetBool ("isFadingIn", true);
				} else if (GUI.Button (getRelativeScreenRect (threeStarsButtonRect), "Three Stars")) {
						showMenu = false;
						starsNumber = 3;
						winDialogAnimator.SetBool ("isFadingIn", true);
				} else if (GUI.Button (getRelativeScreenRect (background1ButtonRect), "None")) {
						topBackgroundSp.sprite = null;
				} else if (GUI.Button (getRelativeScreenRect (background2ButtonRect), "Bg1")) {
						topBackgroundSp.sprite = backgrounds [0];
				} else if (GUI.Button (getRelativeScreenRect (background3ButtonRect), "Bg2")) {
						topBackgroundSp.sprite = backgrounds [1];
				}
		}

		//Get a Rect relative to Screen Width and Screen Height
		public static Rect getRelativeScreenRect (Rect rect)
		{
				int screenWidth = Screen.width;
				int screenHeight = Screen.height;
				return new Rect (Mathf.Ceil (rect.x * screenWidth), Mathf.Ceil (rect.y * screenHeight), Mathf.Ceil (rect.width * screenWidth), Mathf.Ceil (rect.height * screenHeight));
		}
}