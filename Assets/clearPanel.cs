using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clearPanel : MonoBehaviour {

	public void LimparPainel()
    {
        StateMachine.ChangeState(StateMachine.REFATORAR);
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
