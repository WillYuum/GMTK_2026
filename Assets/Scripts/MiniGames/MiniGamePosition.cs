using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class MiniGamePosition : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _frontPanel;
    private Vector3 _initialLocalPosition;

    public bool IsRemoved { get; private set; } = false;

    void Awake()
    {
        _initialLocalPosition = _frontPanel.transform.localPosition;
    }

    void Update()
    {
        Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        //if mouse hover
        var hits = Physics2D.Raycast(mousePosInWorld, Vector2.zero);

        if (hits.collider != null && hits.collider.TryGetComponent<MiniGamePosition>(out MiniGamePosition miniGamePosition))
        {
            bool isClicked = Mouse.current.leftButton.wasPressedThisFrame;
            if (isClicked && miniGamePosition.IsRemoved == false)
            {
                miniGamePosition.OnMouseClick();
            }
        }

    }


    private void OnMouseClick()
    {
        IsRemoved = true;

        Vector3 start = _frontPanel.transform.localPosition;
        float xOffset = Random.Range(-0.25f, 0.25f);

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
            .SetDelay(0.6f);
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
