using EasyAR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour {


    private GameObject[,] mat = new GameObject[1, 1];
    private int[,] directionmat = new int[1, 1];
    private GameObject targetAtual;
    public int targetsFound = 0;
    Dictionary<string, Texture> guiTextures = new Dictionary<string, Texture>();

    public int carregados = 0;
    public int numeroDeTiles = 0;

    public GameObject painel;
    public GameObject texto;

    public void AdicionaTile(GameObject target, int linha, int coluna, int direction)
    {
        //target = target a ser adicionado
        //linha = numero de linhas a ser adicionado a partir do

        if (targetsFound == 0)
        {
            //Se não há tiles encontrados, então é o primeiro tile que vai ser colocado
            mat[0, 0] = target;
            directionmat[0, 0] = direction;
            target.GetComponent<ImageTargetBehaviour>().SetLastPosition(0, 0);
            Debug.Log("Tamanho da matriz = " + mat.GetLength(0) + "x" + mat.GetLength(1));
        }
        else
        {
            //Se não, verifica se tem espaço pro novo tile no mapa atual

            if (linha > mat.GetLength(0) - 1)
            {
                //Se a posicao do novo tile passa o numero de linhas
                //Aumenta o número de linhas                
                mat = ResizeArray(mat, linha + 1, mat.GetLength(1));
                directionmat = ResizeArray(directionmat, linha + 1, directionmat.GetLength(1));

                Debug.Log("Aumentou pra direita");
            }
            else if (linha < 0)
            {
                //Se a posicao do novo tile passa o numero de linhas em posicao negativa
                //Aumenta o número de linhas e move os tiles para a direita
                mat = ResizeArray(mat, mat.GetLength(0) + Math.Abs(linha), mat.GetLength(1));
                directionmat = ResizeArray(directionmat, directionmat.GetLength(0) + Math.Abs(linha), directionmat.GetLength(1));
                Descer_matriz(Math.Abs(linha));
                Descer_matrizD(Math.Abs(linha));
            }

            if (coluna > mat.GetLength(1) - 1)
            {
                //Se a posicao do novo tile passa o numero de colunas
                //Aumenta o número de colunas                
                mat = ResizeArray(mat, mat.GetLength(0), coluna + 1);
                directionmat = ResizeArray(directionmat, directionmat.GetLength(0), coluna + 1);
                Debug.Log("Aumentou pra baixo");
            }
            else if (coluna < 0)
            {
                //Se a posicao do novo tile passa o numero de colunas em posicao negativa
                //Aumenta o número de colunas e move os tiles para a baixo                
                mat = ResizeArray(mat, mat.GetLength(0), mat.GetLength(1) + Math.Abs(coluna));
                directionmat = ResizeArray(directionmat, directionmat.GetLength(0), directionmat.GetLength(1) + Math.Abs(coluna));
                Debug.Log("Aumentou matriz para: " + mat.GetLength(0) + "x" + mat.GetLength(1));
                Direita_matriz(Math.Abs(coluna));
                Direita_matrizD(Math.Abs(coluna));
                Debug.Log("Aumentou pra " + Math.Abs(coluna) + " cima e jogou pra baixo");
            }

            //adiciona o tile na posição da matriz determinada
            mat[Math.Max(0,linha), Math.Max(0,coluna)] = target;
            directionmat[Math.Max(0, linha), Math.Max(0, coluna)] = direction;
            target.GetComponent<ImageTargetBehaviour>().SetLastPosition(Math.Max(0, linha), Math.Max(0, coluna));
        }
        targetAtual = target;
        Debug.Log("Tile "+target.name+" atualizado em " + linha + "x"+ coluna);
    }

    void Update()
    {
        
        if(numeroDeTiles > carregados)
        {
            texto.GetComponent<Text>().text = "Carregando \n"+carregados * 100 / numeroDeTiles+"%";
        }
        else if(painel.activeSelf)
        {
            painel.SetActive(false);
            StaticCoroutine.Start(StateMachine.PerformCoroutine());
        }
    }

    protected T[,] ResizeArray<T>(T[,] original, int rows, int cols)
    {
        var newArray = new T[rows, cols];
        int minRows = Math.Min(rows, original.GetLength(0));
        int minCols = Math.Min(cols, original.GetLength(1));
        for (int i = 0; i < minRows; i++)
            for (int j = 0; j < minCols; j++)
                newArray[i, j] = original[i, j];
        return newArray;
    }

    void Descer_matriz(int num)
    {
        for (int l = mat.GetLength(0) - 1; l > 0; l--)
        {
            for (int c = mat.GetLength(1) - 1; c >= 0; c--)
            {
                mat[l, c] = mat[l - num, c];
                if(mat[l - num, c]!=null)
                {
                    int[] position = {l - num,c};
                    if(mat[l - num, c].GetComponent<ImageTargetBehaviour>().getLastPosition().SequenceEqual(position))
                    {
                        mat[l - num, c].GetComponent<ImageTargetBehaviour>().SetLastPosition(l, c);
                    }
                }
            }
        }
        for (int c = mat.GetLength(1) - 1; c >= 0; c--)
        {
            mat[0, c] = null;
        }

    }

    void Direita_matriz(int num)
    {
        for (int l = mat.GetLength(0) - 1; l >= 0; l--)
        {
            for (int c = mat.GetLength(1) - 1; c > 0; c--)
            {
                mat[l, c] = mat[l, c - num];
                if (mat[l, c - num] != null)
                {
                    int[] position = { l, c - num };
                    if (mat[l, c - num].GetComponent<ImageTargetBehaviour>().getLastPosition().SequenceEqual(position))
                    {
                        mat[l, c - num].GetComponent<ImageTargetBehaviour>().SetLastPosition(l, c);
                    }
                }
            }
        }
        for (int l = mat.GetLength(0) - 1; l >= 0; l--)
        {
            mat[l, 0] = null;
        }

    }

    void Descer_matrizD(int num)
    {
        for (int l = directionmat.GetLength(0) - 1; l > 0; l--)
        {
            for (int c = directionmat.GetLength(1) - 1; c >= 0; c--)
            {
                directionmat[l, c] = directionmat[l - num, c];
            }
        }
        for (int c = directionmat.GetLength(1) - 1; c >= 0; c--)
        {
            directionmat[0, c] = globalMap.Z_POSITIVO;
        }

    }

    void Direita_matrizD(int num)
    {
        for (int l = directionmat.GetLength(0) - 1; l >= 0; l--)
        {
            for (int c = directionmat.GetLength(1) - 1; c > 0; c--)
            {
                directionmat[l, c] = directionmat[l, c - num];
            }
        }
        for (int l = mat.GetLength(0) - 1; l >= 0; l--)
        {
            directionmat[l, 0] = globalMap.Z_POSITIVO;
        }

    }

    public int[] GetActiveTargetPosition(GameObject target)
    {
        int[] position = { 0, 0 };
        for (int c = 0; c < mat.GetLength(0); c++)
        {
            for (int l = 0; l < mat.GetLength(1); l++)
            {
                if(mat[c,l] != null)
                {
                    if (mat[c,l] != target && mat[c, l].activeSelf)
                    {
                        position = mat[c, l].GetComponent<ImageTargetBehaviour>().getLastPosition();
                        Debug.Log("Retornando target " + mat[c, l].name + " na posicao: " + position[0] +"x"+position[1]);
                        return position;
                    }
                }                
            }
        }        
        return position;
    }

    public GameObject getTileTarget(int linha, int coluna)
    {
        return mat[linha, coluna];
    }

    public void Resetar_Mapa()
    {
        mat = new GameObject[1, 1];
    }

    void OnGUI()
    {
        for (int i = 0; i < mat.GetLength(0); i++)
        {
            for (int ii = 0; ii < mat.GetLength(1); ii++)
            {
                
                if (mat[i, ii] != null)
                {
                    ImageTargetBehaviour target = mat[i, ii].GetComponent<ImageTargetBehaviour>();
                    try
                    {
                        GUI.Box(new Rect(ii * 50, i * 50, 50, 50), guiTextures[target.Path]);
                    }
                    catch(KeyNotFoundException)
                    {
                        Debug.Log("textura nao encontrada");
                        GUI.Box(new Rect(ii * 50, i * 50, 50, 50), "");
                        GUI.Label(new Rect(ii * 50, i * 50, 50, 50), target.Name);
                    }
                    continue;
                }
                GUI.Box(new Rect(ii * 50, i * 50, 50, 50), "");
                GUI.Label(new Rect(ii * 50, i * 50, 50, 50), "VAZIO");
            }
        }
    }

    public void AddMapTexture(String path)
    {
        StartCoroutine(LoadSprite(path));
    }

    public IEnumerator LoadSprite(string absoluteImagePath)
    {
        string finalPath;
        WWW localFile;

        finalPath = Path.Combine(Application.streamingAssetsPath, absoluteImagePath);
        localFile = new WWW(finalPath);

        yield return localFile;
        guiTextures.Add(absoluteImagePath,localFile.texture);
    }

    public void SaveMapLoadScene()
    {
        int[,] map = new int[mat.GetLength(0), mat.GetLength(1)];

        for(int i = 0; i<map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if(mat[i, j]!=null)
                {
                    map[i, j] = mat[i, j].GetComponent<ImageTargetBehaviour>().tileID;
                }
                else
                {
                    map[i, j] = 0;
                }
                
            }
        }

        globalMap.Mapa = map;
        globalMap.MapaDirecao = directionmat;
        SceneManager.LoadScene("mapRenderer");
    }

    public void voltarAoMenuPrincipal()
    {
        SceneManager.LoadScene("mainmenu");
    }
}
