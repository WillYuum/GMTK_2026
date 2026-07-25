using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StartGameView : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;




    void Start()
    {
        _startGameButton.onClick.AddListener(ClickAnywhereToStart);
    }


    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ClickAnywhereToStart();
        }
    }

    public void ToggleStartGameView(bool isActive)
    {
        gameObject.SetActive(isActive);
    }


    private void ClickAnywhereToStart()
    {
        ToggleStartGameView(false);
        FindAnyObjectByType<GameloopManager>().StartGame();
    }
}
