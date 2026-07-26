using UnityEngine;

public class MiniGameLauncher : MonoBehaviour
{

    private MiniGame _currentMiniGame;
    private MiniGamePanel _currentMiniGamePanel;
    private GameloopManager _gameloopManager;

    private InteractWithMiniGamesController _interactWithMiniGamesController;

    void Awake()
    {
        _gameloopManager = FindAnyObjectByType<GameloopManager>();
        _interactWithMiniGamesController = FindAnyObjectByType<InteractWithMiniGamesController>();
    }

    public void Initialize(MiniGame miniGame, MiniGamePanel miniGamePanel)
    {
        FindAnyObjectByType<CameraController>().ToggleCameraMovement(false);

        _interactWithMiniGamesController.enabled = false;

        _currentMiniGamePanel = miniGamePanel;
        _currentMiniGamePanel.MiniGameHolder.SetActive(true);
        _currentMiniGame = miniGame;
        _currentMiniGame.gameObject.SetActive(true);

        miniGame.BackPanelRect = _currentMiniGamePanel.GetBackPanelRect();

        miniGame.OnInitialize();
    }


    public void LaunchMiniGame()
    {
        _currentMiniGame.IsGameActive = true;
        _currentMiniGame.OnStart();
        _currentMiniGame.OnGameFinished += (isSuccess) =>
        {
            Debug.Log($"MiniGame finished. Success: {isSuccess}");
            _ = EndMiniGame(isSuccess);
        };
    }


    void Update()
    {
        if (_currentMiniGame != null && _currentMiniGame.IsGameActive)
        {
            _currentMiniGame.OnUpdate();
        }
    }


    public async Awaitable EndMiniGame(bool isSuccess)
    {
        _currentMiniGame.IsGameActive = false;
        FindAnyObjectByType<CameraController>().ToggleCameraMovement(true);

        if (isSuccess)
        {
            _gameloopManager.NotifyMiniGameFinished();
        }

        await Awaitable.WaitForSecondsAsync(1.0f);

        _currentMiniGame.gameObject.SetActive(false);
        _currentMiniGamePanel.PlaceBackPanel(isSuccess);

        _interactWithMiniGamesController.enabled = true;

        _currentMiniGame = null;
        _currentMiniGamePanel = null;
    }
}
