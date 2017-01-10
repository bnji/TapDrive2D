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
    Transform cubeCenter = null;
    
    void Start()
    {
        IsInitialized = false;
    }

    float minDist = 100f;
    void FixedUpdate()
    {
        if(cubeCenter != null)
        {
            var dist = Mathf.Abs(Vector2.Distance(cubeCenter.position, this.transform.position));
            //Debug.Log(dist);
            if(dist < minDist)
            {
                minDist = dist;
                Debug.Log(minDist);
            }
            if(dist <= 0.01f)
            {
                this.rigidBody.velocity = Vector2.zero;
                this.transform.parent = cubeCenter;
                isMoving = false;
                cubeCenter = null;
            }
        }
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
        //Debug.Log(collider.name);
    }

    void OnHitCubeCenter(Transform t)
    {
        this.cubeCenter = t;
        //StartCoroutine(StopMoving(t));
    }
    IEnumerator StopMoving(Transform t)
    {
        yield return new WaitForSeconds(0.05f);
        this.rigidBody.velocity = Vector2.zero;
        this.transform.parent = t;
        isMoving = false;
    }
}
