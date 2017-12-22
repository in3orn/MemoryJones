using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    [SerializeField]
    private HeartImage[] hearts;

    [SerializeField]
    private Text score;

    [SerializeField]
    private Game game;

    void Start()
    {
        game.OnFinished += updateScore;
        game.OnFailed += hideHeart;
    }

    private void updateScore()
    {
        score.text = (10 * game.GreenGems + game.YellowGems).ToString("# ###");
    }

    private void hideHeart()
    {
        hearts[game.Lives].Hide();
    }
}
