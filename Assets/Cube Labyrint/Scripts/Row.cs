using UnityEngine;
using System.Collections;

public class Row : MonoBehaviour {

    bool canMoveDown = false;
    float speed = 1f;
    float ontranslateY = 0f;
    float translatedY = 0f;

    void Update()
    {
        if (canMoveDown)
        {
            speed += 0.1f * Time.deltaTime;
            var v = -1f * Vector3.up * Time.deltaTime * 0.1f * speed;
            this.transform.Translate(v);
            this.translatedY = this.transform.position.y;
        }
    }
	void OnEnterDeathBox(DeathBox box)
    {
        ontranslateY = this.transform.position.y;
        canMoveDown = true;
    }
    
    void OnExitDeathBox(DeathBox box)
    {
        canMoveDown = false;
        StartCoroutine(MoveUp());
    }

    IEnumerator MoveUp()
    {
        yield return new WaitForSeconds(0.1f);
        var pos = transform.position;
        pos.y = 3.1f + (ontranslateY - translatedY);
        //transform.position = pos;
        transform.position = new Vector3(0f, 4.65f, 0f);
    }
}
