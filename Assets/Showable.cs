using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Showable : MonoBehaviour
{

    [SerializeField]
    private float showDuration = 0.5f;

    [SerializeField]
    private float hideDuration = 0.5f;

    [SerializeField]
    private float showDelay = 0f;

    [SerializeField]
    private float hideDelay = 0f;

    private SpriteRenderer spriteRenderer;

    private bool hidden = false;

    public bool Hidden { get { return hidden; } }

    public float ShowDelay { get; set; }

    public float HideDelay { get; set; }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Show()
    {
        hidden = false;

        foreach (Showable child in GetComponentsInChildren<Showable>())
        {
            if (child != this) child.Show();
        }

        StartCoroutine(setOpacity(spriteRenderer.color.a, 1f, showDelay, showDuration));
    }

    public void Hide()
    {
        hidden = true;

        foreach (Showable child in GetComponentsInChildren<Showable>())
        {
            if (child != this) child.Hide();
        }

        StartCoroutine(setOpacity(spriteRenderer.color.a, 0f, hideDelay, hideDuration));
    }

    private IEnumerator setOpacity(float from, float to, float delay, float duration)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        float t = 0f;
        while (true)
        {
            t += Time.deltaTime / duration;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Lerp(from, to, t));

            if (t >= 1) break;
            yield return null;
        }
    }
}
