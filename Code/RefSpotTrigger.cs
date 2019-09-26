using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefSpotTrigger : MonoBehaviour {
    public static RefSpotTrigger instance;


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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GameState.returnedToPlayerSpot = true;
        }
    }
}
