using UnityEngine;
using System.Collections;

namespace com.huldagames.TapDrive2D
{
	[System.Serializable]
	public class WheelProperties
	{
		[Range (1f, 90f)]
		[SerializeField]
		public float maxTurnAngle = 30f;

		[SerializeField]
		public float friction = 5f;
	}
}