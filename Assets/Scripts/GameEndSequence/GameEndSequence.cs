using AudioClasses;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndSequence : MonoBehaviour
{
    [SerializeField] private GameObject _sliderHolder;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _timeBetweenSlides = 3f;


    [SerializeField] private TextMeshProUGUI _finishedCountText;

    [SerializeField] private Image _banner;

    [SerializeField] private GameObject _allTexts;

    [SerializeField] private Transform _rocketTransform;
    [SerializeField] private Transform _rocketStartPoint;
    [SerializeField] private Transform _rocketEndPoint;



    [SerializeField] private SequenceOfFrames _sequences;




    private GameObject[] _sliders;


    private GameloopManager _gameloopManager;

    private bool _isPlaying;

    public void PlayEnding()
    {
        if (_isPlaying)
            return;

        _gameloopManager = FindAnyObjectByType<GameloopManager>();
        int finishedCount = _gameloopManager.MiniGameFinishedTracker.MiniGameFinished;
        int totalCount = _gameloopManager.MiniGameFinishedTracker.MiniGameCounts;

        _isPlaying = true;

        // ReferenceSliders();

        Sequence sequence = DOTween.Sequence();

        // _banner.color = new Color(_banner.color.r, _banner.color.g, _banner.color.b, 0);
        // sequence.Append(_banner.DOFade(1, 0.65f));

        // foreach (GameObject slide in _sliders)
        // {
        //     CanvasGroup cg = slide.GetComponent<CanvasGroup>();
        //     if (cg == null)
        //         cg = slide.AddComponent<CanvasGroup>();

        //     cg.alpha = 0;
        //     slide.SetActive(true);

        //     sequence.Append(cg.DOFade(1, _fadeDuration));
        //     sequence.AppendInterval(_timeBetweenSlides);
        // }

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
        // sequence.AppendCallback(() =>
        // {
        //     _finishedCountText.DOFade(1, _fadeDuration);
        // });




        sequence.OnComplete(() =>
        {
            _isPlaying = false;
            EndingFinished();
        });
    }
    private void ReferenceSliders()
    {
        _sliders = new GameObject[_sliderHolder.transform.childCount];

        for (int i = 0; i < _sliders.Length; i++)
        {
            _sliders[i] = _sliderHolder.transform.GetChild(i).gameObject;
            _sliders[i].SetActive(false);
        }
    }

    private void EndingFinished()
    {
        Debug.Log("Ending Finished");
    }



    private Tween TweenRocket()
    {
        _rocketTransform.position = _rocketStartPoint.position;
        return _rocketTransform.DOMove(_rocketEndPoint.position, 2f).SetEase(Ease.InOutSine);
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