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

		public void ProcessInput ()
		{
			if (Input.GetKey (KeyCode.UpArrow)) {
				car.RigidBody2D.freezeRotation = false;
				Drive (1f);
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				car.RigidBody2D.freezeRotation = true;
				Drive (-0.5f);
			} else {
				foreach (var wheel in car.wheels) {
					wheel.ResetWheelPosition (3f);	
				}
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				foreach (var wheel in car.wheels) {
					if (wheel != null) {
						wheel.TurnLeft (80f);
					}
				}
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				foreach (var wheel in car.wheels) {
					if (wheel != null) {
						wheel.TurnRight (80f);
					}
				}
			}
		}

		void Drive (float dir)
		{
			foreach (var wheel in car.wheels) {
				if (wheel != null) {
					if (car.Speed < car.Properties.maxSpeed) {
						car.Speed += car.Properties.acceleration * Time.deltaTime;
						car.Speed = car.Speed > car.Properties.maxSpeed ? car.Properties.maxSpeed : car.Speed;
					}
					var force = dir * wheel.transform.up * car.Speed * Time.deltaTime * 32f;
					if (car.Properties.isFourWheelDrive) {
						wheel.ApplyForce (force);
					} else {
						if (!wheel.isRearWheel) {
							wheel.ApplyForce (force);
						}
					}
				}
			}
		}

		public Vector3[] HandleScannerItem (CarScanner.HitResult result)
		{
			return null;
		}
	}
}