using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _startWithStartScreen = false;

    [SerializeField] private StartGameView _startGameView;

    private void Start()
    {
        FindAnyObjectByType<GameloopManager>().PrepareLoop();

#if UNITY_EDITOR
        bool startWithStartScreen = _startWithStartScreen;
#else
        // The release version will always be true.
        bool startWithStartScreen = true;
#endif

        if (startWithStartScreen)
        {
            _startGameView.ToggleStartGameView(true);
        }
        else
        {
            StartGameLoop();
        }
    }

    public void StartGameLoop()
    {
        FindAnyObjectByType<GameloopManager>().StartGame();
    }
}