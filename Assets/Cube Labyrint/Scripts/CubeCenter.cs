using UnityEngine;
using System.Collections;

public class CubeCenter : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider)
    {
        collider.gameObject.SendMessage("OnHitCubeCenter", transform, SendMessageOptions.DontRequireReceiver);
    }
}
