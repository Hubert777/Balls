using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{

    //const float G = 667.4f;
    const float G = 1f;

    public static List<Attractor> Attractors;

    public float gravityField;
    
    public Rigidbody rb;
    

    void FixedUpdate()
    {
        foreach (Attractor attractor in Attractors)
        {
            if (attractor.gameObject.GetComponent<Rigidbody>().mass >= 50)
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
        else if (distance<gravityField)
        {
            float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
            Vector3 force = direction.normalized * forceMagnitude;

            rbToAttract.AddForce(force);
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
                gravityField += collision.rigidbody.mass;

                this.gameObject.transform.localScale = new Vector3(thisScale + colliderScale, thisScale + colliderScale, thisScale + colliderScale);

                rb.velocity = new Vector3(0, 0, 0);
                Destroy(collision.gameObject);
            }
        }
    }
}