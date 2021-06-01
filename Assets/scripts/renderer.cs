using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapRenderer : MonoBehaviour {

    public int[,] myArray = {
                                { 1, 0, 1 },
                                { 1, 0, 1 },
                                { 1, 3, 1 },
                                { 1, 0, 1 },
                                { 1, 0, 1 },
                            };

    public GameObject[] tiles;

    public float mapSizeX;
    public float mapSizeY;
    public float tileSize;

    // Use this for initialization
    void Start () {
        tileSize = tiles[0].GetComponent<Renderer>().bounds.size.x;
        mapSizeX = tileSize * myArray.GetLength(0);
        mapSizeY = tileSize * myArray.GetLength(1);
        for (int i=0; i<myArray.GetLength(0); i++)
        {
            for (int j = 0; j < myArray.GetLength(1); j++)
            {
                GameObject tile = Instantiate<GameObject>(tiles[myArray[i, j]]);                
                tile.transform.position = new Vector3(tileSize * i - (mapSizeX/2) + (tileSize/2), 0, tileSize * j - (mapSizeY / 2) + (tileSize / 2));
                tile.transform.parent = this.transform;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
