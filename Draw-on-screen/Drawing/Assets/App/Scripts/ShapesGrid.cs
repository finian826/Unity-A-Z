using UnityEngine;
using System.Collections;

/// <summary>
/// Shapes grid scrolling handler.
/// Note : shapes grid contains a set of groups each groud contains a set of borders.
/// To create new group just create new gameobject with the given name 'group_lastIndex'
/// such that lastInex >=0,also to create new border just create new gameobject with the given
/// name 'border (ShapesGrid)' ,and then you can change the transofrm as you want.
/// </summary>
/// 
public class ShapesGrid : MonoBehaviour
{
		private bool isDragging;//is shapes grid is dragging
		private bool isLerpingToTarget;//is lerping to a traget
		private Vector3 mousePos, mousePosInWorldSpace;
		private Vector3 obPositon;//shapes grid position
		private Vector3 prevPos, lastPos;//previous and last positions
		private GameObject clickedOb;//which gameobject you clicked
		private float xOffset;//offset between shapesgrid and mouse position in the world space
		public Camera cam;//your camera
		public Transform obTransform;//shapes grid transform
		public Transform centerPoint;//center point of the shapesgrid
		private int groupCount;//number of groups
		public SpriteRenderer[]arrows;//arrows sprite renderers
		public SpriteRenderer[]slides;//slides sprite renderers
		public Sprite[] arrowIcons, borderIcons;//icons
		private bool isRunning = true;
		private bool isBorderClickIgnored;//is border click ingored or canceld
		private bool isClickedOnBorder;//is there a click on a border
		private AudioManager audioManagerComp;

		void Awake ()
		{
				GameObject.Find ("Lines" + Shape.selectedShapeId).SetActive (false);//diable current lines of the current shape
		}

		//Use this for Initialization
		IEnumerator Start ()
		{
				if (cam == null) {
						cam = Camera.main;
				}
				
				if (audioManagerComp == null) {
						audioManagerComp = SFX.instance.gameObject.GetComponent<AudioManager> ();
				}

				lastPos = prevPos = Vector3.zero;
				obTransform = transform;

				if (centerPoint == null) {
						centerPoint = GameObject.Find ("Center").transform;
				}
				groupCount = obTransform.childCount;
				SetGridIcons (1);
				yield return 0;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (!isRunning) {
						return;
				}

				if (Input.GetKeyDown (KeyCode.Escape)) {//when you click on escape button
						Application.Quit ();
				}
				if (Input.GetMouseButtonDown (0)) {
						mousePos = Input.mousePosition;//get mouse position
			 
						Vector3 wantedPos = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 0 - cam.transform.position.z));
						RaycastHit2D hit2d = Physics2D.Raycast (wantedPos, Vector2.zero);

						bool doDrag = false;

						if (hit2d.collider != null) {
								string obName = hit2d.collider.name;
								if (obName == "right-slider (ShapesGrid)" || obName == "right-arrow (ShapesGrid)") {//if you clicked on right-slider or right-arrow
										MoveToGroup (int.Parse (GetClosestGroupToCenter ().name.Split ('_') [1]) + 1);
								} else if (obName == "left-slider (ShapesGrid)" || obName == "left-arrow (ShapesGrid)") {//if you clicked on left-slider or left-arrow
										MoveToGroup (int.Parse (GetClosestGroupToCenter ().name.Split ('_') [1]) - 1);
								} else if (obName == "border (ShapesGrid)") {//if you click on a border
										isClickedOnBorder = true;
										clickedOb = hit2d.collider.gameObject;
										clickedOb.GetComponent<SpriteRenderer> ().sprite = borderIcons [1];//set border hover(selected) icon
										doDrag = true;
								} else {
										doDrag = true;
								}
						} else {
								doDrag = true;
						}
						if (doDrag) {//if do a drag
								lastPos = prevPos = obTransform.position;
								StopAllCoroutines ();
								isLerpingToTarget = false;
								mousePosInWorldSpace = cam.ScreenToWorldPoint (mousePos);
								xOffset = mousePosInWorldSpace.x - obTransform.position.x;
								isDragging = true;
						}
				} else if (Input.GetMouseButtonUp (0)) {//when mouse click released
						if (isDragging) {//if was you dragging
								isDragging = false;
								isLerpingToTarget = false;
								StopAllCoroutines ();

								Vector3 wantedPos = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 0 - cam.transform.position.z));
								RaycastHit2D hit2d = Physics2D.Raycast (wantedPos, Vector2.zero);
								if (hit2d.collider != null) {
										string obTag = hit2d.collider.tag;
										if (obTag != "Border") {//if your click was not on a border
												ResetBorderIcons ();
										} else {
												if (!isBorderClickIgnored && isClickedOnBorder) {//if your click released on a border without click ingoring 
														isRunning = false;
														Shape.selectedShapeId = hit2d.collider.gameObject.GetComponent<Shape> ().shapeId;
														Application.LoadLevel (Configuration.drawingLevelSceneName);//load selected shape
												}
										}
								} else {
										ResetBorderIcons ();
								}

								isClickedOnBorder = false;
								isBorderClickIgnored = false;
								float distance = lastPos.x - prevPos.x;
								float sign = Mathf.Sign (distance);
								
								bool isMovedToGroup = false;
								if (Mathf.Abs (distance) >= 0.05f) {
										if (sign == 1) {
												isMovedToGroup = MoveToGroup (int.Parse (GetClosestGroupToCenter ().name.Split ('_') [1]) - 1);
										} else {
												isMovedToGroup = MoveToGroup (int.Parse (GetClosestGroupToCenter ().name.Split ('_') [1]) + 1);
										}
								}
								if (!isMovedToGroup) {
										StartCoroutine ("LerpClosestToCenter");
								}
						}
				}
		}
	
		void FixedUpdate ()
		{
				if (!isRunning) {
						return;
				}
				if (isDragging) {//drag shapesgrid
						mousePos = Input.mousePosition;
						mousePosInWorldSpace = cam.ScreenToWorldPoint (mousePos);
						obPositon = transform.position;//get current shapesgrid position
						obPositon.x = mousePosInWorldSpace.x - xOffset;//set shapesgrid position
						obTransform.position = obPositon;
						prevPos = lastPos;
						lastPos = obTransform.position;
						if (Mathf.Abs (lastPos.x - prevPos.x) >= 0.06f) {
								ResetBorderIcons ();
						}
				}
		}

		//get the closest group to the center point
		private Transform GetClosestGroupToCenter ()
		{
				Transform child;
				Transform closestChild;
				Vector2 v1 = new Vector2 (centerPoint.position.x, 0);
				Vector2 v2 = Vector2.zero;
				Vector3 v3 = Vector2.zero;
				closestChild = obTransform.GetChild (0);

				for (int i = 1; i < groupCount; i++) {
						child = obTransform.GetChild (i);
						v2.x = closestChild.position.x;
						v3.x = child.position.x;
						if (Vector2.Distance (v1, v2) > Vector2.Distance (v1, v3)) {
								closestChild = child;
						}
				}
				return closestChild;
		}

		//lerp or move to the closed group to the center point
		private IEnumerator LerpClosestToCenter ()
		{
				Transform closestToCenter = GetClosestGroupToCenter ();
				SetGridIcons (int.Parse (GetClosestGroupToCenter ().name.Split ('_') [1]));
				float xCenterOffset = closestToCenter.position.x - centerPoint.position.x;
				float tragetX = obTransform.position.x - xCenterOffset;
				isLerpingToTarget = true;
				yield return StartCoroutine (LerpToTarget (tragetX));
		}

		//lerp to a traget
		private IEnumerator LerpToTarget (float tragetX)
		{
				float lerpSpeed = 4;
				Vector3 wantedPos = Vector3.zero;

				while (Mathf.Abs(obTransform.position.x-tragetX) >= 0.12f) {
						wantedPos = obTransform.position;
						wantedPos.x = Mathf.Lerp (obTransform.position.x, tragetX, lerpSpeed * Time.deltaTime);
						obTransform.position = wantedPos;
						yield return 0;
				}
				isLerpingToTarget = false;
		}

		//move to a certain group
		private bool MoveToGroup (int groupNumber)
		{
				if (isLerpingToTarget) {
						return false;
				}

				if (groupNumber >= 1 && groupNumber <= groupCount) {
						//play sfx for slide
						SFX.instance.audioSources [1].clip = SFX.instance.slideSFX;
						AudioManager.PlayClip (SFX.instance.audioSources [1]);//play slide sfx
						SetGridIcons (groupNumber);

						string groupName = "group_" + groupNumber;//get group name
						float xTargetOffset = GameObject.Find (groupName).transform.position.x - centerPoint.position.x;
						isLerpingToTarget = true;
						StartCoroutine (LerpToTarget (obTransform.position.x - xTargetOffset));//move or lerp to this group
						return true;
				}
				return false;
		}

		//set grid icons
		private void SetGridIcons (int groupNumber)
		{
				Color tempColor;
				if (groupNumber == 1) {
						//Setting Arrows Images
						tempColor = arrows [0].color;
						tempColor.a = 100 / 255.0f;
						arrows [0].color = tempColor;

						tempColor = arrows [1].color;
						tempColor.a = 1;
						arrows [1].color = tempColor;

						//Setting Sliders Images
						tempColor = slides [0].color;
						tempColor.r = 73 / 255.0f;
						tempColor.g = 81 / 255.0f;
						tempColor.b = 79 / 255.0f;
						tempColor.a = 111 / 255.0f;
						slides [0].color = tempColor;

						tempColor = slides [1].color;
						tempColor.r = 1;
						tempColor.g = 1;
						tempColor.b = 1;
						tempColor.a = 1;
						slides [1].color = tempColor;
			
				} else if (groupNumber == groupCount) {
						//Setting Arrows Images
		
						tempColor = arrows [0].color;
						tempColor.a = 1;
						arrows [0].color = tempColor;

						tempColor = arrows [1].color;
						tempColor.a = 100 / 255.0f;
						arrows [1].color = tempColor;

						//Setting Sliders Images
						tempColor = slides [0].color;
						tempColor.r = 1;
						tempColor.g = 1;
						tempColor.b = 1;
						tempColor.a = 1;
						slides [0].color = tempColor;

						tempColor = slides [1].color;
						tempColor.r = 73 / 255.0f;
						tempColor.g = 81 / 255.0f;
						tempColor.b = 79 / 255.0f;
						tempColor.a = 111 / 255.0f;
						slides [1].color = tempColor;
		
				} else {
						//Setting Arrows Images
						tempColor = arrows [0].color;
						tempColor.a = 1;
						arrows [0].color = tempColor;
			
						tempColor = arrows [1].color;
						tempColor.a = 1;
						arrows [1].color = tempColor;
			
						//Setting Sliders Images
						tempColor = slides [0].color;
						tempColor.r = 1;
						tempColor.g = 1;
						tempColor.b = 1;
						tempColor.a = 1;
						slides [0].color = tempColor;
			
						tempColor = slides [1].color;
						tempColor.r = 1;
						tempColor.g = 1;
						tempColor.b = 1;
						tempColor.a = 1;
						slides [1].color = tempColor;
				}
		}

		//Reset last selected border icon to default
		private void ResetBorderIcons ()
		{
				if (clickedOb == null) {
						return;
				}
				if (clickedOb.tag == "Border") {
						isBorderClickIgnored = true;
						clickedOb.GetComponent<SpriteRenderer> ().sprite = borderIcons [0];

				}
				clickedOb = null;
		}
}