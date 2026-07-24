using UnityEngine;
using UnityEngine.SceneManagement;

public class GameloopManager : MonoBehaviour
{

    [field: SerializeField] public int StartingCountDownValue { get; private set; } = 90;
    public int CurrentCountDownValue { get; private set; } = 90;


    private float _timer = 1f;


    private CameraController _cameraController;


    [SerializeField] private CountDownTimerController _countDownTimerController;


    void Awake()
    {
        _cameraController = FindAnyObjectByType<CameraController>();
    }

    public void PrepareLoop()
    {
        _cameraController.ToggleCameraMovement(false);
    }


    public void StartGame()
    {
        Debug.Log($"[GameloopManager] StartGame");
        _countDownTimerController.SetTime(StartingCountDownValue);
        CurrentCountDownValue = StartingCountDownValue;

        _cameraController.ToggleCameraMovement(true);
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
        GameEndSequence gameEndSequence = FindAnyObjectByType<GameEndSequence>();

        if (scene.IsValid() && scene.isLoaded)
        {
            gameEndSequence.PlayEnding();
        }
        else
        {
            new SceneAdditiveLoader().LoadSceneAdditive(sceneName, () =>
            {
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
