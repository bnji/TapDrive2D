using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Helpers
{


	public class Curver : MonoBehaviour
	{
		//arrayToCurve is original Vector3 array, smoothness is the number of interpolations.
		public static Vector3[] MakeSmoothCurve (List<Vector3> points, float smoothness)
		{
			List<Vector3> curvedPoints;
			int pointsLength = 0;
			int curvedLength = 0;
			smoothness = smoothness >= 1f ? smoothness : 1f;
			pointsLength = points.Count;
			curvedLength = (pointsLength * Mathf.RoundToInt (smoothness)) - 1;
//			Debug.Log ("curvedLength: " + curvedLength);
			curvedPoints = new List<Vector3> (curvedLength);
			float t = 0.0f;
			for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++) {
				t = Mathf.InverseLerp (0, curvedLength, pointInTimeOnCurve);
				for (int j = pointsLength - 1; j > 0; j--) {
					for (int i = 0; i < j; i++) {
						points [i] = (1 - t) * points [i] + t * points [i + 1];
					}
				}
				curvedPoints.Add (points [0]);
			}
			return(curvedPoints.ToArray ());
		}
	}
}
