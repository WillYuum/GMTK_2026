using UnityEngine;
using UnityEngine.UI;

public class StartGameView : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;




    void Start()
    {
        _startGameButton.onClick.AddListener(OnStartGameButtonClicked);
    }

    public void ToggleStartGameView(bool isActive)
    {
        gameObject.SetActive(isActive);
    }


    private void OnStartGameButtonClicked()
    {
        ToggleStartGameView(false);
        FindAnyObjectByType<GameloopManager>().StartGame();
    }
}
