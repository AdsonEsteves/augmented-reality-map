using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class StateMachine{

    public const string REPOUSO = "repouso";
    public const string PERCA = "perca";
    public const string CRIAR_MAPA = "criando";
    public const string PROGRAMAR = "programando";
    public const string REFATORAR = "refatorando";
    public const string EXECUTAR = "executando";

    static string stateText = "";
    static string state = "";
    static long fileName = System.DateTime.Now.ToFileTime();
    static float target_time = 10.0f;

    public static void ChangeState(string newState)
    {
        if(newState != state)
        {
            stateText += "\n" + newState + " - " + System.DateTime.Now;
            state = newState;
            WriteString("\n" + newState + " - " + System.DateTime.Now);
        }
        ResetTimer();
    }

    public static IEnumerator PerformCoroutine()
    { //the coroutine that runs on our monobehaviour instance
        while (true)
        {
            target_time -= Time.deltaTime;
            if (target_time < 0.0f)
            {
                target_time = 10.0f;
                StateMachine.ChangeState(StateMachine.REPOUSO);
            }
            yield return 0;
        }
    }

    static public void ResetTimer()
    {
        target_time = 10.0f;
    }

    static void WriteString(string Text)
    {
        string path = Application.persistentDataPath + "/"+fileName+".txt";

        Debug.Log(path);

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(Text);
        writer.Close();

        //Re-import the file to update the reference in the editor
        //AssetDatabase.ImportAsset(path);
    }
}
