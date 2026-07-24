using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;

public class MiniGamePanel : MonoBehaviour
{
    [SerializeField] public GameObject MiniGameHolder;
    [SerializeField] private SpriteRenderer _frontPanel;
    private Vector3 _initialLocalPosition;

    public bool IsRemoved { get; private set; } = false;
    public MiniGame MiniGameInstance => MiniGameHolder.GetComponentInChildren<MiniGame>();

    void Awake()
    {
        _initialLocalPosition = _frontPanel.transform.localPosition;
    }

    void Start()
    {
        MiniGameHolder.SetActive(false);
    }

    public void RemovePanel(Action callback)
    {
        IsRemoved = true;

        Sequence seq = DOTween.Sequence();

        float x = UnityEngine.Random.Range(-18f, 18f);

        seq.Append(
            _frontPanel.transform.DOLocalMove(
                _initialLocalPosition + new Vector3(x * 0.2f, 48f, 0),
                0.18f)
            .SetEase(Ease.OutQuad)
        );

        seq.Append(
            _frontPanel.transform.DOLocalMove(
                _initialLocalPosition + new Vector3(x, -126f, 0),
                0.55f)
            .SetEase(Ease.InQuad)
        );

        seq.Join(
            _frontPanel.DOFade(0, 0.3f)
                .SetDelay(0.4f)
        );

        seq.OnComplete(() =>
        {
            callback?.Invoke();
        });
        seq.Play();
    }


    public void PlaceBackPanel()
    {
        IsRemoved = false;

        _frontPanel.DOKill();
        _frontPanel.transform.DOKill();

        _frontPanel.DOFade(1f, 0.2f);
        _frontPanel.transform
            .DOLocalMove(_initialLocalPosition, 0.25f)
            .SetEase(Ease.OutBack);
    }

}
