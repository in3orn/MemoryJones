using UnityEngine;
using System.Collections;

public class Arrows : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer upArrow;

    [SerializeField]
    private SpriteRenderer downArrow;

    [SerializeField]
    private SpriteRenderer leftArrow;

    [SerializeField]
    private SpriteRenderer rightArrow;

    [SerializeField]
    private Level level;

    void Update()
    {
        setVisible(upArrow, level.CanMoveUp());
        setVisible(downArrow, level.CanMoveDown());
        setVisible(leftArrow, level.CanMoveLeft());
        setVisible(rightArrow, level.CanMoveRight());
    }

    private void setVisible(SpriteRenderer arrow, bool visible)
    {
        if(visible && arrow.color.a == 0)
        {
            arrow.color = new Color(arrow.color.r, arrow.color.g, arrow.color.b, 1);
        }
        else if(!visible && arrow.color.a == 1)
        {
            arrow.color = new Color(arrow.color.r, arrow.color.g, arrow.color.b, 0);
        }
        
    }
}
