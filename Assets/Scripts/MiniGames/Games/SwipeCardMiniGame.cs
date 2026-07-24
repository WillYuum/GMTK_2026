using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeCardMiniGame : MiniGame
{
    [Header("References")]
    [SerializeField] private Collider2D _hitboxArea;


    private int _targetCount = 10;

    private Camera _camera;
    private int _count;


    public override void OnStart()
    {
        if (_hitboxArea == null)
        {
            Debug.LogError("The Hitbox Area has not been assigned.");
            return;
        }

        _camera = Camera.main;

        _count = 0;

        Debug.Log($"Counter started. Count: {_count}");
    }

    public override void OnUpdate()
    {


        if (!TryGetPointerDown(out Vector2 screenPosition))
        {
            return;
        }

        if (IsPointerInsideHitbox(screenPosition))
        {
            IncrementCounter();
        }
    }

    public override void OnEnd()
    {

        Debug.Log($"Counter game completed. Final count: {_count}");
    }

    private void IncrementCounter()
    {
        _count++;

        Debug.Log($"Count: {_count}");

        if (_targetCount > 0 && _count >= _targetCount)
        {
            TriggerFinishedGame(true);
        }
    }

    private bool IsPointerInsideHitbox(Vector2 screenPosition)
    {
        Vector3 worldPosition = _camera.ScreenToWorldPoint(
            new Vector3(
                screenPosition.x,
                screenPosition.y,
                Mathf.Abs(_camera.transform.position.z -
                          _hitboxArea.transform.position.z)
            )
        );

        return _hitboxArea.OverlapPoint(worldPosition);
    }

    private bool TryGetPointerDown(out Vector2 screenPosition)
    {
        if (Pointer.current != null &&
            Pointer.current.press.wasPressedThisFrame)
        {
            screenPosition = Pointer.current.position.ReadValue();
            return true;
        }

        screenPosition = default;
        return false;
    }
}