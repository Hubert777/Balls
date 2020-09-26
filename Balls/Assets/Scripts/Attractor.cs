using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    const float G = 0.5f;

    public static List<Attractor> Attractors;
    public Rigidbody rb;

    void FixedUpdate()
    {
        foreach (Attractor attractor in Attractors)
        {
            if (attractor.gameObject.GetComponent<Rigidbody>().mass >= 50 && PlayerPrefs.GetInt("Permission") == 1)
            {
                attractor.gameObject.tag = "Boom";
            }
            if (attractor != this)
                Attract(attractor);
        }
    }

    void OnEnable()
    {
        if (Attractors == null)
            Attractors = new List<Attractor>();

        Attractors.Add(this);
    }

    void OnDisable()
    {
        Attractors.Remove(this);
    }

    void Attract(Attractor objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.rb;

        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        if (distance == 0f)
            return;
        else if (distance<=rb.mass)
        {
            float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
            Vector3 force = direction.normalized * forceMagnitude;

            if (PlayerPrefs.GetInt("Permission") == 0)
            {
                force = -direction.normalized * forceMagnitude;
            }
            rbToAttract.AddForce(force);
        }
        else if (distance > rb.mass)
        {
            rbToAttract.velocity *= 0.9999999f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PlayerPrefs.GetInt("Permission")==1)
        {
            if (transform.position.x > collision.gameObject.transform.position.x)
            {
                float thisScale = transform.localScale.x;
                float colliderScale = collision.transform.localScale.x;

                if (thisScale < colliderScale)
                {
                    transform.position = collision.transform.position;
                }

                rb.mass += collision.rigidbody.mass;

                this.gameObject.transform.localScale = new Vector3(thisScale + colliderScale, thisScale + colliderScale, thisScale + colliderScale);

                rb.velocity = new Vector3(0, 0, 0);
                Destroy(collision.gameObject);
            }
        }
    }
}