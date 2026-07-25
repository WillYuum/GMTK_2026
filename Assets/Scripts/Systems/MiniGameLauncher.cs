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
            EndMiniGame(isSuccess);
        };
    }


    void Update()
    {
        if (_currentMiniGame != null && _currentMiniGame.IsGameActive)
        {
            _currentMiniGame.OnUpdate();
        }
    }

    public void EndMiniGame(bool isSuccess)
    {
        _currentMiniGame.IsGameActive = false;
        _currentMiniGame.gameObject.SetActive(false);
        _currentMiniGamePanel.PlaceBackPanel(isSuccess);
        FindAnyObjectByType<CameraController>().ToggleCameraMovement(true);

        _currentMiniGame = null;
        _currentMiniGamePanel = null;
    }
}
