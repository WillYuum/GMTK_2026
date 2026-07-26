using UnityEngine;
using DG.Tweening;

public class Screw : MonoBehaviour, IHoverInteractable
{
    [SerializeField] private SpriteRenderer _screwSpriteRenderer;

    private int _rotationCount;
    private const int MaxRotations = 5;

    public PointerDisplayType PointerType => PointerDisplayType.ToolScrewdriver;

    private bool _isRotating = false;

    public bool Rotate()
    {
        if (_isRotating)
        {
            return false;
        }

        if (_rotationCount >= MaxRotations)
        {
            return true;
        }

        _isRotating = true;
        _rotationCount++;

        AudioManager.Instance.PlaySFX("RotateScrew");

        _screwSpriteRenderer.transform
            .DOLocalRotate(
                new Vector3(0, 0, 360f),
                0.5f,
                RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                _isRotating = false;
            });

        if (_rotationCount >= MaxRotations)
        {
            PlayRemoveScrew();
            AudioManager.Instance.PlaySFX("RemoveScrew");

            return true;
        }

        return false;
    }
    private void PlayRemoveScrew()
    {
        _screwSpriteRenderer
            .DOFade(0, 0.3f)
            .OnComplete(() =>
            {
                GetComponent<BoxCollider2D>().enabled = false;
            });
    }

    public void ReaddScrew()
    {
        _screwSpriteRenderer.color = new Color(1f, 1f, 1f, 0f);
        _screwSpriteRenderer.transform.localRotation = Quaternion.Euler(0f, 0f, -360f);

        DOTween.Sequence()
            .Append(_screwSpriteRenderer.DOFade(1f, 0.3f))
            .Append(_screwSpriteRenderer.transform.DOLocalRotate(
                Vector3.zero,
                0.8f,
                RotateMode.FastBeyond360))
            .SetEase(Ease.OutCubic);
    }

    public void OnHoverEnter()
    {

    }

    public void OnHoverExit()
    {

    }
}