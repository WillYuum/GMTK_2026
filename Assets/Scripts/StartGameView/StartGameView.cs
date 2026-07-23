using UnityEngine;
using UnityEngine.UI;

public class StartGameView : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;




    void Start()
    {
        _startGameButton.onClick.AddListener(OnStartGameButtonClicked);
    }


    private void OnStartGameButtonClicked()
    {
        FindAnyObjectByType<GameloopManager>().StartGame();
    }
}
