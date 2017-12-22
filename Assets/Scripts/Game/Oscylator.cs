using UnityEngine;

public class Oscylator : MonoBehaviour {

    [SerializeField]
    private Vector3 oscilation;

    private Vector3 start;

    private Vector3 end;

    [SerializeField]
    private float duration = 50f;

    private float dt;

    private float t;

    void Awake()
    {
        start = transform.position;
        end = start + oscilation;

        dt = 1f / duration;
        t = 0f;
    }

	void Update () {
        transform.localPosition = Vector3.Lerp(start, end, 0.5f + 0.5f * Mathf.Sin(t * Mathf.PI));
        t += dt;
    }
}
