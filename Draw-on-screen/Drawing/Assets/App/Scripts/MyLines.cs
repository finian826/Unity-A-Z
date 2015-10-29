using UnityEngine;
using System.Collections;

/// <summary>
/// My lines.
/// Contains a set of attributes for each lines group
/// also it contains a set of attributes for Camera which contains the drawing shape(Shape).
/// </summary>
public class MyLines : MonoBehaviour
{
		public float maxLineZ = -33;//max line render z-position
		public float currentLineZ = 0;//current line render z-position
		public float farClipPlane = 40;//camera far clip plane
		public float cameraZPostition = -39;// camera z-position
}