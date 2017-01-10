using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

    public float speed = 0.1f;

	void Update () {
        this.transform.Translate(Vector3.up * Time.deltaTime * speed * -1f);
	}
}
