using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    Vector3 rotaionAxis = new Vector3(0, 0, -1);
    public float rotationSpeed = 5;
    public GameObject spot;

    [HideInInspector]
    public static BallScript instance;
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

    void Start()
    {

        

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BallRandomize()
    {
        var randomAngle = Random.Range(30, 200);
        StartCoroutine(CircularLerp(transform, rotaionAxis, randomAngle, rotationSpeed));
    }


    IEnumerator CircularLerp(Transform target, Vector3 axis, float angle, float speed)
    {
        for (int i = 0; i < angle / speed; i++)
        {
            Quaternion q = Quaternion.AngleAxis(speed, axis);
            Vector3 finalPos = q * target.position;
            target.position = finalPos;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.2f);
        this.spot.SetActive(true);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GameState.ballReached = true;
        }
    }
}
