using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
public class Collectible : MonoBehaviour
{
    public delegate void CollectedAction();
    public event CollectedAction OnCollected;

    [SerializeField]
    private float collectDuration = 0.5f;

    private bool collected = false;

    private CircleCollider2D circleCollider;

    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void Init(Transform transform)
    {
        this.transform.parent = transform;
        this.transform.position = transform.position;
        this.transform.localScale = Vector3.one;
        circleCollider.enabled = true;
        collected = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!collected)
        {
            Collect();
            OnCollected();
        }
    }

    public void Collect()
    {
        StartCoroutine(collect());
    }

    private IEnumerator collect()
    {
        collected = true;

        Vector3 start = transform.localScale;

        float t = 0f;
        while (true)
        {
            t += Time.deltaTime / collectDuration;
            transform.localScale = Vector3.Lerp(start, Vector3.zero, t);

            if (t >= 1) break;
            yield return null;
        }
    }
}
