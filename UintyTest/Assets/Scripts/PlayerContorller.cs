using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContorller : MonoBehaviour
{
    private Rigidbody rigid;

    public GameObject bezier;
    public Transform target;
    Factory factory;

    [SerializeField] float horiziontal = 0;
    [SerializeField] float vertical = 0;
    [SerializeField] float speed = 0;
    [SerializeField] Vector3 normalizedAxis;

    [SerializeField] float elapsedTime = 1;
    float interval = 1;

    // Start is called before the first frame update
    void Start()
    {
        factory = new Factory(bezier);

        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horiziontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        normalizedAxis = new Vector3(horiziontal, 0, vertical).normalized;
        transform.position += speed * Time.deltaTime * normalizedAxis;

        elapsedTime += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && elapsedTime >= interval)
        {
            elapsedTime = 0;
            StartCoroutine(Fire());
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector3.up, ForceMode.Impulse);
        }
    }

    private void LateUpdate()
    {
        horiziontal = 0;
        vertical = 0;
    }

    IEnumerator Fire()
    {
        int count = 0;

        while(count < 10)
        {
            Bezier missile = factory.Get();
            missile.Restore += Restore;
            missile.Activate(transform.position, target.position);

            yield return new WaitForSeconds(0.05f);
            count++;
        }
    }

    public void Restore(Bezier obj)
    {
        obj.Restore -= Restore;
        factory.Restore(obj);
    }
}

public class Factory
{
    private Queue<Bezier> queue = new Queue<Bezier>();
    private GameObject prefab;

    public Factory(GameObject bezier)
    {
        prefab = bezier;
    }

    public Bezier Get()
    {
        if(queue.Count == 0)
        {
            GameObject obj = GameObject.Instantiate(prefab);
            obj.SetActive(false);
            queue.Enqueue(obj.GetComponent<Bezier>());
        }
        
        return queue.Dequeue();
    }

    public void Restore(Bezier obj)
    {
        obj.gameObject.SetActive(false);
        queue.Enqueue(obj);
    }
}
