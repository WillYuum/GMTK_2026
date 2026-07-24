using UnityEngine;

public class GameloopManager : MonoBehaviour
{

    public int StartingCountDownValue { get; private set; } = 90;
    public int CurrentCountDownValue { get; private set; } = 90;


    private float _timer = 1f;


    private CameraController _cameraController;


    [SerializeField] private CountDownTimerController _countDownTimerUI;


    void Awake()
    {
        _cameraController = FindAnyObjectByType<CameraController>();

    }

    public void StartGame()
    {
        Debug.Log($"[GameloopManager] StartGame");
        _countDownTimerUI.SetTime(StartingCountDownValue);
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
            _countDownTimerUI.UpdateTime(CurrentCountDownValue);
        }


        if (CurrentCountDownValue <= 0)
        {
            Debug.Log($"[GameloopManager] Game Over");
            ShowGameEnding();
            return;
        }
    }


    public void ShowGameEnding()
    {
        _cameraController.ToggleCameraMovement(false);

        //pause every possible minigame

        //Start showing the ending screen with rocket flying sequence
    }
}
