using UnityEngine;
using UnityEngine.SceneManagement;

public class GameloopManager : MonoBehaviour
{

    [field: SerializeField] public int StartingCountDownValue { get; private set; } = 90;
    public int CurrentCountDownValue { get; private set; } = 90;


    private float _timer = 1f;


    private CameraController _cameraController;


    [SerializeField] private CountDownTimerController _countDownTimerController;

    public MiniGamePanel[] ListOfMiniGames { get; private set; }


    void Awake()
    {
        _cameraController = FindAnyObjectByType<CameraController>();
        ListOfMiniGames = FindObjectsByType<MiniGamePanel>();
    }

    public void PrepareLoop()
    {
        enabled = false;
        _cameraController.ToggleCameraMovement(false);
    }


    public void StartGame()
    {
        enabled = true;
        Debug.Log($"[GameloopManager] StartGame");
        _countDownTimerController.SetTime(StartingCountDownValue);
        CurrentCountDownValue = StartingCountDownValue;

        _cameraController.ToggleCameraMovement(true);

        foreach (var miniGame in ListOfMiniGames)
        {
            miniGame.SetState(MiniGamePanelState.Warning);
        }
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _timer = 1f;
            CurrentCountDownValue--;
            _countDownTimerController.UpdateTime(CurrentCountDownValue);
        }


        if (CurrentCountDownValue <= 0)
        {
            Debug.Log($"[GameloopManager] Game Over");
            ShowGameEnding();
            enabled = false;
            return;
        }
    }




    public void ShowGameEnding()
    {
        _cameraController.ToggleCameraMovement(false);

        string sceneName = "GameEndSequence";

        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (scene.IsValid() && scene.isLoaded)
        {
            GameEndSequence gameEndSequence = FindAnyObjectByType<GameEndSequence>();
            gameEndSequence.PlayEnding();
        }
        else
        {
            new SceneAdditiveLoader().LoadSceneAdditive(sceneName, () =>
            {
                GameEndSequence gameEndSequence = FindAnyObjectByType<GameEndSequence>();
                gameEndSequence.PlayEnding();
            });
        }

        // Pause every possible minigame

        // Start showing the ending screen with rocket flying sequence
    }
}



class SceneAdditiveLoader
{
    //Load and wait with callback a scene additive
    public void LoadSceneAdditive(string sceneName, System.Action callback)
    {
        var loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (callback != null)
        {
            loadOperation.completed += _ => callback();
        }
    }
}
