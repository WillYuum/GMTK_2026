using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameEndSequence : MonoBehaviour
{
    [SerializeField] private GameObject _sliderHolder;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _timeBetweenSlides = 3f;

    private GameObject[] _sliders;

    private bool _isPlaying;

    public void PlayEnding()
    {
        if (_isPlaying)
            return;

        _isPlaying = true;

        ReferenceSliders();

        Sequence sequence = DOTween.Sequence();

        foreach (GameObject slide in _sliders)
        {
            CanvasGroup cg = slide.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = slide.AddComponent<CanvasGroup>();

            cg.alpha = 0;
            slide.SetActive(true);

            sequence.Append(cg.DOFade(1, _fadeDuration));
            sequence.AppendInterval(_timeBetweenSlides);
        }

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
}