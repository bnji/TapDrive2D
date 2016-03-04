using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.huldagames.TapDrive2D
{
	public class WayPointHandler
	{
		private CarScanner _scanner;

		public CarScanner Scanner {
			get { return _scanner; }
			set { _scanner = value; }
		}

		// waypoints
		private Dictionary<int, Vector3> oldWaypoints = new Dictionary<int, Vector3> ();
		private Vector3[] _wayPoints;
		private Vector3 _nextWayPoint = Vector3.zero;
		private int nextWayPointIndex = -1;
		private int _lapsCompleted = 0;
		private int lastModifiedWaypointIndexStart = -1;

		public Vector3[] WayPoints {
			get { return _wayPoints; }
			set { _wayPoints = value; }
		}

		public Vector3 NextWayPoint {
			get { return _nextWayPoint; }
		}

		public int LapsCompleted {
			get { return _lapsCompleted; }
		}

		public WayPointHandler (CarScanner scanner)
		{
			_scanner = scanner;
		}

		int GetClosesetWayPointIndex (Vector3 position)
		{
			var index = nextWayPointIndex;
			var lowestDistanceRegistered = float.MaxValue;
			for (int i = 0; i < _wayPoints.Length; i++) {
				var wp = _wayPoints [i];
				var distance = Vector2.Distance (wp, position);
				if (distance < lowestDistanceRegistered) {
					lowestDistanceRegistered = distance;
					index = i;
				}
			}
			return index;
		}

		public bool SetNextWayPoint (Vector3 currentPosition, float sensitivityMult, bool drawLine = false)
		{
			var dist = Vector2.Distance (currentPosition, _nextWayPoint);
			if (_wayPoints.Length > 0 && (_nextWayPoint == Vector3.zero || dist <= sensitivityMult)) {

				if (_nextWayPoint == Vector3.zero) {
					nextWayPointIndex = GetClosesetWayPointIndex (currentPosition);
				}

				//				if (speed >= properties.maxSpeed) {
				//					nextWayPointIndex = nextWayPointIndex + 3;
				//				} else if (speed >= properties.maxSpeed / 2f) {
				//					nextWayPointIndex = nextWayPointIndex + 2;
				//				} else {
				//					nextWayPointIndex = nextWayPointIndex + 1;
				//				}
				nextWayPointIndex = nextWayPointIndex + 1;
				if (nextWayPointIndex == _wayPoints.Length) {
					_lapsCompleted++;
				}
				nextWayPointIndex = nextWayPointIndex % _wayPoints.Length;
				_nextWayPoint = _wayPoints [nextWayPointIndex];
				if (_scanner != null && !_scanner.IsActive) {
					//					Debug.Log ("nextWayPointIndex: " + nextWayPointIndex + ", oldwaypoints: " + oldWaypoints.Count + ", lastModifiedWaypointIndexStart: " + lastModifiedWaypointIndexStart);
					if (nextWayPointIndex >= oldWaypoints.Count + lastModifiedWaypointIndexStart) {
						foreach (KeyValuePair<int, Vector3> kvp in oldWaypoints) {
							_wayPoints [kvp.Key] = kvp.Value;
						}
						Debug.Log ("Scanner is active again");
						_scanner.IsActive = true;
					}
				}
				//				Debug.Log ("next waypoint index: " + nextWayPointIndex + " - point: " + nextWayPoint);
				return true;
			}
			if (drawLine) {
				Debug.DrawLine (currentPosition, _nextWayPoint, Color.red);
			}
			return false;
		}

		Vector3 ManipulateWayPoint (int index, Transform t)
		{
			var pos = t.position;
			var rot = t.rotation.eulerAngles.z;
			var tempWP = _wayPoints [index];
			rot = rot <= 0 || rot >= 360f ? 180f : rot;
			var rotMult = rot / 180f;
			var mult = 0.5f;
			var xt = 1 - Mathf.Abs (rotMult);
			var yt = 1 - xt;
			var rotMultSign = Mathf.Sign (rotMult);
			xt = xt * mult * rotMultSign;
			yt = yt * mult * rotMultSign;
			//			Debug.Log ("xt: " + xt + ", yt: " + yt + ", rotmult: " + rotMult + ", rot: " + rot);
			_wayPoints [index] = new Vector3 (tempWP.x + xt, tempWP.y + yt, _nextWayPoint.z);
			//			Debug.Log ("WayPoint changed from: " + tempWP + " to " + wayPoints [index]);
			return _wayPoints [index];
		}

		public Vector3[] ManipulateWayPoints (int index, Transform t, int amount)
		{
			index = nextWayPointIndex + index;
			Vector3[] tempWaypoints = new Vector3[amount];
			oldWaypoints.Clear ();
			lastModifiedWaypointIndexStart = index;
			for (int i = 0; i < amount; i++) {
				var newIndex = (index + i) % _wayPoints.Length;
//				Debug.Log ("newIndex: " + newIndex + " - " + index);
				if (!oldWaypoints.ContainsKey (newIndex)) {
					oldWaypoints.Add (newIndex, _wayPoints [newIndex]);
					tempWaypoints [i] = ManipulateWayPoint (newIndex, t);
				}
			}
			return tempWaypoints;
		}

	}
}