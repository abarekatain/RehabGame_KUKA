using CSNamedPipe;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public CircleCollider2D PlayerBounds;
    public GameObject originSpot;
    public static PlayerScript instance;
    public Vector2 inputXBounds, inputYBounds,updownBounds,leftrightBounds,xCoeff,yCoeff;
    TrailRenderer trailRenderer;

    //Comunication Variables
    public bool ServerInitiated = false;
    NamedPipeServer PServerReciever, PServerSender;

    string[] stringSeparators = new string[] { "," };

    public Vector3 recievedCoordinates;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }
    }



    void Start () {
        StartCoroutine(StartServers());
        trailRenderer = GetComponent<TrailRenderer>();
        AdjustRangeOfMotion();
    }
	
	// Update is called once per frame
	void Update () {

        transform.position = Vector3.Lerp(transform.position, recievedCoordinates, 0.6f);
        transform.position =  Vector3.ClampMagnitude(transform.position, PlayerBounds.radius);

        if (GameState.ballReached)
        {
            trailRenderer.Clear();
            BallScript.instance.spot.SetActive(false);
            this.originSpot.SetActive(true);
            GameState.ballReached = false;
        }
        if (GameState.returnedToPlayerSpot)
        {
            trailRenderer.Clear();
            BallScript.instance.BallRandomize();
            GameState.returnedToPlayerSpot = false;
            this.originSpot.SetActive(false);
        }
	}
    void OnApplicationQuit()
    {
        PServerReciever.StopServer();
        PServerSender.StopServer();
    }

    IEnumerator StartServers()
    {
        PServerReciever = new NamedPipeServer(@"\\.\pipe\myNamedPipe1", 0);
        PServerSender = new NamedPipeServer(@"\\.\pipe\myNamedPipe2", 1);

        PServerReciever.myEvent += getData;

        PServerReciever.Start();
        PServerSender.Start();

        ServerInitiated = true;
        yield return null;
    }
    private void getData(string s)
    {
        string[] c = s.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
        var x = float.Parse(c[0]) * xCoeff[0] + xCoeff[1];
        var y = float.Parse(c[1]) * yCoeff[0] + yCoeff[1];
        recievedCoordinates = new Vector3(x, y, -0.1f);
    }



    public void AdjustRangeOfMotion()
    {
        float up = PlayerBounds.transform.position.y + PlayerBounds.radius;
        float down = PlayerBounds.transform.position.y - PlayerBounds.radius;
        updownBounds = new Vector2(down, up);
        yCoeff = DeriveCoeffs(inputYBounds, updownBounds);

        float right = PlayerBounds.transform.position.x + PlayerBounds.radius;
        float left = PlayerBounds.transform.position.x - PlayerBounds.radius;
        leftrightBounds = new Vector2(left, right);
        xCoeff = DeriveCoeffs(inputXBounds, leftrightBounds);

    }

    public Vector2 DeriveCoeffs(Vector2 inputs,Vector2 bounds)
    {
        var a = (bounds.x - bounds.y) / (inputs.x - inputs.y);
        var b = bounds.x - a * inputs.x;
        return new Vector2(a, b);
    }
}
