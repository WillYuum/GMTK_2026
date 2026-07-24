using UnityEngine;

public class MiniGameLauncher : MonoBehaviour
{

    private MiniGame _currentMiniGame;
    private MiniGamePanel _currentMiniGamePanel;

    public void Initialize(MiniGame miniGame, MiniGamePanel miniGamePanel)
    {
        FindAnyObjectByType<CameraController>().ToggleCameraMovement(false);

        _currentMiniGamePanel = miniGamePanel;
        _currentMiniGamePanel.MiniGameHolder.SetActive(true);
        _currentMiniGame = miniGame;
        _currentMiniGame.gameObject.SetActive(true);
    }


    public void LaunchMiniGame()
    {
        _currentMiniGame.IsGameActive = true;
        _currentMiniGame.OnStart();
        _currentMiniGame.OnGameFinished += (isSuccess) =>
        {
            Debug.Log($"MiniGame finished. Success: {isSuccess}");
            EndMiniGame();
        };
    }


    void Update()
    {
        if (_currentMiniGame != null)
        {
            _currentMiniGame.OnUpdate();
        }
    }

    public void EndMiniGame()
    {
        _currentMiniGamePanel.PlaceBackPanel();
        FindAnyObjectByType<CameraController>().ToggleCameraMovement(true);
    }
}
