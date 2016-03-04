using UnityEngine;
using System.Collections;

namespace com.huldagames.TapDrive2D
{
	[System.Serializable]
	public class CarProperties
	{
		[Range (1f, 5f)]
		[SerializeField]
		public float weight = 1f;

		[Range (1f, 25f)]
		[SerializeField]
		public int obstaclePathSteps = 7;

		[Range (1f, 10f)]
		[SerializeField]
		public float obstacleMinDistanceBeforeDetection = 7f;

		[Range (1f, 150f)]
		[SerializeField]
		public float maxSpeed = 100f;

		[Range (0f, 150f)]
		[SerializeField]
		public float minSpeed = 0f;

		[SerializeField]
		public float acceleration = 100f;

		[SerializeField]
		[Range (0.1f, 1f)]
		public float sensitivityMult = 0.5f;

		[SerializeField]
		public bool isFourWheelDrive = false;
	}
}