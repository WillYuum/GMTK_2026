using UnityEngine;

public class MiniGameLauncher : MonoBehaviour
{

    private MiniGame _currentMiniGame;

    public void LaunchMiniGame()
    {
        // _currentMiniGame = new SwipeCardMiniGame();
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
