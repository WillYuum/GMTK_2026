using UnityEngine;

public class MiniGameLauncher : MonoBehaviour
{

    private MiniGame _currentMiniGame;

    public void InitializeMiniGame(MiniGame miniGame)
    {
        _currentMiniGame = miniGame;
    }

    public void LaunchMiniGame()
    {
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
