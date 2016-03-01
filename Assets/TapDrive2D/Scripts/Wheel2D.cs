using UnityEngine;
using System.Collections;

namespace TapDrive2D.Vehicles.Car
{
	public enum WheelSide
	{
		LEFT,
		RIGHT
	}

	[RequireComponent (typeof(Rigidbody2D))]
	[RequireComponent (typeof(HingeJoint2D))]
	public class Wheel2D : MonoBehaviour
	{
		public bool isRearWheel = false;
		public bool isActive = true;
		public WheelSide WheelSide;

		Rigidbody2D rb;

		public void Drive (Vector3 force)
		{
			if (isActive) {
				if (isRearWheel) {
					return;
				}
				rb.AddForce (force);
			}
		}

		// Use this for initialization
		void Start ()
		{
			rb = GetComponent<Rigidbody2D> ();
		}
	}
}