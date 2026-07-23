using UnityEngine;
using UnityEngine.InputSystem;

public class InteractWithMiniGamesController : MonoBehaviour
{


    void Update()
    {
        Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        //if mouse hover
        var hits = Physics2D.Raycast(mousePosInWorld, Vector2.zero);

        if (hits.collider != null && hits.collider.TryGetComponent(out MiniGamePanel miniGamePosition))
        {
            bool isClicked = Mouse.current.leftButton.wasPressedThisFrame;
            if (isClicked && miniGamePosition.IsRemoved == false)
            {
                miniGamePosition.RemovePanel();
            }
        }

    }
}
