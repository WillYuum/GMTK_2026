using UnityEngine;
using DG.Tweening;
using System;


public enum MiniGamePanelState
{
    Warning,
    Solved,
}

[RequireComponent(typeof(BoxCollider2D))]
public class MiniGamePanel : MonoBehaviour, IHoverInteractable
{
    [SerializeField] public GameObject MiniGameHolder;
    [SerializeField] private SpriteRenderer _frontPanel;
    [SerializeField] private SpriteRenderer _BackPanel;
    [SerializeField] private SpriteRenderer _taskCompletedCheckmark;
    [SerializeField] private SpriteRenderer _taskWarningIcon;

    public PointerDisplayType PointerType => PointerDisplayType.ToolPointer;

    private BoxCollider2D _boxCollider;
    private Vector3 _initialLocalPosition;


    [SerializeField] private Transform _screwsHolder;


    public bool IsRemoved { get; private set; } = false;
    public MiniGame MiniGameInstance => MiniGameHolder.GetComponentInChildren<MiniGame>();

    void Awake()
    {
        _initialLocalPosition = _frontPanel.transform.localPosition;
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        _boxCollider.size = new Vector2(_frontPanel.localBounds.size.x, _frontPanel.localBounds.size.y);
        _boxCollider.offset = new Vector2(_frontPanel.transform.localPosition.x, _frontPanel.transform.localPosition.y);

        MiniGameHolder.SetActive(false);
        _frontPanel.transform.parent.gameObject.SetActive(true); //incase it was disabled in the scene editor

        _taskCompletedCheckmark.gameObject.SetActive(false);
    }

    public void SetState(MiniGamePanelState state)
    {
        switch (state)
        {
            case MiniGamePanelState.Warning:
                _taskWarningIcon.gameObject.SetActive(true);
                _taskCompletedCheckmark.gameObject.SetActive(false);

                // TODO wait until AudioManager is initialized
                // AudioManager.Instance.PlaySFX("Malfunction");

                break;
            case MiniGamePanelState.Solved:
                _taskWarningIcon.gameObject.SetActive(false);
                _taskCompletedCheckmark.gameObject.SetActive(true);

                // TODO figure out why this doesn't trigger
                AudioManager.Instance.PlaySFX("CompleteMinigame");

                break;
        }
    }

    public void RemovePanel(Action callback)
    {
        IsRemoved = true;
        _boxCollider.enabled = false;

        Sequence seq = DOTween.Sequence();

        float x = UnityEngine.Random.Range(-18f, 18f);


        _taskWarningIcon.DOFade(0, 0.2f);

        AudioManager.Instance.PlaySFX("RemovePanel");

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


    public void PlaceBackPanel(bool isSuccess = false)
    {
        IsRemoved = false;

        _frontPanel.DOKill();
        _frontPanel.transform.DOKill();

        if (isSuccess)
        {
            _taskCompletedCheckmark.gameObject.SetActive(false);
        }

        _frontPanel.DOFade(1f, 0.2f);
        _frontPanel.transform
            .DOLocalMove(_initialLocalPosition, 0.25f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                if (isSuccess)
                {
                    _taskCompletedCheckmark.gameObject.SetActive(true);

                    _taskCompletedCheckmark.color = new Color(1, 1, 1, 0);
                    _taskCompletedCheckmark.transform.localScale = new Vector3(1.5f, 0.5f, 1f);
                    _taskCompletedCheckmark.transform.localRotation = Quaternion.Euler(0, 0, -8f);

                    Sequence seq = DOTween.Sequence();

                    seq.Append(_taskCompletedCheckmark.DOFade(1f, 0.04f));

                    seq.Join(
                        _taskCompletedCheckmark.transform
                            .DOScale(Vector3.one, 0.15f)
                            .SetEase(Ease.OutQuad)
                    );

                    seq.Join(
                        _taskCompletedCheckmark.transform
                            .DOLocalRotate(Vector3.zero, 0.15f)
                            .SetEase(Ease.OutQuad)
                    );

                    seq.Append(
                        _taskCompletedCheckmark.transform.DOPunchPosition(Vector3.down * 1.5f, 0.08f, 1, 0)
                    );

                    ReAddAllScrews();

                    seq.OnComplete(() =>
                    {
                        _taskCompletedCheckmark.DOFade(0, 0.2f).SetDelay(0.5f);
                    });
                }
            });
    }



    [SerializeField] private float _readdDelay = 0.1f;

    private void ReAddAllScrews()
    {
        Sequence sequence = DOTween.Sequence();

        foreach (Transform screw in _screwsHolder)
        {
            if (screw.TryGetComponent<Screw>(out var screwComponent))
            {
                sequence.AppendCallback(() => screwComponent.ReaddScrew());
                sequence.AppendInterval(_readdDelay);
            }
        }
    }

    public bool CheckIsLocked()
    {
        bool isLocked = false;

        for (int i = 0; i < _screwsHolder.childCount; i++)
        {
            if (_screwsHolder.GetChild(i).TryGetComponent<BoxCollider2D>(out var screwComponent))
            {
                if (screwComponent.enabled == true)
                {
                    isLocked = true;
                    break;
                }
            }
        }

        return isLocked;
    }

    public Rect GetBackPanelRect()
    {
        Bounds bounds = _BackPanel.bounds;
        return new Rect(bounds.min.x, bounds.min.y, bounds.size.x, bounds.size.y);
    }

    public void OnHoverEnter()
    {

    }

    public void OnHoverExit()
    {

    }
}
