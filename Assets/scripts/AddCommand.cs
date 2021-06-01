using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddCommand : MonoBehaviour {

    bool update = false;
    public bool commandPanelType = true;

    private void Start()
    {
        if(commandPanelType)
        {
            this.gameObject.GetComponent<Button>().onClick.AddListener(AdicionaComando);
        }
        else
        {
            this.gameObject.GetComponent<Button>().onClick.AddListener(RemoveComando);
        }
        
    }

    public void AdicionaComando()
    {
        StateMachine.ChangeState(StateMachine.PROGRAMAR);

        GameObject quadro = GameObject.FindGameObjectWithTag("quadroComandos");
        ScrollRect scroll = GameObject.FindGameObjectWithTag("ScrollView").GetComponent<ScrollRect>();

        GameObject comandoNovo = Instantiate < GameObject > (this.gameObject);
        comandoNovo.GetComponent<AddCommand>().commandPanelType = false;
        comandoNovo.transform.SetParent(quadro.transform);
        comandoNovo.transform.localScale = new Vector3(1f, 1f, 1f);
        update = true;
    }

    public void RemoveComando()
    {
        StateMachine.ChangeState(StateMachine.REFATORAR);
        Object.Destroy(this.gameObject);
    }

    private void Update()
    {
        if(update)
        {
            Canvas.ForceUpdateCanvases();
            ScrollRect scroll = GameObject.FindGameObjectWithTag("ScrollView").GetComponent<ScrollRect>();
            scroll.horizontalNormalizedPosition = 1f;
            Canvas.ForceUpdateCanvases();
            update = false;
        }
    }
}
