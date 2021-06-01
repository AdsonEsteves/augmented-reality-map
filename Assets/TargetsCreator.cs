using EasyAR;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetsCreator : MonoBehaviour {

    public List<String> targetsNames = new List<String>();

    public GameObject targetPrefab;
    public GameObject trackerPrefab;
    public int[] sameTargetMaxNumber;
    public int lastID = 0;
    public Material selectedMaterial;

    // Use this for initialization
    void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        int tilesCriados = 0;
		foreach(String targetName in targetsNames)
        {
            for (int i = 0; i < sameTargetMaxNumber[lastID]; i++)
            {
                //GameObject tracker = Instantiate<GameObject>(trackerPrefab);
                //tracker.transform.SetParent(GameObject.FindGameObjectWithTag("ARGroup").transform);
                //tracker.name = targetName + "Tracker";

                GameObject target = Instantiate(targetPrefab);
                target.name = targetName + "Target" + i;
                ImageTargetBehaviour targetBehaviour = target.GetComponent<ImageTargetBehaviour>();
                targetBehaviour.Path = targetName + ".png";
                targetBehaviour.Name = targetName;
                targetBehaviour.Size.x = 10;
                targetBehaviour.Size.y = 10;
                targetBehaviour.tileID = lastID;
                if (i == 0) { targetBehaviour.AddToTextures(); }

                //targetBehaviour.Bind(tracker.GetComponent<ImageTrackerBaseBehaviour>());
                targetBehaviour.Bind(GameObject.FindGameObjectWithTag("MainTracker").GetComponent<ImageTrackerBaseBehaviour>());

                var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plane.transform.SetParent(target.transform);
                plane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                plane.GetComponent<Renderer>().material = selectedMaterial;
                tilesCriados++;
            }
            lastID++;
        }
        GetComponent<MapGenerator>().numeroDeTiles = tilesCriados;        
    }    
}
