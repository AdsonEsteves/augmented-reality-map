using EasyAR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTargetBehaviour : ImageTargetBaseBehaviour
{

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        //GameObjectActiveControl = false;            
        TargetFound += OnTargetFound;
        TargetLost += OnTargetLost;
        TargetLoad += OnTargetLoad;
        TargetUnload += OnTargetUnload;
        StaticCoroutine.Start(StateMachine.PerformCoroutine());
    }

    void OnTargetFound(TargetAbstractBehaviour behaviour)
    {
        //this.gameObject.SetActive(true);
    }

    void OnTargetLost(TargetAbstractBehaviour behaviour)
    {
        StateMachine.ChangeState(StateMachine.PERCA);
        //gameObject.SetActive(false);
    }

    void OnTargetLoad(ImageTargetBaseBehaviour behaviour, ImageTrackerBaseBehaviour tracker, bool status)
    {
        //Debug.Log("Load target (" + status + "): " + Target.Id + " (" + Target.Name + ") " + " -> " + tracker);
    }

    void OnTargetUnload(ImageTargetBaseBehaviour behaviour, ImageTrackerBaseBehaviour tracker, bool status)
    {
        //Debug.Log("Unload target (" + status + "): " + Target.Id + " (" + Target.Name + ") " + " -> " + tracker);
    }

}
