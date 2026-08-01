using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum PointerDisplayType
{
    Default,
    ToolPointer,
    ToolHand,
    ToolBroom,
    ToolScrewdriver,
    ToolWrench,
}

public interface IHoverInteractable
{
    PointerDisplayType PointerType { get; }

    void OnHoverEnter();
    void OnHoverExit();
}

public class PointerDisplaySystem : MonoBehaviour
{
    [System.Serializable]
    public struct PointerData
    {
        public PointerDisplayType Type;
        public Sprite Sprite;
        [Tooltip("Additional manual tweak on top of automatic baseline alignment")]
        public Vector2 FineTuneOffset;
    }

    [Header("Cursor Settings")]
    [SerializeField] private SpriteRenderer _cursorRenderer;
    [SerializeField] private PointerData[] _pointers;

#if UNITY_EDITOR
    [SerializeField] private bool _debugShowSystemCursor;
    private bool _lastCursorVisible;
#endif

    private Camera _camera;
    private IHoverInteractable _currentHover;
    private PointerDisplayType _currentType = PointerDisplayType.Default;

    void Awake()
    {
        _camera = Camera.main;

#if UNITY_EDITOR
        UpdateEditorCursorVisibility();
#else
        Cursor.visible = false;
#endif
    }

    void Start()
    {
        SetDefaultPointer();
    }

    void LateUpdate()
    {
#if UNITY_EDITOR
        UpdateEditorCursorVisibility();
#endif

        UpdateCursorPosition();
        UpdateHover();
    }

#if UNITY_EDITOR
    private void UpdateEditorCursorVisibility()
    {
        bool showCursor =
            _debugShowSystemCursor ||
            EditorWindow.mouseOverWindow == null ||
            EditorWindow.mouseOverWindow.GetType().Name != "GameView";

        if (showCursor == _lastCursorVisible)
        {
            return;
        }

        Cursor.visible = showCursor;
        _lastCursorVisible = showCursor;
    }
#endif

    private void UpdateCursorPosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = -_camera.transform.position.z;

        Vector3 worldPos = _camera.ScreenToWorldPoint(mousePosition);

        Vector2 autoOffset = GetAutomaticOffset(_currentType);
        Vector2 manualOffset = GetCurrentManualOffset(_currentType);

        worldPos.x += autoOffset.x + manualOffset.x;
        worldPos.y += autoOffset.y + manualOffset.y;

        _cursorRenderer.transform.position = worldPos;
    }

    private Vector2 GetAutomaticOffset(PointerDisplayType type)
    {
        if (_cursorRenderer.sprite == null)
        {
            return Vector2.zero;
        }

        Vector2 spriteSize = _cursorRenderer.sprite.bounds.size;
        Vector2 pivot = _cursorRenderer.sprite.pivot / _cursorRenderer.sprite.rect.size;

        float yOffset = -(spriteSize.y * (1f - pivot.y));
        float xOffset;

        switch (type)
        {
            case PointerDisplayType.ToolScrewdriver:
            case PointerDisplayType.ToolBroom:
                xOffset = spriteSize.x * (0.5f - pivot.x);
                break;

            case PointerDisplayType.Default:
            case PointerDisplayType.ToolPointer:
            case PointerDisplayType.ToolHand:
            case PointerDisplayType.ToolWrench:
            default:
                xOffset = -(spriteSize.x * pivot.x);
                break;
        }

        return new Vector2(xOffset, yOffset);
    }

    private Vector2 GetCurrentManualOffset(PointerDisplayType type)
    {
        for (int i = 0; i < _pointers.Length; i++)
        {
            if (_pointers[i].Type == type)
            {
                return _pointers[i].FineTuneOffset;
            }
        }

        return Vector2.zero;
    }

    private void UpdateHover()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        IHoverInteractable newHover = null;
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            hit.collider.TryGetComponent(out newHover);
        }

        if (_currentHover == newHover)
        {
            return;
        }

        _currentHover?.OnHoverExit();
        _currentHover = newHover;

        if (_currentHover != null)
        {
            SetPointer(_currentHover.PointerType);
            _currentHover.OnHoverEnter();
        }
        else
        {
            SetDefaultPointer();
        }
    }

    public void SetPointer(PointerDisplayType type)
    {
        _currentType = type;

        for (int i = 0; i < _pointers.Length; i++)
        {
            if (_pointers[i].Type == type)
            {
                _cursorRenderer.sprite = _pointers[i].Sprite;
                return;
            }
        }

        SetDefaultPointer();
    }

    public void SetDefaultPointer()
    {
        SetPointer(PointerDisplayType.Default);
    }
}