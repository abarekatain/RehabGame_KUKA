using CSNamedPipe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComTest : MonoBehaviour {
    string Ms;
    public bool dd = false;
    public bool ServerInitiated = false;
    public string Data = null;
    NamedPipeServer PServer1, PServer2;
    // Use this for initialization
    void Start () {

        StartCoroutine(StartServers());


    }
	
	// Update is called once per frame
	void Update () {
        
        if (dd && ServerInitiated)
        {

            Ms = "dadayeh";
            PServer2.SendMessage(Ms, PServer2.clientse);
            dd = false;
        }
	}

    void OnApplicationQuit()
    {
        Debug.Log("BYEBYE");
        PServer1.StopServer();
        PServer2.StopServer();
    }

    IEnumerator StartServers()
    {
        PServer1 = new NamedPipeServer(@"\\.\pipe\myNamedPipe1", 0);
        PServer2 = new NamedPipeServer(@"\\.\pipe\myNamedPipe2", 1);

        PServer1.myEvent += getData;

        PServer1.Start();
        PServer2.Start();

        ServerInitiated = true;
        yield return null;
    }

    private void getData(string s)
    {
        Data = s;
    }
}
