using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{

    public int[,] localmap = {
                                { 4, 2, 3 },
                                { 4, 2, 5 },
                                { 11, 6, 4 },
                                { 4, 2, 9 },
                                { 12, 0, 3 },
                            };
    public int[,] directionlocalmap = {
                                        { globalMap.Z_POSITIVO, globalMap.Z_POSITIVO, globalMap.Z_POSITIVO },
                                        { globalMap.Z_POSITIVO, globalMap.Z_POSITIVO, globalMap.Z_POSITIVO },
                                        { globalMap.Z_POSITIVO, globalMap.Z_NEGATIVO, globalMap.Z_POSITIVO },
                                        { globalMap.Z_POSITIVO, globalMap.Z_POSITIVO, globalMap.X_NEGATIVO },
                                        { globalMap.Z_POSITIVO, globalMap.Z_NEGATIVO, globalMap.Z_POSITIVO },
                                    };

    public GameObject[] tiles;
    public int[] collisionTypes;

    public float mapSizeX;
    public float mapSizeY;
    public float tileSize;

    public GameObject[,] tileMap;
    public GameObject personagem;
    
    public void RenderMap()
    {
        if (globalMap.Mapa != null)
        {
            localmap = globalMap.Mapa;
        }
        if (globalMap.MapaDirecao != null)
        {
            directionlocalmap = globalMap.MapaDirecao;
        }
        else
        {
            globalMap.MapaDirecao = directionlocalmap;
        }
        tileMap = new GameObject[localmap.GetLength(0), localmap.GetLength(1)];

        tileSize = tiles[tiles.Length - 1].GetComponent<Renderer>().bounds.size.x;
        mapSizeY = tileSize * localmap.GetLength(0);
        mapSizeX = tileSize * localmap.GetLength(1);
        for (int i = 0; i < localmap.GetLength(0); i++)
        {
            for (int j = 0; j < localmap.GetLength(1); j++)
            {
                GameObject tile;

                if (localmap[i, j] == 0 || localmap[i, j] == 1)
                {
                    personagem = Instantiate<GameObject>(tiles[localmap[i, j]]);
                    personagem.transform.parent = this.transform;
                    tile = Instantiate<GameObject>(tiles[2]);
                }
                else
                {
                    tile = Instantiate<GameObject>(tiles[localmap[i, j]]);
                }

                tile.transform.position = new Vector3(tileSize * j - (mapSizeX / 2) + (tileSize / 2), 0, (tileSize * i - (mapSizeY / 2) + (tileSize / 2)) * -1);
                tile.transform.parent = this.transform;
                SetObjectDirection(tile, globalMap.MapaDirecao[i, j]);
                int[] position = { i, j };
                tile.GetComponent<TileData>().position = position;
                tile.GetComponent<TileData>().colisionType = collisionTypes[localmap[i, j]];
                tileMap[i, j] = tile;
            }
        }
        if(personagem == null)
        {
            personagem = Instantiate<GameObject>(tiles[0]);
            personagem.transform.parent = this.transform;
        }
    }

    public static void SetObjectDirection(GameObject obj, int direction)
    {
        switch(direction)
        {
            case globalMap.X_POSITIVO:
                obj.transform.eulerAngles = new Vector3(0, 90, 0);
                break;

            case globalMap.X_NEGATIVO:
                obj.transform.eulerAngles = new Vector3(0, 270, 0);
                break;

            case globalMap.Z_POSITIVO:
                obj.transform.eulerAngles = new Vector3(0, 0, 0);
                break;

            case globalMap.Z_NEGATIVO:
                obj.transform.eulerAngles = new Vector3(0, 180, 0);
                break;
        }
    }

}