using UnityEngine;

public class CountDownTimerController : MonoBehaviour
{
    [SerializeField] private CountDownTimerUI _countDownTimerUI;

    public int CurrentCountDownValue { get; private set; } = 90;


    public void SetTime(int newTime)
    {
        CurrentCountDownValue = newTime;
        _countDownTimerUI.SetValue(CurrentCountDownValue);
    }

    public void UpdateTime(int newTime)
    {
        CurrentCountDownValue = newTime;
        _countDownTimerUI.SetValue(CurrentCountDownValue);
    }
}
