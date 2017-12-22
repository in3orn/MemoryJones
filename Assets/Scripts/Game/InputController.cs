using UnityEngine;

public class InputController : MonoBehaviour {

    [SerializeField]
    private Game game;

    [SerializeField]
    private float MinSwipeLength = 50.0f;

    private Vector2 start;

    private bool mouseDown = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            start = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            mouseDown = true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            mouseDown = false;

            Vector2 end = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            
            if(Mathf.Abs(end.x - start.x) < MinSwipeLength || Mathf.Abs(end.y - start.y) < MinSwipeLength)
            {
                if (end.x - start.x > MinSwipeLength)
                {
                    game.MoveRight();
                    return;
                }
                if (end.x - start.x < -MinSwipeLength)
                {
                    game.MoveLeft();
                    return;
                }
                if (end.y - start.y > MinSwipeLength)
                {
                    game.MoveUp();
                    return;
                }
                if (end.y - start.y < -MinSwipeLength)
                {
                    game.MoveDown();
                    return;
                }
            }
        }
    }
}
