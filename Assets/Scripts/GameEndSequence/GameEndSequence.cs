using AudioClasses;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameEndSequence : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _timeBetweenSlides = 3f;


    private bool _canContinue;

    [SerializeField] private TextMeshProUGUI _finishedCountText;

    [SerializeField] private Image _banner;

    [SerializeField] private GameObject _allTexts;

    [SerializeField] private Transform _rocketTransform;
    [SerializeField] private Transform _rocketStartPoint;
    [SerializeField] private Transform _rocketEndPoint;



    [SerializeField] private SequenceOfFrames _sequences;



    private GameloopManager _gameloopManager;

    private bool _isPlaying;


    void Update()
    {
        if (!_canContinue)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _canContinue = false;
            HandlePressedRestart();
        }
    }

    public void PlayEnding()
    {
        if (_isPlaying)
            return;

        _gameloopManager = FindAnyObjectByType<GameloopManager>();
        int finishedCount = _gameloopManager.MiniGameFinishedTracker.MiniGameFinished;
        int totalCount = _gameloopManager.MiniGameFinishedTracker.MiniGameCounts;

        _isPlaying = true;

        Sequence sequence = DOTween.Sequence();

        _sequences.Start();
        AudioManager.Instance.PlaySFX("Rocket_Launch");
        sequence.Append(TweenRocket());

        sequence.AppendCallback(() =>
        {
            AudioManager.Instance.PlayBGM("title_bgm");
        });


        for (int i = 0; i < _sequences.Frames.Length; i++)
        {
            sequence.AppendCallback(() => _sequences.ShowNextFrame());
            sequence.AppendInterval(_sequences.DurationToNextFrame);
        }


        _finishedCountText.text = $"{finishedCount}";
        for (int i = 0; i < _allTexts.transform.childCount; i++)
        {
            TextMeshProUGUI text = _allTexts.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            text.alpha = 0;
            sequence.Append(text.DOFade(1, _fadeDuration));
            sequence.AppendInterval(0.25f);
        }


        sequence.OnComplete(() =>
        {
            EndingFinished();
        });
    }

    private void EndingFinished()
    {
        Debug.Log("Ending Finished");
        _isPlaying = false;
        _canContinue = true;
    }



    private Tween TweenRocket()
    {
        _rocketTransform.position = _rocketStartPoint.position;
        return _rocketTransform.DOMove(_rocketEndPoint.position, 2f).SetEase(Ease.InOutSine);
    }


    private void HandlePressedRestart()
    {
        FindAnyObjectByType<GameManager>().RestartGame(true);
    }
}


[System.Serializable]
public class SequenceOfFrames
{
    [field: SerializeField] public GameObject[] Frames { get; private set; }
    public float DurationToNextFrame { get; private set; } = 1f;

    public int CurrentFrameIndex { get; private set; }

    public void Start()
    {
        //Hide all frames
        for (int i = 0; i < Frames.Length; i++)
        {
            Frames[i].SetActive(false);
        }
    }

    public void ShowNextFrame()
    {
        if (CurrentFrameIndex < Frames.Length)
        {
            Frames[CurrentFrameIndex].SetActive(true);
            CurrentFrameIndex++;
        }
    }
}