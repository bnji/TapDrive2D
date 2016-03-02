using UnityEngine;
using System.Collections;

namespace com.huldagames.TapDrive2D
{
	public class AudioHandler : MonoBehaviour
	{
		public AudioClip audioDriving;
		public AudioClip audioAcceleration;
		public AudioClip audioDeceleration;

		private AudioSource audioSource;

		public void HandleAcceleration (float speed)
		{

			if (!audioSource.isPlaying) {
				if (speed == 0f) {
					audioSource.clip = audioAcceleration;
					audioSource.Play ();
				} else {
					audioSource.clip = audioDriving;
					audioSource.Play ();
				}
			}
		}

		public void HandleDeceleration (float speed)
		{
			if (audioSource.clip != audioDeceleration && speed > 0) {
				audioSource.clip = audioDeceleration;
				audioSource.Play ();
			}
		}

		void Start ()
		{
			audioSource = GetComponent<AudioSource> ();
		}
	}
}