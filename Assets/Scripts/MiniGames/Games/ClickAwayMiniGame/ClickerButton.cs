using UnityEngine;

public enum ButtonState
{
    Locked,
    Available,
    Pressed
}

public class ClickerButton : MonoBehaviour, IHoverInteractable
{
    public PointerDisplayType PointerType => PointerDisplayType.ToolPointer;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    public ButtonState State { get; private set; }

    private ClickAwayMiniGame _game;

    public void Initialize(ClickAwayMiniGame game)
    {
        _game = game;
    }

    public void SetState(ButtonState state, Sprite sprite)
    {
        State = state;
        _spriteRenderer.sprite = sprite;
    }

    public void Click()
    {
        _game.PressButton(this);
    }

    public void OnHoverEnter()
    {
    }

    public void OnHoverExit()
    {
    }
}