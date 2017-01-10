using UnityEngine;
using System.Collections;

public class CubeHandler : MonoBehaviour {

    //public Color[] colors;
    //public Sprite[] sprites;
    //public TextHandler textHandler;
    //public Rotate rotateHandler;

    public CubePlayer player;
    public Transform playerSpawnPosition;
    public Object[] cubePrefabs;

    void Start()
    {
        var cubeGameObjects = GameObject.FindGameObjectsWithTag("CubeSpawner");
        foreach (var go in cubeGameObjects)
        {
            var cubeSpawner = go.GetComponent<CubeSpawner>();
            if(cubeSpawner != null)
            {
                var pos = cubeSpawner.transform.position;
                var cube = (GameObject)Instantiate(cubePrefabs[Random.Range(0, cubePrefabs.Length)], pos, Quaternion.identity);
                cube.transform.parent = cubeSpawner.transform;
                cubeSpawner.enabled = false;
                //Destroy(cubeSpawner.gameObject);
            }
        }
        player.Initialize(playerSpawnPosition);
        //this.cube = gameObject.GetComponentInChildren<Cube>();
        //if(this.cube != null)
        //{
        //    var sr = this.cube.GetComponent<SpriteRenderer>();
        //    sr.sprite = this.sprites[Random.Range(0, sprites.Length)];
        //    sr.color = this.colors[Random.Range(0, colors.Length)];
        //}
        //if(this.rotateHandler != null)
        //{
        //    this.rotateHandler.rotationInterval = Random.Range(1, 5);
        //    this.rotateHandler.rotationDelta = Random.Range(90, 180);
        //}
    }
}
