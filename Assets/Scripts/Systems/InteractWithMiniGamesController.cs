using UnityEngine;
using UnityEngine.InputSystem;

public class InteractWithMiniGamesController : MonoBehaviour
{

    private MiniGameLauncher _miniGameLauncher;

    void Awake()
    {
        _miniGameLauncher = FindAnyObjectByType<MiniGameLauncher>();
    }


    void Update()
    {
        Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        //if mouse hover
        var hits = Physics2D.Raycast(mousePosInWorld, Vector2.zero);

        if (hits.collider != null && hits.collider.TryGetComponent(out MiniGamePanel miniGamePanel))
        {
            bool isClicked = Mouse.current.leftButton.wasPressedThisFrame;
            if (isClicked && miniGamePanel.IsRemoved == false)
            {
                MiniGame miniGame = miniGamePanel.MiniGameInstance;

                _miniGameLauncher.Initialize(miniGame, miniGamePanel);
                miniGamePanel.RemovePanel(() =>
                {
                    _miniGameLauncher.LaunchMiniGame();
                });
            }
        }
    }
}
