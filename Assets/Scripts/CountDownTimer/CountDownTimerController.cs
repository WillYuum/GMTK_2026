using UnityEngine;

public class CountDownTimerController : MonoBehaviour
{
    [SerializeField] private CountDownTimerUI _countDownTimerUI;

    public int StartingCountDownValue = 90;
    public int CurrentCountDownValue = 90;
    private float _timer = 1f;


    void Start()
    {
        _countDownTimerUI.SetValue(StartingCountDownValue);
        CurrentCountDownValue = StartingCountDownValue;
    }

    void Update()
    {
        HandleCountDown();
    }

    private void HandleCountDown()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _timer = 1f;
            CurrentCountDownValue--;
            _countDownTimerUI.SetValue(CurrentCountDownValue);
        }
    }
}
