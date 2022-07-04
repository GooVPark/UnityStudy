using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public System.Action<Bezier> Restore;
    [SerializeField] float speed = 10;
    private Vector3 target;
    private LineRenderer lineRenderer;

    public void Activate(Vector3 start, Vector3 target)
    {
        transform.position = start;
        lineRenderer = GetComponent<LineRenderer>();
        gameObject.SetActive(true);
        this.target = target;
        if (bezierCurve != null) StopCoroutine(bezierCurve);
        bezierCurve = StartCoroutine(BezierCurve());
    }

    Coroutine bezierCurve;
    IEnumerator BezierCurve()
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = Random.onUnitSphere * Random.Range(1f, 30f);
        Vector3 pos3 = Random.onUnitSphere * Random.Range(1f, 50f);
        Vector3 pos4 = target;

        float elapsedTime = 0;
        float time = 5f;
        float t = 0;
        while (t < 1)
        {
            elapsedTime += Time.fixedDeltaTime * speed;
            t = elapsedTime / time;
            Vector3 pos1To2 = Vector3.Lerp(pos1, pos2, t);
            Vector3 pos2To3 = Vector3.Lerp(pos2, pos3, t);
            Vector3 pos3To4 = Vector3.Lerp(pos3, pos4, t);

            Vector3 pos12To23 = Vector3.Lerp(pos1To2, pos2To3, t);
            Vector3 pos23To34 = Vector3.Lerp(pos2To3, pos3To4, t);

            Vector3 posFinal = Vector3.Lerp(pos12To23, pos23To34, t);

            transform.LookAt(posFinal);
            lineRenderer.SetPositions(new Vector3[2] { transform.position, posFinal });
            transform.position = posFinal;

            yield return new WaitForFixedUpdate();
        }

        Restore(this);
    }
}
