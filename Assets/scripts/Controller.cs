using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

    const int FRENTE    =  1;
    const int TRAS      =  -1;
    const int ESQUERDA  =  2;
    const int DIREITA   =  3;

    public GameObject map;
    public GameObject personagem;

    public float mapSizeX;
    public float mapSizeY;
    public float tileSize;

    int movimentoAtual = 0;
    bool executando = false;

    NavMeshAgent agent;
    GameObject[,] tileMap;

    float direcaoAtual;

    List<int> listaComandos = new List<int> {};

    public GameObject startButton;
    public GameObject stopButton;

    public int collisionTicks = 100;
    private int collisionCounter = 0;

    bool started = false;

    public float oldscale = 0f;
    // Use this for initialization
    void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        MapRenderer renderer = map.GetComponent<MapRenderer>();
        renderer.RenderMap();

        mapSizeX = renderer.mapSizeX;
        mapSizeY = renderer.mapSizeY;
        tileSize = renderer.tileSize;
        tileMap  = renderer.tileMap;
        personagem = renderer.personagem;
        agent = personagem.GetComponent<NavMeshAgent>();
        personagem.SetActive(true);      
        AddCharToMap();

        started = true;
    }

    public void voltarAoMenuPrincipal()
    {
        SceneManager.LoadScene("mainmenu");
    }

    void AddCharToMap()
    {
        int[,] newmap = map.GetComponent<MapRenderer>().localmap;

        for(int i = 0; i<newmap.GetLength(0); i++)
        {
            for (int j = 0; j < newmap.GetLength(1); j++)
            {
                if(newmap[i,j] == 0 || newmap[i, j] == 1)
                {
                    SetCharPosition(i, j);
                    MapRenderer.SetObjectDirection(personagem, globalMap.MapaDirecao[i, j]);
                    SetCharDirection(-1);
                    return;
                }                
            }
        }
        SetCharPosition(newmap.GetLength(0)/2, newmap.GetLength(1)/2);
        return;
    }

    void SetCharDirection(float direcao)
    {
        if(direcao>0)
        {
            personagem.transform.eulerAngles = new Vector3(0, direcao, 0);
        }        
        direcaoAtual = personagem.transform.rotation.eulerAngles.y % 360;
    }

    void SetCharPosition(int linha, int coluna)
    {
        personagem.transform.position = new Vector3(tileSize * coluna - (mapSizeX / 2) + (tileSize / 2), 0, (tileSize * linha - (mapSizeY / 2) + (tileSize / 2))*-1);
        personagem.GetComponent<Movement>().linha = linha;
        personagem.GetComponent<Movement>().coluna = coluna;
        agent.SetDestination(personagem.transform.position);
    }

    void SetaNovaPosicao(int direcao)
    {
        Debug.Log(personagem.transform.forward.x +" " + personagem.transform.forward.z);
        if (personagem.transform.forward.x < -0.5)
        {
            //apontando pra X negativo            
            personagem.GetComponent<Movement>().coluna += -1 * direcao;
            agent.SetDestination(personagem.transform.position + new Vector3(direcao * -tileSize, 0, 0));
        }
        else if (personagem.transform.forward.x > 0.5)
        {
            //apontando pra X positivo            
            personagem.GetComponent<Movement>().coluna += 1 * direcao;
            agent.SetDestination(personagem.transform.position + new Vector3(direcao * tileSize, 0, 0));
        }
        else if (personagem.transform.forward.z < -0.5)
        {
            //apontando pra Z negativo            
            personagem.GetComponent<Movement>().linha += 1 * direcao;
            agent.SetDestination(personagem.transform.position + new Vector3(0, 0, direcao * -tileSize));
        }
        else if (personagem.transform.forward.z > 0.5)
        {
            //apontando pra Z positivo            
            personagem.GetComponent<Movement>().linha += -1 * direcao;
            agent.SetDestination(personagem.transform.position + new Vector3(0, 0, direcao * tileSize));
        }
        
    }

    void SetaNovaDirecao(int direcao)
    {
        switch(direcao)
        {
            case DIREITA:
                direcaoAtual = (personagem.transform.rotation.eulerAngles.y%360 + 90)%360;
                break;

            case ESQUERDA:
                direcaoAtual = (personagem.transform.rotation.eulerAngles.y%360 - 90 + 360)% 360;
                break;
            
            default:
                Debug.Log("THIS SHOULD NOT EVER HAPPEN MAN");
                break;
        }
    }

    private bool NaoVirouCompletamente(int direcao)
    {
        if (personagem.GetComponent<Movement>().rotatedAngle>=90)
        {
            personagem.GetComponent<Movement>().CorrigirMovimento();
            personagem.GetComponent<Movement>().rotatedAngle = 0;
            return false;
        }

        if(direcao == FRENTE || direcao == TRAS)
        {
            return false;
        }
        return true;
    }

    public void Executar()
    {
        startButton.SetActive(false);
        stopButton.SetActive(true);
        StartCoroutine(ExecutaChar(0));
    }

    public void Parar()
    {
        startButton.SetActive(true);
        stopButton.SetActive(false);
        movimentoAtual = listaComandos.Count;
    }

    private bool Colidiu(int direcao)
    {
        int charC = personagem.GetComponent<Movement>().coluna;
        int charL = personagem.GetComponent<Movement>().linha;

        if (personagem.transform.forward.x < -0.5)
        {
            //apontando pra X negativo
            charC += (-1 * direcao);
        }
        else if (personagem.transform.forward.x > 0.5)
        {
            //apontando pra X positivo
            charC += (1 * direcao);
        }
        else if (personagem.transform.forward.z < -0.5)
        {
            //apontando pra Z negativo
            charL += (1 * direcao);
        }
        else if (personagem.transform.forward.z > 0.5)
        {
            //apontando pra Z positivo
            charL += (-1 * direcao);
        }

        if(charC >= tileMap.GetLength(1) || charL >= tileMap.GetLength(0) || charC < 0 || charL < 0)
        {
            return true;
        }

        if (tileMap[charL,charC].GetComponent<TileData>().colisionType == 2)
        {
            return true;
        }

        return false;
    }

    private void OnDisable()
    {
        if(personagem!=null)
        {
            personagem.GetComponent<Movement>().parar();
            SetCharDirection(direcaoAtual);
            SetCharPosition(personagem.GetComponent<Movement>().linha, personagem.GetComponent<Movement>().coluna);
            
            movimentoAtual++;
        }        
    }

    private void OnEnable()
    {
        if(executando)
        StartCoroutine(ExecutaChar(movimentoAtual));
    }

    private bool ColisaoTile()
    {
        if(collisionCounter > 0)
        {
            collisionCounter--;
            return true;
        }
        return false;
    }

    private void CheckPassableTile()
    {
        int charC = personagem.GetComponent<Movement>().coluna;
        int charL = personagem.GetComponent<Movement>().linha;

        if (personagem.transform.localScale.x == 0f)
        {
            personagem.transform.localScale = new Vector3(oldscale, oldscale, oldscale);
        }
        else if (tileMap[charL, charC].GetComponent<TileData>().colisionType == 1)
        {            
            oldscale = personagem.transform.localScale.x;
            personagem.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public IEnumerator ExecutaChar(int comecaEm)
    {        
        agent.isStopped = false;
        executando = true;
        movimentoAtual = comecaEm;
        listaComandos.Clear();
        GameObject quadro = GameObject.FindGameObjectWithTag("quadroComandos");        
        foreach(Transform child in quadro.transform)
        {
            if(child.gameObject.tag.Equals("frente"))
            {
                listaComandos.Add(FRENTE);
            }
            else if (child.gameObject.tag.Equals("tras"))
            {
                listaComandos.Add(TRAS);
            }
            else if (child.gameObject.tag.Equals("esquerda"))
            {
                listaComandos.Add(ESQUERDA);
            }
            else if (child.gameObject.tag.Equals("direita"))
            {
                listaComandos.Add(DIREITA);
            }
        }

        Movement charMovement = personagem.GetComponent<Movement>();
        while (map.activeSelf && listaComandos.Count > movimentoAtual)
        {
            CheckPassableTile();
            StateMachine.ChangeState(StateMachine.EXECUTAR);
            int tipoMovimento = listaComandos[movimentoAtual];
            switch (tipoMovimento)
            {
                case FRENTE:
                    charMovement.moverParaFrente();
                    if (Colidiu(FRENTE)) {
                        collisionCounter = collisionTicks;
                        break;
                    }
                    SetaNovaPosicao(FRENTE);                    
                    break;
                case TRAS:
                    charMovement.moverParaTras();
                    if (Colidiu(TRAS)){
                        collisionCounter = collisionTicks;
                        break;
                    }
                    SetaNovaPosicao(TRAS);                    
                    break;
                case DIREITA:
                    SetaNovaDirecao(DIREITA);
                    charMovement.virarDireita();
                    break;
                case ESQUERDA:
                    SetaNovaDirecao(ESQUERDA);
                    charMovement.virarEsquerda();
                    break;
                default:
                    Debug.Log("THIS SHOULD NOT EVER HAPPEN");
                    break;
            }        

            yield return null;

            while (agent.hasPath || agent.velocity.sqrMagnitude != 0 || NaoVirouCompletamente(tipoMovimento) || ColisaoTile())
            {
                yield return null;
            }
            charMovement.parar();
            movimentoAtual++;
            CheckPassableTile();
        }
        startButton.SetActive(true);
        stopButton.SetActive(false);
        executando = false;
        agent.isStopped = true;
    }

    public void RastrearMapa()
    {
        SceneManager.LoadScene("mapTracker");
    }

    public void ResetaChar()
    {
        StateMachine.ChangeState(StateMachine.REFATORAR);
        AddCharToMap();
    }
}
