using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderScript : MonoBehaviour {
    public Vector3 bounds;
	
	void Start () {
        bounds = GetComponent<SpriteRenderer>().bounds.extents + transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
