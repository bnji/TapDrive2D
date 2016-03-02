using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TapDrive2D.Vehicles.Car
{
	public class RaceTrackPlotter : MonoBehaviour
	{
		// plotter
		public Object checkpoint;
		//		float lastTimeCreatedWayPoint = 0f;
		public List<Vector3> wayPoints;

		public void Create ()
		{
			Debug.Log (Input.mousePosition);
			//				if (Time.time - lastTimeCreatedWayPoint >= 0.2f) {
			var pos = Input.mousePosition;
			pos.z = 10;
			pos = Camera.main.ScreenToWorldPoint (pos);
			if (!wayPoints.Contains (pos)) {
				Instantiate (checkpoint, pos, Quaternion.identity);
				wayPoints.Add (pos);
//				lastTimeCreatedWayPoint = Time.time;
			}
			//				}
		}
	}
}