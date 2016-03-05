using UnityEngine;
using System.Collections;

namespace com.huldagames.TapDrive2D
{
	public class FollowMouseInputController : IInputController
	{
		private Car car;

		public FollowMouseInputController (Car _car)
		{
			car = _car;
		}

		public void ProcessInput ()
		{
			Drive (GetTargetPosition ());
		}

		Vector3 GetTargetPosition ()
		{
			var position = Input.mousePosition;
			if (Input.touchCount == 1) {
				position = Input.GetTouch (0).position;
			}
			position = Camera.main.ScreenToWorldPoint (position);
			position.z = car.transform.position.z;
			return position;
		}

		void Drive (Vector3 position)
		{
			if (Vector2.Dot (car.transform.up.normalized, (position - car.transform.position).normalized) <= 0f) {
				return;
			}
			foreach (var wheel in car.wheels) {
				if (wheel != null) {
					var force = (position - car.transform.position).normalized * car.Speed * Time.deltaTime * 32f;
					if (car.Properties.isFourWheelDrive) {
						wheel.ApplyForce (force);
					} else {
						if (!wheel.isRearWheel) {
							wheel.ApplyForce (force);
						}
					}
				}
			}
			car.SendMessage ("OnMovingForward", SendMessageOptions.DontRequireReceiver);
		}

		public void LoadWayPoints (Vector3[] wayPoints)
		{

		}

		public Vector3[] HandleScannerItem (CarScanner.HitResult result)
		{
			return null;
		}
	}
}