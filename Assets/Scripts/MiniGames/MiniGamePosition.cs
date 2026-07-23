using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGamePosition : MonoBehaviour
{


    void Update()
    {
        Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        //if mouse hover
        var hits = Physics2D.Raycast(mousePosInWorld, Vector2.zero);

        if (hits.collider != null && hits.collider.TryGetComponent<MiniGamePosition>(out MiniGamePosition miniGamePosition))
        {
            bool isClicked = Mouse.current.leftButton.wasPressedThisFrame;
            if (isClicked)
            {
                OnMouseClick();
            }
        }

    }




    private void OnMouseClick()
    {
        //Run Game
    }

}
