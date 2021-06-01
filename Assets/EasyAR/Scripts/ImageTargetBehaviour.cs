//=============================================================================================================================
//
// Copyright (c) 2015-2017 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
// EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
// and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//=============================================================================================================================

using UnityEngine;

namespace EasyAR
{
    public class ImageTargetBehaviour : ImageTargetBaseBehaviour
    {

        public MapGenerator mapGenerator;
        public int tileID;

        private bool tracked = false;
        private int[] lastPosition = new int[2];

        protected override void Awake()
        {
            base.Awake();
            //GameObjectActiveControl = false;            
            TargetFound += OnTargetFound;
            TargetLost += OnTargetLost;
            TargetLoad += OnTargetLoad;
            TargetUnload += OnTargetUnload;
            mapGenerator = GameObject.FindGameObjectWithTag("MapTracker").GetComponent<MapGenerator>();
        }

        
        protected override void Start()
        {
            base.Start();
            //gameObject.SetActive(false);
        }

        protected override void Update()
        {
            base.Update();

            if(tracked)
            {
                if (mapGenerator.targetsFound <=0)
                {
                    Debug.Log("Primeiro Tile");
                    mapGenerator.AdicionaTile(this.gameObject, 0, 0, findDirection());
                }
                else
                {
                    Debug.Log("Novo Tile "+this.gameObject.name);
                    int[] activeTargetPositions = mapGenerator.GetActiveTargetPosition(this.gameObject);
                    GameObject targetAtivo = mapGenerator.getTileTarget(activeTargetPositions[0], activeTargetPositions[1]);
                    Debug.Log("posicao tile ativo "+targetAtivo.name+" " + targetAtivo.transform.position);
                    Debug.Log("posicao tile novo " + this.gameObject.name + " " + this.gameObject.transform.position);
                    float diferencaX = (this.gameObject.transform.position.x - targetAtivo.transform.position.x) / 10;
                    float diferencaZ = (targetAtivo.transform.position.z - this.gameObject.transform.position.z) / 10;
                    int coluna = activeTargetPositions[1] + Mathf.RoundToInt(diferencaX);
                    int linha = activeTargetPositions[0] + Mathf.RoundToInt(diferencaZ);
                    Debug.Log("Adicionar em " + coluna + " X e " + linha + " Y");
                    mapGenerator.AdicionaTile(this.gameObject, linha, coluna, findDirection());
                }
                mapGenerator.targetsFound++;
                tracked = false;
            }
        }

        void OnTargetFound(TargetAbstractBehaviour behaviour)
        {
            StateMachine.ChangeState(StateMachine.CRIAR_MAPA);
            Debug.Log("Found: " + Name);
            tracked = true;            
        }

        void OnTargetLost(TargetAbstractBehaviour behaviour)
        {
            mapGenerator.targetsFound--;
            if(mapGenerator.targetsFound<=0)
            {
                StateMachine.ChangeState(StateMachine.PERCA);
                mapGenerator.Resetar_Mapa();
                mapGenerator.targetsFound = 0;
            }
            Debug.Log("Lost: " + Name);            
        }

        void OnTargetLoad(ImageTargetBaseBehaviour behaviour, ImageTrackerBaseBehaviour tracker, bool status)
        {
            mapGenerator.carregados++;
            //Debug.Log("Load target (" + status + "): " + Target.Id + " (" + Target.Name + ") " + " -> " + tracker);
        }

        void OnTargetUnload(ImageTargetBaseBehaviour behaviour, ImageTrackerBaseBehaviour tracker, bool status)
        {
           //Debug.Log("Unload target (" + status + "): " + Target.Id + " (" + Target.Name + ") " + " -> " + tracker);
        }

        public void SetLastPosition(int x, int y)
        {
            lastPosition[0] = x;
            lastPosition[1] = y;
        }

        public int[] getLastPosition()
        {
            return lastPosition;
        }

        public void AddToTextures()
        {
            mapGenerator.AddMapTexture(this.Path);
        }

        private int findDirection()
        {
            if(this.transform.forward.x > 0.5)
            {
                return globalMap.X_POSITIVO;
            }
            if (this.transform.forward.x < -0.5)
            {
                return globalMap.X_NEGATIVO;
            }
            if (this.transform.forward.z > 0.5)
            {
                return globalMap.Z_POSITIVO;
            }
            if (this.transform.forward.z < -0.5)
            {
                return globalMap.Z_NEGATIVO;
            }

            return globalMap.Z_POSITIVO;
        }
    }
}
