using UnityEngine;
using System.Collections;

/// <summary>
/// Drawing level.
/// This script handles the inputs on the drawing tools,animations,and menus.
/// </summary>
public class DrawingLevel : MonoBehaviour
{
		public Sprite[] default_buttons;//default icons
		public Sprite[] click_buttons;//click icons
		public Sprite[] music_buttons;//music icons
		public Sprite[] sound_buttons;//sound icons
		public SpriteRenderer[] nextPrevSPComp;//next and previous sprite render components
		private float rayCast2dZPos = 0;//raycast zpos
		public Camera cam;//main camera (top camera)
		public Camera middleCam;//middle cam
		public OnScreenDrawing onScreenDrawingComp;//OnScreenDrawing component
		public Animator menuWindowAnimator;//menu window animator
		public Animator blakScreenAnimator;//black screen animator
		public Animator drawResizerBackgroundAnimator;//draw resizer background animator
		public Animator toolsAnimator;//tools animator
		public static bool isEraserSelected;//is eraser selected
		private ScreenShot screenShotComp;//ScreenShot component
		private bool isTakingScreenShot;//is taking a screen shot
		public GameObject[] Shapes;//drawing shapes
	
		// Use this for initialization
		IEnumerator Start ()
		{
				if (cam == null) {
						cam = Camera.main;
				}
		
				if (onScreenDrawingComp == null) {
						onScreenDrawingComp = cam.GetComponent<OnScreenDrawing> ();
				}
		
				if (menuWindowAnimator == null) {
						menuWindowAnimator = GameObject.Find ("menu-window (Drawing Level)").GetComponent<Animator> ();
				}
		
				if (blakScreenAnimator == null) {
						blakScreenAnimator = GameObject.Find ("black-screen").GetComponent<Animator> ();
				}
		
				if (drawResizerBackgroundAnimator == null) {
						drawResizerBackgroundAnimator = GameObject.Find ("draw-resizer-background (Drawing Level)").GetComponent<Animator> ();
				}
		
				if (toolsAnimator == null) {
						toolsAnimator = GameObject.Find ("Tools").GetComponent<Animator> ();
				}
		
				if (middleCam == null) {
						middleCam = GameObject.FindGameObjectWithTag ("MiddleCam").GetComponent<Camera> ();
				}
		
				screenShotComp = GetComponent<ScreenShot> ();
				CreateShape ();
				CheckNextPreviousAlpha ();
				CheckMusicIcon (SFX.instance.audioSources [0], GameObject.Find ("music_on (Drawing Level)"));
				CheckSoundIcon (SFX.instance.audioSources [1], GameObject.Find ("sound_on (Drawing Level)"));
				yield return new WaitForSeconds (0.5f);
				DrawingColor.isReady = true;
				yield return new WaitForSeconds (1.5f);
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.Escape)) {
						BackToHome ();
				}
				if (Input.GetMouseButtonDown (0)) {
			
						Vector3 clickPos = Input.mousePosition;
						clickPos.z = rayCast2dZPos - cam.transform.position.z;
						Vector3 wantedPos = Camera.main.ScreenToWorldPoint (clickPos);
			
						RaycastHit2D hit2d = Physics2D.Raycast (wantedPos, Vector2.zero);
			
						if (hit2d.collider != null) {
								GameObjectClickHandle (hit2d.collider.gameObject);
						} else {//if the click was not on any gameobject (have a collider)
								if (menuWindowAnimator.GetBool ("isRunningForward")) {
										menuWindowAnimator.SetTrigger ("isRunningBackword");
										menuWindowAnimator.SetBool ("isRunningForward", false);
										blakScreenAnimator.SetTrigger ("isFadingOut");
										blakScreenAnimator.SetBool ("isFadingIn", false);
								} else if (drawResizerBackgroundAnimator.GetBool ("isFadingIn")) {
										drawResizerBackgroundAnimator.SetTrigger ("isFadingOut");
										drawResizerBackgroundAnimator.SetBool ("isFadingIn", false);
										blakScreenAnimator.SetTrigger ("isFadingOut");
										blakScreenAnimator.SetBool ("isFadingIn", false);
								}
						}
				}
		}

		//handle the click on the screen (on which gameobject)
		private void GameObjectClickHandle (GameObject ob)
		{
				if (ob == null || isTakingScreenShot) {
						return;
				}
		
				Debug.Log (ob.name + " Clicked at " + Time.time);
				if (drawResizerBackgroundAnimator.GetBool ("isFadingIn")) {

						if (!(ob.name == "Draw-Resizer (Drawing Level)" || ob.tag == "WheelColor")) {
								if (ob.tag == "DrawSize") {
										onScreenDrawingComp.ToolsEvents (ob);
								}
								drawResizerBackgroundAnimator.SetTrigger ("isFadingOut");
								drawResizerBackgroundAnimator.SetBool ("isFadingIn", false);
								blakScreenAnimator.SetTrigger ("isFadingOut");
								blakScreenAnimator.SetBool ("isFadingIn", false);
						} else if (ob.tag == "WheelColor") {
								onScreenDrawingComp.ToolsEvents (ob);
						}
				} else if (menuWindowAnimator.GetBool ("isRunningForward")) {
						if (!(ob.tag == "Menu")) {
								menuWindowAnimator.SetTrigger ("isRunningBackword");
								menuWindowAnimator.SetBool ("isRunningForward", false);
								blakScreenAnimator.SetTrigger ("isFadingOut");
								blakScreenAnimator.SetBool ("isFadingIn", false);
						} else if (ob.name == "camera (Drawing Level)") {
								CaptureScreenShot ();
						} else if (ob.name == "right (Drawing Level)") {
								if (Shape.selectedShapeId < Shapes.Length - 1) {
										Shape.selectedShapeId++;
										CreateShape ();
										CheckNextPreviousAlpha ();
								}
						} else if (ob.name == "left (Drawing Level)") {
								if (Shape.selectedShapeId > 0) {
										Shape.selectedShapeId--;
										CreateShape ();
										CheckNextPreviousAlpha ();
								}
						} else if (ob.name == "facebook (Drawing Level)") {
								Application.OpenURL (Configuration.fbUrl);
						} else if (ob.name == "aboutUs (Drawing Level)") {
								Application.OpenURL (Configuration.aboutUsUrl);
						} else if (ob.name == "music_on (Drawing Level)") {
								SFX.instance.audioSources [0].mute = !SFX.instance.audioSources [0].mute;
								CheckMusicIcon (SFX.instance.audioSources [0], ob);
						} else if (ob.name == "sound_on (Drawing Level)") {
								SFX.instance.audioSources [1].mute = !SFX.instance.audioSources [1].mute;
								GameObject [] drawingColors = GameObject.FindGameObjectsWithTag ("WheelColor");
								AudioSource dcAc;
								foreach (GameObject dc in drawingColors) {
										dcAc = dc.GetComponent<AudioSource> ();
										dcAc.mute = !dcAc.mute;
								}
								CheckSoundIcon (SFX.instance.audioSources [1], ob);
						} 
				} else if (ob.name == "menu (Drawing Level)") {
						bool isRunningForward = menuWindowAnimator.GetBool ("isRunningForward");
						if (!isRunningForward) {
								GameObject.Find ("Shape").collider2D.enabled = false;
								OnScreenDrawing.isRunning = false;
								menuWindowAnimator.SetTrigger ("isRunningForward");
								blakScreenAnimator.SetBool ("isFadingOut", false);
								blakScreenAnimator.SetTrigger ("isFadingIn");
						} 
				} else if (ob.name == "home (Drawing Level)") {
						BackToHome ();
				} else if (ob.name == "Draw-Resizer (Drawing Level)") {
						bool isFadingIn = toolsAnimator.GetBool ("isFadingIn");

						if (!isFadingIn) {
								OnScreenDrawing.isRunning = false;
								toolsAnimator.enabled = true;
								toolsAnimator.SetBool ("isFadingIn", false);
								toolsAnimator.SetTrigger ("isFadingOut");
								blakScreenAnimator.SetBool ("isFadingOut", false);
								blakScreenAnimator.SetTrigger ("isFadingIn");
						}
				} else if (ob.tag == "WheelColor" || ob.name == "trash (Drawing Level)" || ob.name == "eraser (Drawing Level)" || ob.name == "pencil (Drawing Level)" || ob.name == "brush (Drawing Level)") {
						onScreenDrawingComp.ToolsEvents (ob);
				}

		}

		//capture screen shot
		private void CaptureScreenShot ()
		{
				if (screenShotComp == null)
						return;
				SFX.instance.audioSources [1].clip = SFX.instance.cameraSFX;
				AudioManager.PlayClip (SFX.instance.audioSources [1]);
				showCameraFlash ();
				StartCoroutine ("TakingScreenShot");
		}

		//taking a screen shot using IEnumerator
		private IEnumerator TakingScreenShot ()
		{
				isTakingScreenShot = true;
				if (screenShotComp == null)
						yield break;
		
				menuWindowAnimator.SetTrigger ("isRunningBackword");
				blakScreenAnimator.SetTrigger ("isFadingOut");
				yield return new WaitForSeconds (0.95f);
				screenShotComp.TakeShot ();
				isTakingScreenShot = false;
		}

		//show camera flash
		private void showCameraFlash ()
		{
				transform.Find ("camera-flash").GetComponent<CameraFlash> ().Flash ();
		}

		//back to main or home scene
		private void BackToHome ()
		{
				DrawingColor.isReady = false;
				Application.LoadLevel (Configuration.shapesGridSceneName);
		}

		//check sound icons
		private void CheckSoundIcon (AudioSource asource, GameObject ob)
		{
				if (asource == null || ob == null) {
						return;
				}
				if (asource.mute) {
						ob.GetComponent<SpriteRenderer> ().sprite = sound_buttons [1];
				} else {
						ob.GetComponent<SpriteRenderer> ().sprite = sound_buttons [0];
				}
		}

		//check music icons
		private void CheckMusicIcon (AudioSource asource, GameObject ob)
		{
				if (asource == null || ob == null) {
						return;
				}
				if (asource.mute) {
						ob.GetComponent<SpriteRenderer> ().sprite = music_buttons [1];
				} else {
						ob.GetComponent<SpriteRenderer> ().sprite = music_buttons [0];
				}
		}

		//check alpha for next and previous icons
		private void CheckNextPreviousAlpha ()
		{
				Color color;
				if (Shape.selectedShapeId == 0) {
						color = nextPrevSPComp [0].color;
						color.a = 0.4f;
						nextPrevSPComp [0].color = color;
						color = nextPrevSPComp [1].color;
						color.a = 1;
						nextPrevSPComp [1].color = color;
				} else if (Shape.selectedShapeId == Shapes.Length - 1) {
						color = nextPrevSPComp [0].color;
						color.a = 1;
						nextPrevSPComp [0].color = color;
						color = nextPrevSPComp [1].color;
						color.a = 0.4f;
						nextPrevSPComp [1].color = color;
				} else {
						color = nextPrevSPComp [0].color;
						color.a = 1;
						nextPrevSPComp [0].color = color;
						color = nextPrevSPComp [1].color;
						color.a = 1;
						nextPrevSPComp [1].color = color;
				}
		}

		//create current drawing shape
		private void CreateShape ()
		{
				GameObject currentShape = GameObject.Find ("Shape");//get current shape
				if (currentShape != null) {
						Destroy (currentShape);
				}
		
				bool loadFromResources = false;//load from resources flag
				int selectedShapeId = Shape.selectedShapeId;
				if (Shapes == null) {
						loadFromResources = true;
				} else {
						if (!(selectedShapeId >= 0 && selectedShapeId < Shapes.Length)) {//if the selected shape is out of array boundary
								return;
						} else {
								if (Shapes [selectedShapeId] == null) {
										loadFromResources = true;
								}
						}
				}
		
				GameObject prefab = null;
				if (loadFromResources) {
						prefab = Resources.Load ("shapes/" + selectedShapeId + "/shape") as GameObject;//load the shape from resources folder
				} else {
						prefab = Shapes [selectedShapeId];//getting the shape from shapes array
				}
				GameObject shape = (GameObject)Instantiate (prefab, new Vector3 (0, 1, -38), Quaternion.identity);//create the shape
				shape.name = "Shape";//setting up shape name
		
				if (menuWindowAnimator.GetBool ("isRunningForward")) {
						shape.collider2D.enabled = false;//disable shape collider
				}
				shape.transform.parent = middleCam.transform;//setting up shape parent
				onScreenDrawingComp.LoadLines ();//load shape lines
		}
}