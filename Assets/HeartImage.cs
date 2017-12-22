using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class HeartImage : MonoBehaviour {

    [SerializeField]
    private float showDuration = 0.5f;

    [SerializeField]
    private float hideDuration = 0.5f;

    private Image image;

    private bool hidden = false;

    public bool Hidden { get { return hidden; } }

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Show()
    {
        hidden = false;
        StartCoroutine(setOpacity(image.color.a, 1f, showDuration));
    }

    public void Hide()
    {
        hidden = true;
        StartCoroutine(setOpacity(image.color.a, 0f, hideDuration));
    }

    private IEnumerator setOpacity(float from, float to, float duration)
    {
        float t = 0f;
        while (true)
        {
            t += Time.deltaTime / duration;
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(from, to, t));

            if (t >= 1) break;
            yield return null;
        }
    }
}
