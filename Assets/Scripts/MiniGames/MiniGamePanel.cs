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

        Vector3 start = _frontPanel.transform.localPosition;
        float xOffset = UnityEngine.Random.Range(-0.25f, 0.25f);

        Vector3[] path =
        {
            start,
            // Up
            start + new Vector3(xOffset * 0.4f, 0.55f, 0f), 
            // Down + diagonal
            start + new Vector3(xOffset, -1.2f, 0f)
        };

        _frontPanel.transform
            .DOLocalPath(path, 0.8f, PathType.CatmullRom)
            .SetEase(Ease.InQuart);

        _frontPanel
            .DOFade(0f, 0.45f)
            .SetDelay(0.6f)
            .OnComplete(() =>
            {
                callback?.Invoke();
            });
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
