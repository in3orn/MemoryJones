using UnityEngine;

public class Game : MonoBehaviour
{
    public delegate void FinishedAction();
    public event FinishedAction OnFinished;

    public delegate void FailedAction();
    public event FailedAction OnFailed;

    [SerializeField]
    private Level level;

    [SerializeField]
    private int lives = 3;

    [SerializeField]
    private int minLevelDifficulty = 10;

    [SerializeField]
    private int failedLevelDrop = 3;

    private int redGems;

    private int greenGems;

    private int yellowGems;

    private int currentLevel;

    private int currentDifficulty;

    [SerializeField]
    PopUpCanvas gameOverCanvas;

    public int GreenGems
    {
        get { return greenGems; }
    }

    public int YellowGems
    {
        get { return yellowGems; }
    }

    public int Lives
    {
        get { return lives; }
    }

    void Start()
    {
        level.OnFinished += StartNextLevel;
        level.OnFailed += CancelLevel;
        startLevel();
    }

    public void StartNextLevel()
    {
        greenGems++;
        currentLevel++;
        currentDifficulty++;
        startLevel();
        
        OnFinished();
    }

    public void CancelLevel()
    {
        lives--;
        if (lives > 0)
        {
            currentDifficulty = Mathf.Max(minLevelDifficulty, currentDifficulty - failedLevelDrop);
            startLevel();
        }
        else
        {
            //TODO wait 18h for next live
            gameOverCanvas.Show();
        }
        
        OnFailed();
    }

    private void startLevel()
    {
        level.Init(currentLevel);
    }

    public void MoveLeft()
    {
        level.MoveLeft();
    }

    public void MoveRight()
    {
        level.MoveRight();
    }

    public void MoveUp()
    {
        level.MoveUp();
    }

    public void MoveDown()
    {
        level.MoveDown();
    }
}
