using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour {
    
    public GameObject ball;
    public Text text;
    Vector3 zero = new Vector3(0, 0, 0);

    bool permission = true;
    int boomBalls = 50;
    int counterOfBalls;
    List<GameObject> balls;
    
	void Start () {
        PlayerPrefs.SetInt("Permission", 1);
        InvokeRepeating("SpawnBall", 0f, 0.25f);
    }

    void FixedUpdate()
    {
        GameObject g = GameObject.FindGameObjectWithTag("Boom");
        if (g != null && permission)
        {
            balls = new List<GameObject>();
            boomBalls = (int)g.GetComponent<Rigidbody>().mass;
            Transform tr = g.transform;

            Destroy(g);

            for(int i = 0; i < boomBalls; i++)
            {
                GameObject oneOfFifty = Instantiate(ball, tr.position, Quaternion.identity);
                oneOfFifty.GetComponent<Collider>().enabled = false;

                balls.Add(oneOfFifty);

                Vector3 vector = new Vector3(Random.Range(-300,300), Random.Range(-300, 300), Random.Range(-300, 300));
                oneOfFifty.GetComponent<Rigidbody>().AddForce(vector);
                
                StartCoroutine(TurnOnCollisions(oneOfFifty));
            }
        }
        foreach(var one in balls)
        {
            SlowDown(one);
        }
    }

    void Update () {
        CountBalls();
    }

    void SlowDown(GameObject ball)
    {
        if (ball.GetComponent<Rigidbody>().velocity != zero)
        {
            ball.GetComponent<Rigidbody>().velocity *= 0.95f;
        }
    }

    void SpawnBall()
    {
        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 40f));
        Instantiate(ball, screenPosition, Quaternion.identity);
    }

    void CountBalls()
    {
        counterOfBalls = GameObject.FindGameObjectsWithTag("Ball").Length;
        text.text = "Balls "+counterOfBalls.ToString();
        if (counterOfBalls >= 250)
        {
            permission = false;
            PlayerPrefs.SetInt("Permission", 0);
            CancelInvoke();
        }
    }

    IEnumerator TurnOnCollisions(GameObject ball)
    {
        yield return new WaitForSeconds(0.5f);
        ball.GetComponent<Collider>().enabled = true;
    }
}
