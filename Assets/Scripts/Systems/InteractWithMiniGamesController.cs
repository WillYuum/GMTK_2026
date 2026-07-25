using UnityEngine;
using UnityEngine.InputSystem;

public class InteractWithMiniGamesController : MonoBehaviour
{

    [SerializeField] private float _rotationInterval = 0.5f;

    private Screw _currentScrew;
    private float _holdTimer;

    private MiniGameLauncher _miniGameLauncher;

    private Camera _mainCamera;

    void Awake()
    {
        _miniGameLauncher = FindAnyObjectByType<MiniGameLauncher>();
        _mainCamera = Camera.main;
    }


    void Update()
    {
        Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        var hit = Physics2D.Raycast(mousePosInWorld, Vector2.zero);

        if (hit.collider == null)
        {
            Debug.Log("No collider hit");
            return;
        }

        if (hit.collider.TryGetComponent(out Screw foundScrew))
        {
            HandleScrew(foundScrew);
        }
        else if (hit.collider.TryGetComponent(out MiniGamePanel foundPanel))
        {
            HandlePanel(foundPanel);
        }
    }


    private void HandleScrew(Screw screw)
    {
        if (!Mouse.current.leftButton.isPressed)
        {
            ResetScrewInteraction();
            return;
        }

        if (_currentScrew != screw)
        {
            _currentScrew = screw;
            _holdTimer = 0f;
        }

        _holdTimer += Time.deltaTime;

        if (_holdTimer >= _rotationInterval)
        {
            _holdTimer -= _rotationInterval;

            bool removed = screw.Rotate();

            if (removed)
            {
                ResetScrewInteraction();
            }
        }
    }

    private void HandlePanel(MiniGamePanel miniGamePanel)
    {
        bool isClicked = Mouse.current.leftButton.wasPressedThisFrame;

        if (!isClicked || miniGamePanel.IsRemoved || miniGamePanel.CheckIsLocked())
        {
            return;
        }

        MiniGame miniGame = miniGamePanel.MiniGameInstance;

        _miniGameLauncher.Initialize(miniGame, miniGamePanel);

        miniGamePanel.RemovePanel(() =>
        {
            _miniGameLauncher.LaunchMiniGame();
        });
    }

    private void ResetScrewInteraction()
    {
        _currentScrew = null;
        _holdTimer = 0f;
    }
}
