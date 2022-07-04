using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallContoller : MonoBehaviour
{
    private Rigidbody rigid;
    public float speed = 10.0f;

    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(-speed, 0f, speed * 0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            Vector3 currentPosition = transform.position;
            Vector3 incomeVector = currentPosition - startPosition;
            Vector3 normalVector = collision.contacts[0].normal;
            Vector3 reflectVector = Vector3.Reflect(incomeVector, normalVector).normalized;

            rigid.AddForce(reflectVector * speed);
        }

        startPosition = transform.position;
    }
}
