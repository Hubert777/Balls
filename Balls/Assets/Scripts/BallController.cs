using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
    
    public GameObject ball;

    int counterOfBalls;
    List<GameObject> Balls = new List<GameObject>();


	// Use this for initialization
	void Start () {
        PlayerPrefs.SetInt("Permission", 1);
        InvokeRepeating("SpawnBall", 0f, 0.25f);
    }

    void FixedUpdate()
    {
        GameObject g = GameObject.FindGameObjectWithTag("Boom");
        if (g != null && PlayerPrefs.GetInt("Permission") == 1)
        {
            
            Transform tr = g.transform;
            Destroy(g);
            
            
            for(int i = 0; i < 50; i++)
            {
                GameObject oneOfFifty = Instantiate(ball, tr.position, Quaternion.identity);
                oneOfFifty.GetComponent<Attractor>().enabled = false;
                oneOfFifty.GetComponent<Collider>().enabled = false;
                Balls.Add(oneOfFifty);
                Vector3 vector = new Vector3(Random.Range(-300,300), Random.Range(-300, 300), Random.Range(-300, 300));
                oneOfFifty.GetComponent<Rigidbody>().AddForce(vector);
                //ZWALNIANIE
            }
            StartCoroutine(TurnOnCollisions(Balls));
        }
    }

    // Update is called once per frame
    void Update () {
        CountBalls();
	}

    void SpawnBall()
    {
        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 40f));
        Instantiate(ball, screenPosition, Quaternion.identity);
    }

    void CountBalls()
    {
        counterOfBalls = GameObject.FindGameObjectsWithTag("Ball").Length;
        Debug.Log(counterOfBalls);
        if (counterOfBalls >= 250)
        {
            PlayerPrefs.SetInt("Permission", 0);
            CancelInvoke();
        }
    }

    IEnumerator TurnOnCollisions(List<GameObject> balls)
    {
        yield return new WaitForSeconds(0.5f);

        foreach(var ball in balls)
        {
            ball.GetComponent<Collider>().enabled = true;
            ball.GetComponent<Attractor>().enabled = true;
        
        }
        Balls.Clear();
    }
}
