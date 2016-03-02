using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.huldagames.TapDrive2D
{
	public class RaceTrackPlotter : MonoBehaviour
	{
		public float plotInterval = 200;
		public Object checkpoint;

		private float lastTimeCreatedWayPoint = 0f;
		private List<Vector3> tempWayPoints;

		void Awake ()
		{
			tempWayPoints = new List<Vector3> ();
		}

		void Update ()
		{
			SetupWayPoints ();
		}

		public Vector3[] SetupWayPoints ()
		{
			Vector3[] result = null;
			if (tempWayPoints != null) {
				if (tempWayPoints.Count > 0 && (Input.GetMouseButtonDown (1) || (Input.touchCount >= 1 && Input.touchCount >= 1 && Input.GetTouch (0).phase == TouchPhase.Ended))) {
					result = tempWayPoints.ToArray ();// Helpers.MakeSmoothCurve (tempWayPoints, 5f);
					tempWayPoints = null;
					//					foreach (var waypoint in GameObject.FindObjectsOfType<WayPoint>()) {
					//						waypoint.GetComponent<SpriteRenderer> ().enabled = false;
					//					}
				}
				if (Input.GetMouseButton (0) || (Input.touchCount >= 1 && (Input.GetTouch (0).phase == TouchPhase.Began || Input.GetTouch (0).phase == TouchPhase.Moved))) {
					if ((Time.time - lastTimeCreatedWayPoint) * 1000f >= plotInterval) {
						var pos = Input.mousePosition;
						pos.z = 10;
						pos = Camera.main.ScreenToWorldPoint (pos);
						if (!tempWayPoints.Contains (pos)) {
							Instantiate (checkpoint, pos, Quaternion.identity);
							tempWayPoints.Add (pos);
							lastTimeCreatedWayPoint = Time.time;
						}
					}
				}
			}
			return result;
		}

		//		// plotter
		//		public Object checkpoint;
		//		//		float lastTimeCreatedWayPoint = 0f;
		//		public List<Vector3> wayPoints;
		//
		//		public void Create ()
		//		{
		//			Debug.Log (Input.mousePosition);
		//			//				if (Time.time - lastTimeCreatedWayPoint >= 0.2f) {
		//			var pos = Input.mousePosition;
		//			pos.z = 10;
		//			pos = Camera.main.ScreenToWorldPoint (pos);
		//			if (!wayPoints.Contains (pos)) {
		//				Instantiate (checkpoint, pos, Quaternion.identity);
		//				wayPoints.Add (pos);
		////				lastTimeCreatedWayPoint = Time.time;
		//			}
		//			//				}
		//		}
	}
}