using UnityEngine;
using System.Collections;

namespace com.huldagames.TapDrive2D
{
	public interface IInputController
	{
		void ProcessInput ();

		void LoadWayPoints (Vector3[] wayPoints);

		Vector3[] HandleScannerItem (CarScanner.HitResult result);
	}
}