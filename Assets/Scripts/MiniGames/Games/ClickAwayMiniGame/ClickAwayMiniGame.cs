using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickAwayMiniGame : MiniGame
{
    [SerializeField] private Transform _buttonHolder;
    private ClickerButton[] _buttons;

    [Header("Chances")]
    [Range(0, 1)][SerializeField] private float _unlockChance = 0.4f;
    [Range(0, 1)][SerializeField] private float _lockChance = 0.3f;
    [Range(0, 1)][SerializeField] private float _unpressChance = 0.2f;

    [Header("State Sprites")]
    [SerializeField] private Sprite _lockedSprite;
    [SerializeField] private Sprite _availableSprite;
    [SerializeField] private Sprite _pressedSprite;

    public override void OnStart()
    {
        _buttons = _buttonHolder.GetComponentsInChildren<ClickerButton>(true);

        foreach (var button in _buttons)
        {
            button.Initialize(this);
            button.SetState(ButtonState.Locked, GetSprite(ButtonState.Locked));
        }

        int unlockCount = Mathf.Min(Random.Range(1, 3), _buttons.Length);

        List<int> indices = Enumerable.Range(0, _buttons.Length).ToList();

        for (int i = indices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }

        for (int i = 0; i < unlockCount; i++)
        {
            _buttons[indices[i]]
                .SetState(ButtonState.Available, GetSprite(ButtonState.Available));
        }
    }

    public override void OnUpdate()
    {
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);

        if (hit.collider == null)
        {
            return;
        }

        if (hit.collider.TryGetComponent<ClickerButton>(out var button))
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                button.Click();
            }
        }
    }

    public override void OnEnd() { }

    public void PressButton(ClickerButton pressedButton)
    {
        if (pressedButton.State != ButtonState.Available)
            return;

        pressedButton.SetState(ButtonState.Pressed, GetSprite(ButtonState.Pressed));

        foreach (var button in _buttons)
        {
            if (button == pressedButton)
                continue;

            switch (button.State)
            {
                case ButtonState.Locked:
                    if (Random.value < _unlockChance)
                        button.SetState(ButtonState.Available, GetSprite(ButtonState.Available));
                    break;

                case ButtonState.Available:
                    if (Random.value < _lockChance)
                        button.SetState(ButtonState.Locked, GetSprite(ButtonState.Locked));
                    break;

                case ButtonState.Pressed:
                    if (Random.value < _unpressChance)
                        button.SetState(ButtonState.Available, GetSprite(ButtonState.Available));
                    break;
            }
        }

        EnsureOneAvailable();

        if (_buttons.All(x => x.State == ButtonState.Pressed))
        {
            TriggerFinishedGame(true);
        }
    }

    private void EnsureOneAvailable()
    {
        if (_buttons.Any(x => x.State == ButtonState.Available))
        {
            return;
        }

        List<ClickerButton> locked = _buttons
            .Where(x => x.State == ButtonState.Locked)
            .ToList();

        if (locked.Count > 0)
        {
            locked[Random.Range(0, locked.Count)].SetState(ButtonState.Available, GetSprite(ButtonState.Available));
        }
    }

    private Sprite GetSprite(ButtonState state)
    {
        return state switch
        {
            ButtonState.Locked => _lockedSprite,
            ButtonState.Available => _availableSprite,
            ButtonState.Pressed => _pressedSprite,
            _ => null
        };
    }
}