using UnityEngine;
using UnityEngine.InputSystem;

public enum PointerDisplayType
{
    Default,
    ToolPointer,
    ToolHand,
    ToolBroom,
    ToolScrewdriver,
}

public interface IHoverInteractable
{
    PointerDisplayType PointerType { get; }

    void OnHoverEnter();
    void OnHoverExit();
}

public class PointerDisplaySystem : MonoBehaviour
{
    [Header("Cursor")]
    [SerializeField] private SpriteRenderer _cursorRenderer;

    [Header("Cursor Sprites")]
    [SerializeField] private Sprite _defaultPointer;
    [SerializeField] private Sprite _toolPointer;
    [SerializeField] private Sprite _toolHand;
    [SerializeField] private Sprite _toolBroom;
    [SerializeField] private Sprite _toolScrewdriver;

    private Camera _camera;
    private IHoverInteractable _currentHover;

    private void Awake()
    {
        _camera = Camera.main;

        Cursor.visible = false;
    }

    void Start()
    {
        SetDefaultPointer();
    }

    void LateUpdate()
    {
        UpdateCursorPosition();
        UpdateHover();
    }

    private void UpdateCursorPosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        mousePosition.z = -_camera.transform.position.z;

        _cursorRenderer.transform.position = _camera.ScreenToWorldPoint(mousePosition);
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
            return;

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
        _cursorRenderer.sprite = GetSprite(type);
    }

    public void SetDefaultPointer()
    {
        SetPointer(PointerDisplayType.Default);
    }

    private Sprite GetSprite(PointerDisplayType type)
    {
        return type switch
        {
            PointerDisplayType.Default => _defaultPointer,
            PointerDisplayType.ToolPointer => _toolPointer,
            PointerDisplayType.ToolHand => _toolHand,
            PointerDisplayType.ToolBroom => _toolBroom,
            PointerDisplayType.ToolScrewdriver => _toolScrewdriver,
            _ => _defaultPointer
        };
    }
}