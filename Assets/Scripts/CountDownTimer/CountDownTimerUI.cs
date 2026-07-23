using DG.Tweening;
using TMPro;
using UnityEngine;

public class CountDownTimerUI : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _warningColor = Color.red;
    [SerializeField] private int redThreshold = 10;


    private int _currentValue = -1;
    private Vector3 _originalScale;
    private Tween _pulseTween;



    void Awake()
    {
        _originalScale = _countdownText.transform.localScale;
    }

    void OnDisable()
    {
        if (_pulseTween != null)
        {
            _pulseTween.Kill();
        }

        transform.localScale = _originalScale;
    }

    public void SetValue(int seconds)
    {
        seconds = Mathf.Max(0, seconds);

        if (_currentValue == seconds)
        {
            return;
        }

        _currentValue = seconds;

        UpdateDisplay();
    }


    private void UpdateDisplay()
    {
        if (_countdownText == null)
        {
            return;
        }

        int minutes = _currentValue / 60;
        int seconds = _currentValue % 60;

        _countdownText.text = $"{minutes:00}:{seconds:00}";
        _countdownText.color = _currentValue <= redThreshold ? _warningColor : _normalColor;
    }
}