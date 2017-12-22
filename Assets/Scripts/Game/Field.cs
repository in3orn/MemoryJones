using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Showable))]
public class Field : MonoBehaviour
{
    public static readonly float SIZE = 1.2f;

    public enum TypeEnum
    {
        Ground = 0,
        Wall,
        Hole
    }

    [SerializeField]
    private TypeEnum type = TypeEnum.Ground;

    [SerializeField]
    private Color groundColor;

    [SerializeField]
    private Color wallColor;

    [SerializeField]
    private Color holeColor;

    private SpriteRenderer spriteRenderer;

    private Showable showable;

    private bool visited = false;

    public bool Visited { get { return visited; } }

    public TypeEnum Type
    {
        get { return type; }
        set
        {
            type = value;

            switch (type)
            {
                case TypeEnum.Ground:
                    spriteRenderer.color = groundColor;
                    break;
                case TypeEnum.Wall:
                    spriteRenderer.color = wallColor;
                    break;
                case TypeEnum.Hole:
                    spriteRenderer.color = holeColor;
                    break;
            }
        }
    }

    public void Init(int x, int y, int type)
    {
        transform.position = new Vector3(Field.SIZE * x, -Field.SIZE * y, 0);
        Type = (Field.TypeEnum)type;
    }

    public bool IsAccessible()
    {
        return Type != TypeEnum.Wall;
    }

    public bool IsDeadly()
    {
        return Type == TypeEnum.Hole;
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        showable = GetComponent<Showable>();
    }

    public void Show()
    {
        showable.Show();
    }

    public void Hide()
    {
        showable.Hide();
    }

    public void Visit()
    {
        visited = true;
        if (showable.Hidden) showable.Show();
    }
}
