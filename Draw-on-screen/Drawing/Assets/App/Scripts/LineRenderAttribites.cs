using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Line render attribites.
/// </summary>
public class LineRenderAttribites : MonoBehaviour
{
		public Color color;//line color
		private int numberOfPoints;//number of points
		private List<Vector3> points;//line points

		void Start ()
		{
				points = new List<Vector3> ();
		}
	
		public int NumberOfPoints {
				get{ return this.numberOfPoints;}
				set{ this.numberOfPoints = value;}
		}

		public List<Vector3> Points {
				get{ return this.points;}
				set{ this.points = value;}
		}

}
