using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _startWithStartScreen = false;

    [SerializeField] private StartGameView _startGameView;

    private void Start()
    {
        if (!AudioManager.Instance.LoadedAudio)
        {
            AudioManager.Instance.Load();
        }

        FindAnyObjectByType<GameloopManager>().PrepareLoop();

#if UNITY_EDITOR
        bool startWithStartScreen = _startWithStartScreen;
#else
        // The release version will always be true.
        bool startWithStartScreen = true;
#endif

        if (startWithStartScreen)
        {
            AudioManager.Instance.PlayBGM("title_bgm");
            _startGameView.ToggleStartGameView(true);
        }
        else
        {
            AudioManager.Instance.PlayBGM("bgm");
            StartGameLoop();
        }

    }

    public void StartGameLoop()
    {
        AudioManager.Instance.StopAllBGM();
        AudioManager.Instance.PlayBGM("bgm");
        FindAnyObjectByType<GameloopManager>().StartGame();
    }



    public void RestartGame(bool isGameEnding)
    {
        SceneManager.LoadScene("MainScene");
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            StartGameLoop();
        };

    }
}