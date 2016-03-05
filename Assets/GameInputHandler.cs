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
			if (Input.GetKey (KeyCode.R) || (Input.touchCount >= 1 && Input.GetTouch (0).tapCount >= 2)) {
				GameManager.Instance.ReloadScene (0.5f);
			}
		}
	}
}