using UnityEngine;
using System.Collections;

namespace com.huldagames.TapDrive2D
{
	public class UserInputController : IInputController
	{
		Car car;

		public UserInputController (Car _car)
		{
			car = _car;
		}

		public void Handle ()
		{
			if (Input.GetKey (KeyCode.UpArrow)) {
				car.RigidBody2D.freezeRotation = false;
				foreach (var wheel in car.wheels) {
					if (wheel != null) {
						wheel.Move (WheelDirection.FORWARD);
					}
				}
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				car.RigidBody2D.freezeRotation = true;
				foreach (var wheel in car.wheels) {
					if (wheel != null) {
						wheel.Move (WheelDirection.BACKWARD);
					}
				}
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				foreach (var wheel in car.wheels) {
					if (wheel != null) {
						wheel.TurnLeft ();
					}
				}
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				foreach (var wheel in car.wheels) {
					if (wheel != null) {
						wheel.TurnRight ();
					}
				}
			}
		}

		public Vector3[] HandleScannerItem (CarScanner.CarScannerHitResult result)
		{
			return null;
		}
	}
}