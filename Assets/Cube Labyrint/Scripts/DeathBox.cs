using UnityEngine;
using System.Collections;

public class DeathBox : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider)
    {
        collider.gameObject.SendMessageUpwards("OnEnterDeathBox", this, SendMessageOptions.DontRequireReceiver);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        collider.gameObject.SendMessageUpwards("OnExitDeathBox", this, SendMessageOptions.DontRequireReceiver);
    }
}
