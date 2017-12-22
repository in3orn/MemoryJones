using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    public delegate void FinishedAction();
    public event FinishedAction OnFinished;

    public delegate void FailedAction();
    public event FailedAction OnFailed;

    private enum StateEnum
    {
        Idle = 0,
        Showing,
        Playing,
        Finished,
        Failed
    }

    private StateEnum state;

    [SerializeField]
    private bool canExtendTheMap = false;

    [SerializeField]
    private FieldsFactory fieldsFactory;

    [SerializeField]
    private GameObject skullTemplate;

    [SerializeField]
    private GameObject teleportTemplate;

    [SerializeField]
    private Player player;

    [SerializeField]
    private Collectible finishGem;

    private Field[,] fields;

    private List<Collectible> collectibles;

    private List<GameObject> objects;

    [SerializeField]
    private GameObject center;

    [SerializeField]
    private float finishDuration = 1f;

    [SerializeField]
    private float hideInterval = 0.1f;

    void Awake()
    {
        collectibles = new List<Collectible>();
        objects = new List<GameObject>();
    }

    void Start()
    {
        player.OnMoved += performAction;
        player.OnDied += FailLevel;

        finishGem.OnCollected += FinishLevel;
    }

    public void Init(int level)
    {
        Clear();

        switch (level)
        {
            case 0:
                initNastyTeleportLevel1(); //initFirstPathLevel();
                break;
            case 1:
                initFirstHoleLevel();
                break;
            case 2:
                initFirstTeleportLevel();
                break;
            default:
                initRandomLevel();
                break;
        }

        initCenter();

        state = StateEnum.Idle;
        ShowFields();
    }

    private void initFirstPathLevel()
    {
        fields = fieldsFactory.Create(new int[5, 5] {
            { 1, 1, 1, 1, 1 },
            { 1, 1, 1, 0, 1 },
            { 1, 1, 1, 0, 1 },
            { 1, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1 }
        });
        finishGem.Init(fields[1, 3].transform);

        player.Init(fields[3, 1].transform);
        fields[3, 1].Visit();
    }

    private void initFirstHoleLevel()
    {
        fields = fieldsFactory.Create(new int[6, 6] {
            { 1, 1, 1, 1, 1, 1 },
            { 1, 1, 0, 0, 0, 1 },
            { 1, 1, 0, 1, 2, 1 },
            { 1, 1, 0, 1, 0, 1 },
            { 1, 0, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1, 1 }
        });
        finishGem.Init(fields[1, 4].transform);

        player.Init(fields[4, 1].transform);
        fields[4, 1].Visit();

        objects.Add(createSkull(fields[3, 4].transform));
    }

    private void initFirstTeleportLevel()
    {
        fields = fieldsFactory.Create(new int[7, 7] {
            { 1, 1, 1, 1, 1, 1, 1 },
            { 1, 0, 0, 0, 0, 0, 1 },
            { 1, 2, 2, 2, 2, 2, 1 },
            { 1, 2, 0, 0, 0, 2, 1 },
            { 1, 2, 0, 2, 2, 2, 1 },
            { 1, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1, 1, 1 }
        });
        finishGem.Init(fields[1, 5].transform);

        player.Init(fields[5, 1].transform);
        fields[5, 1].Visit();

        objects.Add(createTeleport(fields[3, 4].transform, new Vector2(1, 1)));
    }

    private void initLevel1()
    {
        fields = fieldsFactory.Create(new int[7, 7] {
            { 2, 2, 0, 0, 2, 2, 2 },
            { 0, 0, 0, 0, 0, 0, 2 },
            { 0, 2, 2, 2, 2, 2, 0 },
            { 0, 0, 0, 0, 0, 0, 0 },
            { 2, 2, 2, 2, 0, 2, 2 },
            { 2, 0, 0, 0, 0, 0, 2 },
            { 0, 0, 2, 0, 2, 2, 2 }
        });
        finishGem.Init(fields[0, 3].transform);

        player.Init(fields[6, 0].transform); //TODO field
        fields[6, 0].Visit(); //TODO move to player.init

        objects.Add(createTeleport(fields[3, 4].transform, new Vector2(1, 1)));
    }

    private void initNastyTeleportLevel1()
    {
        fields = fieldsFactory.Create(new int[7, 7] {
            { 0, 2, 0, 2, 2, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 2 },
            { 0, 2, 2, 2, 2, 0, 0 },
            { 0, 0, 0, 0, 0, 2, 0 },
            { 2, 2, 0, 2, 0, 2, 0 },
            { 2, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 2, 0, 2, 2, 2 }
        });
        finishGem.Init(fields[0, 6].transform);

        player.Init(fields[6, 0].transform); //TODO field
        fields[6, 0].Visit(); //TODO move to player.init

        objects.Add(createTeleport(fields[5, 3].transform, new Vector2(6, 0)));
    }

    private void initRandomLevel()
    {
        fields = fieldsFactory.Create(new int[3, 3] { { 2, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } });
        finishGem.Init(fields[2, 0].transform);

        player.Init(fields[0, 2].transform);
    }

    private GameObject createSkull(Transform transform)
    {
        GameObject obj = Instantiate(skullTemplate, transform) as GameObject;
        obj.transform.position = transform.position;
        return obj;
    }

    private GameObject createTeleport(Transform transform, Vector2 target)
    {
        GameObject obj = Instantiate(teleportTemplate) as GameObject;
        Teleport teleport = obj.GetComponent<Teleport>();
        teleport.Init(transform, target);
        teleport.OnUsed += teleportPlayer;
        return obj;
    }

    //TODO reusable fields & collectibles could be better
    public void Clear()
    {
        clearFields();
        clearCollectibles();
        clearObjects();
    }

    private void clearCollectibles()
    {
        foreach (Collectible collectible in collectibles)
        {
            Destroy(collectible.gameObject);
        }
        collectibles.Clear();
    }

    private void clearObjects()
    {
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
        objects.Clear();
    }

    private void clearFields()
    {
        if (fields != null)
        {
            for (int y = 0; y < fields.GetLength(0); y++)
            {
                for (int x = 0; x < fields.GetLength(1); x++)
                {
                    Destroy(fields[y, x].gameObject);
                    fields[y, x] = null;
                }
            }
            fields = null;
        }
    }

    private void initCenter()
    {
        center.transform.position = new Vector2((fields.GetLength(0) - 1) * Field.SIZE, -(fields.GetLength(1) - 1) * Field.SIZE) * 0.5f;
    }

    public bool CanMoveLeft()
    {
        return CanMove() && CanMoveLeft(player);
    }

    public bool CanMoveRight()
    {
        return CanMove() && CanMoveRight(player);
    }

    public bool CanMoveUp()
    {
        return CanMove() && CanMoveUp(player);
    }

    public bool CanMoveDown()
    {
        return CanMove() && CanMoveDown(player);
    }

    public bool CanMove()
    {
        return (state == StateEnum.Showing || state == StateEnum.Playing) && player.CanMove();
    }

    public void MoveLeft()
    {
        if (CanMoveLeft())
        {
            player.MoveLeft();
        }
    }

    public void MoveRight()
    {
        if (CanMoveRight())
        {
            player.MoveRight();
        }
    }

    public void MoveUp()
    {
        if (CanMoveUp())
        {
            player.MoveUp();
        }
    }

    public void MoveDown()
    {
        if (CanMoveDown())
        {
            player.MoveDown();
        }
    }

    //TODO should be in LevelMap
    public bool CanMoveLeft(Player player)
    {
        int x = Mathf.RoundToInt(player.transform.position.x / Field.SIZE) - 1;
        int y = -Mathf.RoundToInt(player.transform.position.y / Field.SIZE);
        return x >= 0 ? fields[y, x].IsAccessible() : canExtendTheMap;
    }

    public bool CanMoveRight(Player player)
    {
        int x = Mathf.RoundToInt(player.transform.position.x / Field.SIZE) + 1;
        int y = -Mathf.RoundToInt(player.transform.position.y / Field.SIZE);
        return x < fields.GetLength(0) ? fields[y, x].IsAccessible() : canExtendTheMap;
    }

    public bool CanMoveUp(Player player)
    {
        int x = Mathf.RoundToInt(player.transform.position.x / Field.SIZE);
        int y = -Mathf.RoundToInt(player.transform.position.y / Field.SIZE) - 1;
        return y >= 0 ? fields[y, x].IsAccessible() : canExtendTheMap;
    }

    public bool CanMoveDown(Player player)
    {
        Vector2 pos = getFieldPosition(player) + Vector2.up;
        int x = (int)pos.x;
        int y = (int)pos.y;
        return y < fields.GetLength(1) ? fields[y, x].IsAccessible() : canExtendTheMap;
    }

    private Vector2 getFieldPosition(Player player)
    {
        int x = Mathf.RoundToInt(player.transform.position.x / Field.SIZE);
        int y = -Mathf.RoundToInt(player.transform.position.y / Field.SIZE);

        return new Vector2(x, y);
    }

    private void performAction()
    {
        Vector2 pos = getFieldPosition(player);
        int x = (int)pos.x;
        int y = (int)pos.y;
        if (x < 0 || y < 0 || x >= fields.GetLength(0) || y >= fields.GetLength(1))
        {
            player.Fall();
        }
        else
        {
            Field field = fields[y, x];
            field.Visit();
            player.transform.parent = field.transform;

            switch (field.Type)
            {
                case Field.TypeEnum.Hole:
                    player.Fall();
                    break;
                    //TODO lawa, woda, etc?
            }
        }

        if (state == StateEnum.Showing)
        {
            state = StateEnum.Playing;
            HideFields(false);
        }
    }

    private void teleportPlayer(Vector2 target)
    {
        Field field = fields[(int)target.x, (int)target.y];
        player.Teleport(field);
    }

    public void FinishLevel()
    {
        state = StateEnum.Finished;
        HideFields(true);
    }

    public void FailLevel()
    {
        state = StateEnum.Failed;
        HideFields(true);
    }

    public void ShowFields()
    {
        StartCoroutine(showFields());
    }

    private IEnumerator showFields() //TODO show from player
    {
        int size = fields.GetLength(0) + fields.GetLength(1);
        for (int s = 1; s < size; s++)
        {
            for (int ds = 0; ds < s; ds++)
            {
                int y = ds;
                int x = s - ds - 1;
                if (y >= 0 && x >= 0 && y < fields.GetLength(0) && x < fields.GetLength(1))
                {
                    Field field = fields[y, x];
                    field.Show();
                }
            }
            yield return new WaitForSeconds(hideInterval);
        }

        state = StateEnum.Showing;
    }

    public void HideFields(bool hideAll)
    {
        StartCoroutine(hideFields(hideAll));
    }

    private IEnumerator hideFields(bool hideAll)
    {
        int size = fields.GetLength(0) + fields.GetLength(1);
        for (int s = 1; s < size; s++)
        {
            for (int ds = 0; ds < s; ds++)
            {
                int y = ds;
                int x = s - ds - 1;
                if (y >= 0 && x >= 0 && y < fields.GetLength(0) && x < fields.GetLength(1))
                {
                    Field field = fields[y, x];
                    if (hideAll || !field.Visited) field.Hide();
                }
            }
            yield return new WaitForSeconds(hideInterval);
        }

        onHidden();
    }

    private void onHidden()
    {
        switch (state)
        {
            case StateEnum.Finished:
                OnFinished();
                break;
            case StateEnum.Failed:
                OnFailed();
                break;
        }
    }
}
