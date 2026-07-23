using UnityEngine;

public class SwipeCardMiniGame : MiniGame
{
    [Header("References")]
    [SerializeField] private Transform _cardPrefab;
    [SerializeField] private Transform _cardScanner;

    private Camera _camera;

    [Header("Swipe Settings")]
    [SerializeField] private float _completionDistance = 0.15f;

    private Vector3 _startingPosition;
    private Vector3 _dragOffset;

    private Plane _dragPlane;

    private bool _isDragging;
    private bool _isPlaying;
    private bool _isCompleted;

    public override void StartGame()
    {
        if (_cardPrefab == null || _cardScanner == null)
        {
            Debug.LogError("Swipe card references have not been assigned.");
            return;
        }

        _camera = Camera.main;


        _startingPosition = _cardPrefab.position;

        // The card will move on a plane facing the camera.
        _dragPlane = new Plane(
            -_camera.transform.forward,
            _cardPrefab.position
        );

        _cardPrefab.position = _startingPosition;

        _isDragging = false;
        _isCompleted = false;
        _isPlaying = true;
    }

    public override void UpdateGame()
    {
        if (!_isPlaying || _isCompleted)
        {
            return;
        }

        if (TryGetPointerDown(out Vector2 pointerDownPosition))
        {
            TryStartDragging(pointerDownPosition);
        }

        if (_isDragging && TryGetPointerPosition(out Vector2 pointerPosition))
        {
            MoveCard(pointerPosition);

            if (HasReachedScanner())
            {
                CompleteSwipe();
                return;
            }
        }

        if (_isDragging && IsPointerReleased())
        {
            StopDragging();
        }
    }

    public override void EndGame()
    {
        _isDragging = false;
        _isPlaying = false;
        _isCompleted = true;

        Debug.Log("Card swipe completed.");

        // Add your MiniGame completion logic here.
        // For example:
        // base.EndGame();
    }

    private void TryStartDragging(Vector2 screenPosition)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        // The card requires a Collider or Collider2D.
        if (!DidPressCard(ray, screenPosition))
        {
            return;
        }

        if (!TryGetWorldPosition(screenPosition, out Vector3 pointerWorldPosition))
        {
            return;
        }

        _dragOffset = _cardPrefab.position - pointerWorldPosition;
        _isDragging = true;
    }

    private bool DidPressCard(Ray ray, Vector2 screenPosition)
    {
        // Check 3D colliders.
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == _cardPrefab ||
                hit.transform.IsChildOf(_cardPrefab))
            {
                return true;
            }
        }

        // Check 2D colliders.
        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);

        if (hit2D.collider != null)
        {
            Transform hitTransform = hit2D.transform;

            if (hitTransform == _cardPrefab ||
                hitTransform.IsChildOf(_cardPrefab))
            {
                return true;
            }
        }

        return false;
    }

    private void MoveCard(Vector2 screenPosition)
    {
        if (!TryGetWorldPosition(screenPosition, out Vector3 pointerWorldPosition))
        {
            return;
        }

        Vector3 desiredPosition = pointerWorldPosition + _dragOffset;

        Vector3 swipeDirection =
            (_cardScanner.position - _startingPosition).normalized;

        float swipeLength =
            Vector3.Distance(_startingPosition, _cardScanner.position);

        // Project the card onto the swipe path.
        float distanceAlongSwipe = Vector3.Dot(
            desiredPosition - _startingPosition,
            swipeDirection
        );

        distanceAlongSwipe = Mathf.Clamp(
            distanceAlongSwipe,
            0f,
            swipeLength
        );

        _cardPrefab.position =
            _startingPosition + swipeDirection * distanceAlongSwipe;
    }

    private bool HasReachedScanner()
    {
        return Vector3.Distance(
            _cardPrefab.position,
            _cardScanner.position
        ) <= _completionDistance;
    }

    private void CompleteSwipe()
    {
        _cardPrefab.position = _cardScanner.position;
        EndGame();
    }

    private void StopDragging()
    {
        _isDragging = false;

        // Return the card to the beginning when released too early.
        _cardPrefab.position = _startingPosition;
    }

    private bool TryGetWorldPosition(
        Vector2 screenPosition,
        out Vector3 worldPosition)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (_dragPlane.Raycast(ray, out float distance))
        {
            worldPosition = ray.GetPoint(distance);
            return true;
        }

        worldPosition = default;
        return false;
    }

    private bool TryGetPointerDown(out Vector2 screenPosition)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                screenPosition = touch.position;
                return true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            screenPosition = Input.mousePosition;
            return true;
        }

        screenPosition = default;
        return false;
    }

    private bool TryGetPointerPosition(out Vector2 screenPosition)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase != TouchPhase.Ended &&
                touch.phase != TouchPhase.Canceled)
            {
                screenPosition = touch.position;
                return true;
            }
        }

        if (Input.GetMouseButton(0))
        {
            screenPosition = Input.mousePosition;
            return true;
        }

        screenPosition = default;
        return false;
    }

    private bool IsPointerReleased()
    {
        if (Input.touchCount > 0)
        {
            TouchPhase phase = Input.GetTouch(0).phase;

            return phase == TouchPhase.Ended ||
                   phase == TouchPhase.Canceled;
        }

        return Input.GetMouseButtonUp(0);
    }
}