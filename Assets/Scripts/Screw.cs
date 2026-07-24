using UnityEngine;
using DG.Tweening;

public class Screw : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _screwSpriteRenderer;

    private int _rotationCount;
    private const int MaxRotations = 5;

    public bool Rotate()
    {
        if (_rotationCount >= MaxRotations)
            return true;

        _rotationCount++;

        _screwSpriteRenderer.transform
            .DOLocalRotate(
                new Vector3(0, 0, 360f),
                0.5f,
                RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuad);

        if (_rotationCount >= MaxRotations)
        {
            PlayRemoveScrew();
            return true;
        }

        return false;
    }

    private void PlayRemoveScrew()
    {
        _screwSpriteRenderer
            .DOFade(0, 0.3f)
            .OnComplete(() => Destroy(gameObject));
    }
}