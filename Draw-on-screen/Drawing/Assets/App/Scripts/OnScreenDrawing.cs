
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Drawing lines on screen using line renderer.
/// </summary>
/// 
public class OnScreenDrawing : MonoBehaviour
{
		public static bool isRunning = true;
		public Camera middleCam;// middleCamera 
		public GameObject drawResizerOb;//draw resizer
		private GameObject drawResizerBackground;//draw resizer background
		public SpriteRenderer circleColorSpriteRendererComp;//circle color sprite renderer
		private FontSizeType fontSizeType = FontSizeType.PEN;//font size type
		private FontSize fontSize = FontSize.MEDIUM;//font size
		public FollowTarget penObFollowTarget, brushObFollowTarget, deleteObFollowTarget;//follow targets
		public Animator toolsAnimator;//tools animator
		private ColorsWheelController colorsWheelControllerComp;//color wheel controller
		private SelectedTool selectedTool;//selected tool
		public GameObject lineRenderPrefab;//line renderer prefab
		private static Material drawingMaterial;//current drawing material
		private static Material previousMaterial;//previous material
		public Color lineColor = Color.black;//current line color
		private Color prevColor;//previous line color
		private bool canDraw;//can you draw
		private Vector3 prevPoint;//previous point
		public float lineRenderWidth;//line rendere width
		private float maxLineZ;//max line renderer z-position
		private	float currentLineZ;//curent line rendere z-position
		private const int firstMaxLineZ = -33;//first max line rendere z-position
		private Material lineMaterial;//line rendere material
		private GameObject currentLineRender = null;//current line renderer
		private DrawingColor drawingColorComp;//drawing color

		//Use this for Initialization
		void Start ()
		{
				fontSizeType = FontSizeType.PEN;
				fontSize = FontSize.MEDIUM;
				selectedTool = SelectedTool.PEN;
				lineColor = new Color (0, 147, 68, 100) / 255.0f;
				prevColor = lineColor;
				drawingMaterial = GameObject.Find ("green (Drawing Level)").GetComponent<DrawingColor> ().drawingMaterial;
				previousMaterial = drawingMaterial;

				if (middleCam == null) {
						middleCam = GameObject.FindGameObjectWithTag ("MiddleCam").GetComponent<Camera> ();//setting up middle camera reference
				}

				if (drawResizerOb == null) {
						drawResizerOb = GameObject.Find ("Draw-Resizer (Drawing Level)");
				}

				if (toolsAnimator == null) {
						toolsAnimator = GameObject.Find ("Tools").GetComponent<Animator> ();
				}

				if (brushObFollowTarget == null) {
						brushObFollowTarget = GameObject.Find ("brush (Drawing Level)").GetComponent<FollowTarget> ();
				}

				if (deleteObFollowTarget == null) {
						deleteObFollowTarget = GameObject.Find ("eraser (Drawing Level)").GetComponent<FollowTarget> ();
				}

				if (penObFollowTarget == null) {
						penObFollowTarget = GameObject.Find ("pencil (Drawing Level)").GetComponent<FollowTarget> ();
				}

				if (colorsWheelControllerComp == null) {
						colorsWheelControllerComp = GameObject.Find ("ColorsWheel (Drawing Level)").GetComponent<ColorsWheelController> ();
				}
	
				drawResizerBackground = drawResizerOb.transform.Find ("draw-resizer-background (Drawing Level)").gameObject;
				SetDrawResizerColor ();
				SetFontSize ();
		}
	
		void Update ()
		{
				if (!isRunning) {
						return;
				}

				if (Input.GetMouseButtonDown (0)) {//if click began on screen
						GameObject ob;
						Vector3 mousePos = Input.mousePosition;//get mouse position
						Vector3 wantedPos = middleCam.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 0 - middleCam.transform.position.z));
						RaycastHit2D hit2d = Physics2D.Raycast (wantedPos, Vector2.zero);
						if (hit2d.collider != null) {
								ob = hit2d.transform.gameObject;
								if (ob.tag == "DrawingArea" || ob.tag == "DrawingShape") {//if you click on drawing shape or drawing area
										canDraw = true;
								} 
						}
				} else if (Input.GetMouseButtonUp (0)) {//when click released
						canDraw = false;
						currentLineRender = null;
				}
		}

		void FixedUpdate ()
		{
				//if can draw, then make drawing
				if (canDraw) {
						MakeDrawing ();
				}
		}

		//draw lines
		private void MakeDrawing ()
		{
				GameObject ob;
				Vector3 mousePos = Input.mousePosition;//get mouse position
				Vector3 wantedPos = middleCam.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 0 - middleCam.transform.position.z));
				RaycastHit2D hit2d = Physics2D.Raycast (wantedPos, Vector2.zero);
				if (hit2d.collider != null) {
						ob = hit2d.transform.gameObject;
						if (!(ob.tag == "DrawingArea" || ob.tag == "DrawingShape")) {
								return;
						}
				} else {
						return;
				}
		
				if (currentLineRender == null) {//there current line rendere is null , then instanitae new one
						GameObject currentLines = GameObject.Find ("Lines" + Shape.selectedShapeId);
						currentLineRender = Instantiate (lineRenderPrefab) as GameObject;//instaiate object
						currentLineRender.transform.parent = currentLines.transform;
						LineRenderer lineRenderComp = currentLineRender.GetComponent<LineRenderer> ();
						lineRenderComp.SetWidth (lineRenderWidth, lineRenderWidth);
						lineRenderComp.material = drawingMaterial;
						MyLines myLinesComp = currentLines.GetComponent<MyLines> ();
						this.currentLineZ -= 0.0001f;
						myLinesComp.currentLineZ = this.currentLineZ;
				}
		
				Vector3 MousePos = Input.mousePosition;
				Vector3 worldpos = middleCam.ScreenToWorldPoint (MousePos);
				worldpos.z = currentLineZ;

				if (Mathf.Abs (currentLineZ) >= Mathf.Abs (maxLineZ)) {//if current line z >= maxlinez ,then set the next attributes
						this.maxLineZ -= 33;//decrease maxlineZ
						middleCam.transform.Translate (new Vector3 (0, 0, -40));//translate middleCamera in the z-position
						middleCam.farClipPlane += 40;//increase middleCamera far clip plane
			
						//Save status into current MyLines
						MyLines myLinesComp = GameObject.Find ("Lines" + Shape.selectedShapeId).GetComponent<MyLines> ();
						myLinesComp.maxLineZ = this.maxLineZ;
						myLinesComp.currentLineZ = this.currentLineZ;
						Vector3 middleCampos = middleCam.transform.localPosition;
						myLinesComp.cameraZPostition = middleCampos.z;
						myLinesComp.farClipPlane = middleCam.farClipPlane;
				}
		
				float distance = Vector3.Distance (worldpos, prevPoint);
		
				prevPoint = worldpos;
				if (distance >= 0.03f) {
						LineRenderer ln = currentLineRender.GetComponent<LineRenderer> ();
						LineRenderAttribites line_attributes = currentLineRender.GetComponent<LineRenderAttribites> ();
						int numberOfPoints = line_attributes.NumberOfPoints;
						numberOfPoints++;
						line_attributes.NumberOfPoints = numberOfPoints;
						ln.SetVertexCount (numberOfPoints);
						ln.SetPosition (numberOfPoints - 1, worldpos);
				}
		}

		//Clear Board (clear lines of the current shape)
		private void ClearBoard ()
		{
				GameObject currentLines = GameObject.Find ("Lines" + Shape.selectedShapeId);
				int childCount = currentLines.transform.childCount;
				for (int i = 0; i < childCount; i++) {
						Destroy (currentLines.transform.GetChild (i).gameObject);	
				}
				this.maxLineZ = firstMaxLineZ;
				this.currentLineZ = 0;
				Vector3 middleCampos = middleCam.transform.localPosition;
				middleCam.transform.localPosition = new Vector3 (middleCampos.x, middleCampos.y, -39);
				middleCam.farClipPlane = 40;
				setDefaultParametersForMyLine (currentLines.GetComponent<MyLines> ());
		}

		//load lines of the current(selected) shape
		public void LoadLines ()
		{
				StartCoroutine (LoadLinesCorouine ());
		}

		//load the lines of the current shape
		IEnumerator LoadLinesCorouine ()
		{
				GameObject linesGroups = GameObject.Find ("LinesGroups");
				int childCount = linesGroups.transform.childCount;
		
				for (int i = 0; i < childCount; i++) {
						GameObject childob = linesGroups.transform.GetChild (i).gameObject;
						MyLines myLinesComp = childob.GetComponent<MyLines> ();//get MyLines component
					
						if (childob.name == "Lines" + Shape.selectedShapeId) {//if these are the lines of the current shape
								this.maxLineZ = myLinesComp.maxLineZ;
								this.currentLineZ = myLinesComp.currentLineZ;
								Vector3 middleCampos = middleCam.transform.localPosition;
								middleCam.transform.localPosition = new Vector3 (middleCampos.x, middleCampos.y, myLinesComp.cameraZPostition);
								middleCam.farClipPlane = myLinesComp.farClipPlane;
								childob.SetActive (true);
						} else {
								childob.SetActive (false);
						}
				}
				yield return 0;
		}

		//set default parameters for myline
		private void setDefaultParametersForMyLine (MyLines myLinesComp)
		{
				myLinesComp.maxLineZ = this.maxLineZ;
				myLinesComp.currentLineZ = this.currentLineZ;
				myLinesComp.cameraZPostition = middleCam.transform.localPosition.z;
				myLinesComp.farClipPlane = middleCam.farClipPlane;
		}

		//tools events
		public void ToolsEvents (GameObject toolOb)
		{
				if (toolOb == null) {
						return;
				}

				string toolName = toolOb.name;
				string obTag = toolOb.tag;
				bool checkSelectedTool = false;
				bool checkFontSize = false;
				bool checkPrevColor = false;
				bool checkDrawResizerColor = false;

				if (toolName == "small-size-circle (Drawing Level)") {
						fontSize = FontSize.SMALL;
						checkFontSize = true;
						checkDrawResizerColor = true;
				} else if (toolName == "med-size-circle (Drawing Level)") {
						fontSize = FontSize.MEDIUM;
						checkFontSize = true;
						checkDrawResizerColor = true;
				} else if (toolName == "larg-size-circle (Drawing Level)") {
						fontSize = FontSize.LARGE;
						checkFontSize = true;
						checkDrawResizerColor = true;
				} else if (toolName == "trash (Drawing Level)") {
						ClearBoard ();
						colorsWheelControllerComp.ScrollingRelease (360, 0);
				} else if (obTag == "WheelColor") {
						checkDrawResizerColor = true;
						checkPrevColor = true;
						drawingColorComp = toolOb.GetComponent<DrawingColor> ();
						lineColor = drawingColorComp.color;
						drawingMaterial = drawingColorComp.drawingMaterial;
						previousMaterial = drawingMaterial;
						if (DrawingLevel.isEraserSelected) {
								checkSelectedTool = true;
								checkFontSize = true;
								fontSizeType = FontSizeType.PEN;
								selectedTool = SelectedTool.PEN;
						}
				} else if (toolName == "eraser (Drawing Level)") {
						DrawingLevel.isEraserSelected = true;
						selectedTool = SelectedTool.DELETE;
						checkDrawResizerColor = true;
						checkSelectedTool = true;
						lineColor = Color.white;
						drawingMaterial = DrawingColor.whiteMaterial;
				} else if (toolName == "pencil (Drawing Level)") {
						DrawingLevel.isEraserSelected = false;
						selectedTool = SelectedTool.PEN;
						checkDrawResizerColor = true;
						checkSelectedTool = true;
						checkFontSize = true;
						fontSizeType = FontSizeType.PEN;
						lineColor = prevColor;
						drawingMaterial = previousMaterial;
				} else if (toolName == "brush (Drawing Level)") {
						DrawingLevel.isEraserSelected = false;
						selectedTool = SelectedTool.BRUSH;
						checkDrawResizerColor = true;
						checkSelectedTool = true;
						checkFontSize = true;
						fontSizeType = FontSizeType.BRUSH;
						lineColor = prevColor;
						drawingMaterial = previousMaterial;
				}

				if (checkDrawResizerColor) {
						if (checkPrevColor) {
								prevColor = lineColor;
						}
						SetDrawResizerColor ();
				} 

				if (checkSelectedTool) {
						SetSelectedTool ();
				}

				if (checkFontSize) {
						SetFontSize ();
				}
		}

		//set drawing resizer color
		private void SetDrawResizerColor ()
		{
				if (drawResizerOb == null) {
						return;
				}
		 
				Color tempColor = lineColor;
				tempColor.a = 1;
				circleColorSpriteRendererComp.color = tempColor;

				drawResizerOb.GetComponent<SpriteRenderer> ().color = tempColor;
		
				GameObject child;
				int childCount = drawResizerBackground.transform.childCount;
				SpriteRenderer sp;
				string name = "";
				
				for (int i = 0; i < childCount; i++) {
						child = drawResizerBackground.transform.GetChild (i).gameObject;
						sp = child.GetComponent<SpriteRenderer> ();

						name = child.name;

						if (name.Contains ("larg") && fontSize == FontSize.LARGE) {
								tempColor.a = 1;
						} else	if (name.Contains ("med") && fontSize == FontSize.MEDIUM) {
								tempColor.a = 1;
						} else if (name.Contains ("small") && fontSize == FontSize.SMALL) {
								tempColor.a = 1;
						} else {
								tempColor.a = 0.25f;
						}
						sp.color = tempColor;
				}
		}

		//set font size
		public void SetFontSize ()
		{
				if (fontSize == FontSize.SMALL) {
						if (fontSizeType == FontSizeType.PEN) {
								this.lineRenderWidth = 0.05f;
						} else {
								this.lineRenderWidth = 0.2f;
						}
				} else if (fontSize == FontSize.MEDIUM) {
						if (fontSizeType == FontSizeType.PEN) {
								this.lineRenderWidth = 0.2f;
						} else {
								this.lineRenderWidth = 0.4f;
						}
				} else if (fontSize == FontSize.LARGE) {
						if (fontSizeType == FontSizeType.PEN) {
								this.lineRenderWidth = 0.3f;
						} else {
								this.lineRenderWidth = 0.6f;
						}
				}
		}

		//Set the selected drawing tool
		public void SetSelectedTool ()
		{
				if (selectedTool == SelectedTool.DELETE) {
						deleteObFollowTarget.tragetIndex = 0;
						brushObFollowTarget.tragetIndex = 1;
						penObFollowTarget.tragetIndex = 1;
						deleteObFollowTarget.isRunning = true;
						brushObFollowTarget.isRunning = true;
						penObFollowTarget.isRunning = true;
				} else if (selectedTool == SelectedTool.BRUSH) {
						deleteObFollowTarget.tragetIndex = 1;
						brushObFollowTarget.tragetIndex = 0;
						penObFollowTarget.tragetIndex = 1;
						deleteObFollowTarget.isRunning = true;
						brushObFollowTarget.isRunning = true;
						penObFollowTarget.isRunning = true;
				} else if (selectedTool == SelectedTool.PEN) {
						deleteObFollowTarget.tragetIndex = 1;
						brushObFollowTarget.tragetIndex = 1;
						penObFollowTarget.tragetIndex = 0;
						deleteObFollowTarget.isRunning = true;
						brushObFollowTarget.isRunning = true;
						penObFollowTarget.isRunning = true;
				}
		}
	
		public enum FontSizeType
		{
				PEN,
				BRUSH
		}

		public enum SelectedTool
		{
				BRUSH,
				PEN,
				DELETE
		}

		public enum FontSize
		{
				SMALL,
				MEDIUM,
				LARGE
	}
		;
}