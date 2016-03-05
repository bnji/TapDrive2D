using UnityEngine;
using System.Collections;

namespace com.huldagames.TapDrive2D
{
	public class GameInputHandler : MonoBehaviour
	{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (Input.GetKeyDown (KeyCode.R) || (Input.touchCount >= 1 && Input.GetTouch (0).tapCount >= 2)) {
				GameManager.Instance.ReloadScene (0.5f);
			}

			if (Input.GetKeyDown (KeyCode.L)) {// || (Input.touchCount >= 1 && Input.GetTouch (0).tapCount >= 2)) {
				var wayPoints = GameManager.LoadWayPoints ();
				var car = GameObject.FindObjectOfType<Car> ();
				if (car != null && wayPoints != null) {
					car.InputController.LoadWayPoints (wayPoints);
					Debug.Log (wayPoints.Length + " waypoints have been loaded!");
				} else {
					Debug.Log ("Could not load waypoints from file!");
				}
			}
		}
	}
}