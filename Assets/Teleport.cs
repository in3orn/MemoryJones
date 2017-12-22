using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
public class Teleport : MonoBehaviour
{
    public delegate void UsedAction(Vector2 target);
    public event UsedAction OnUsed;

    [SerializeField]
    private Vector2 target;

    private CircleCollider2D circleCollider;

    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void Init(Transform transform, Vector2 target)
    {
        this.transform.parent = transform;
        this.transform.position = transform.position;
        this.target = target;
        circleCollider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        OnUsed(target);
    }
}
