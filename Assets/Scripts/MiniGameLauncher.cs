using UnityEngine;

public class MiniGameLauncher : MonoBehaviour
{

    private MiniGame _currentMiniGame;

    public void LaunchMiniGame()
    {
        _currentMiniGame = new SwipeCardMiniGame();
        _currentMiniGame.StartGame();
    }


    void Update()
    {
        if (_currentMiniGame != null)
        {
            _currentMiniGame.UpdateGame();
        }
    }

    public void EndMiniGame()
    {
        if (_currentMiniGame != null)
        {
            _currentMiniGame.EndGame();
            _currentMiniGame = null;
        }
    }
}


public class SwipeCardMiniGame : MiniGame
{
    public override void StartGame()
    {
        // Start the swipe card mini-game
    }

    public override void EndGame()
    {
        // End the swipe card mini-game
    }

    public override void UpdateGame()
    {
        // Update the swipe card mini-game logic
    }
}

