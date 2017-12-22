using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
public class Player : MonoBehaviour
{
    public delegate void MovedAction();
    public event MovedAction OnMoved;

    public delegate void DiedAction();
    public event DiedAction OnDied;

    enum StateEnum
    {
        Idle = 0,
        Moving,
        Falling,
        Died
    }

    [SerializeField]
    private float moveDuration = 0.5f;

    [SerializeField]
    private float fallDuration = 0.5f;

    [SerializeField]
    private float teleportDuration = 0.5f;

    private StateEnum state;

    private CircleCollider2D circleCollider;

    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }
    public void Init(Transform fieldTransform)
    {
        state = StateEnum.Idle;

        transform.parent = fieldTransform;
        transform.position = fieldTransform.position;
        transform.localScale = Vector3.one;

        circleCollider.enabled = true;
    }

    public bool CanMove()
    {
        return state == StateEnum.Idle;
    }

    public void MoveLeft()
    {
        move(transform.position + Vector3.left * Field.SIZE);
    }

    public void MoveRight()
    {
        move(transform.position + Vector3.right * Field.SIZE);
    }

    public void MoveUp()
    {
        move(transform.position + Vector3.up * Field.SIZE);
    }

    public void MoveDown()
    {
        move(transform.position + Vector3.down * Field.SIZE);
    }

    public void Teleport(Field field)
    {
        StartCoroutine(teleport(field));
    }

    private IEnumerator teleport(Field field)
    {
        Vector3 prevScale = transform.localScale;

        yield return scaleTo(prevScale, Vector3.zero);
        field.Visit();
        transform.parent = field.transform;
        transform.position = field.transform.position;
        yield return scaleTo(Vector3.zero, prevScale);
    }

    private IEnumerator scaleTo(Vector3 from, Vector3 to)
    {
        float t = 0f;
        while (true)
        {
            t += Time.deltaTime / teleportDuration;
            transform.localScale = Vector3.Lerp(from, to, t);

            if (t >= 1) break;
            yield return null;
        }
    }

    private void move(Vector2 target)
    {
        StartCoroutine(move(transform.position, target));
    }

    private IEnumerator move(Vector3 start, Vector3 end)
    {
        state = StateEnum.Moving;

        float t = 0f;
        while(true)
        {
            t += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(start, end, t);

            if (t >= 1) break;
            yield return null;
        }

        state = StateEnum.Idle;
        OnMoved();
    }

    public void Fall()
    {
        StartCoroutine(fall(transform.localScale));
    }

    private IEnumerator fall(Vector3 start)
    {
        state = StateEnum.Falling;

        float t = 0f;
        while (true)
        {
            t += Time.deltaTime / fallDuration;
            transform.localScale = Vector3.Lerp(start, Vector3.zero, t);

            if (t >= 1) break;
            yield return null;
        }

        state = StateEnum.Died;
        OnDied();
    }
}
