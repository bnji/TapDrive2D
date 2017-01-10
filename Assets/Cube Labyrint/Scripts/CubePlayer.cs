using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class CubePlayer : MonoBehaviour
{

    public float movementForce = 1f;
    public Rigidbody2D rigidBody;
    public bool IsInitialized { get; private set; }

    bool isMoving = false;

    // Use this for initialization
    void Start()
    {
        IsInitialized = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void Initialize(Transform t)
    {
        if (!this.IsInitialized)
        {
            this.transform.parent = t;
            this.transform.position = t.position;
            this.IsInitialized = true;
        }
    }

    void OnSwipeUp()
    {
        if (isMoving) return;
        this.rigidBody.AddForce(Vector2.up * 1000f * movementForce * Time.deltaTime);
        isMoving = true;
    }

    void OnSwipeDown()
    {
        if (isMoving) return;
        this.rigidBody.AddForce(-1f * Vector2.up * 1000f * movementForce * Time.deltaTime);
        isMoving = true;
    }
    void OnSwipeLeft()
    {
        if (isMoving) return;
        this.rigidBody.AddForce(-1f * Vector2.right * 1000f * movementForce * Time.deltaTime);
        isMoving = true;
    }
    void OnSwipeRight()
    {
        if (isMoving) return;
        this.rigidBody.AddForce(Vector2.right * 1000f * movementForce * Time.deltaTime);
        isMoving = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.name.Equals("Center") && !collider.name.StartsWith("Cube Spawner"))
        {
            SceneManager.LoadScene(1);
            //StopMoving();
        }
        Debug.Log(collider.name);
    }

    void OnHitCubeCenter(Transform t)
    {
        StartCoroutine(StopMoving(t));
    }
    IEnumerator StopMoving(Transform t)
    {
        yield return new WaitForSeconds(0.05f);
        this.rigidBody.velocity = Vector2.zero;
        this.transform.parent = t;
        isMoving = false;
    }
}
