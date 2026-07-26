using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameloopManager : MonoBehaviour
{

    [field: SerializeField] public int StartingCountDownValue { get; private set; } = 90;
    public int CurrentCountDownValue { get; private set; } = 90;


    private float _timer = 1f;

    [SerializeField] private GameObject _rocketEntity;
    [SerializeField] private GameObject _hud;

    private CameraController _cameraController;

    public MiniGameFinishedTracker MiniGameFinishedTracker { get; private set; }


    [SerializeField] private CountDownTimerController _countDownTimerController;

    public MiniGamePanel[] ListOfMiniGames { get; private set; }


    void Awake()
    {
        _cameraController = FindAnyObjectByType<CameraController>();
        ListOfMiniGames = FindObjectsByType<MiniGamePanel>();
    }

    public void PrepareLoop()
    {
        enabled = false;
        _cameraController.ToggleCameraMovement(false);
    }


    public void StartGame()
    {
        enabled = true;
        Debug.Log($"[GameloopManager] StartGame");
        _countDownTimerController.SetTime(StartingCountDownValue);
        CurrentCountDownValue = StartingCountDownValue;

        MiniGameFinishedTracker = new(ListOfMiniGames.Length);

        _cameraController.ToggleCameraMovement(true);

        foreach (var miniGame in ListOfMiniGames)
        {
            miniGame.SetState(MiniGamePanelState.Warning);
        }
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _timer = 1f;
            CurrentCountDownValue--;
            _countDownTimerController.UpdateTime(CurrentCountDownValue);
        }


        if (CurrentCountDownValue <= 0)
        {
            Debug.Log($"[GameloopManager] Game Over");
            ShowGameEnding();
            enabled = false;
            return;
        }
    }

    public void NotifyMiniGameFinished()
    {
        MiniGameFinishedTracker.IncrementFinishedCount();
        if (MiniGameFinishedTracker.IsAllMiniGamesFinished())
        {
            Debug.Log($"[GameloopManager] All MiniGames Finished");
            enabled = false;
            ShowGameEnding();
        }
    }




    public void ShowGameEnding()
    {
        AudioManager.Instance.StopAllBGM();
        AudioManager.Instance.PlaySFX("title_bgm");

        _cameraController.ToggleCameraMovement(false);

        string sceneName = "GameEndSequence";

        Scene scene = SceneManager.GetSceneByName(sceneName);




        //Hide UI for timer
        _hud.GetComponent<CanvasGroup>().DOFade(0f, 1f);

        // fade out rocket
        //Get all sprite renderers from rocket entity
        var spriteRenderers = _rocketEntity.GetComponentsInChildren<SpriteRenderer>();
        var sequence = DOTween.Sequence();
        foreach (var spriteRenderer in spriteRenderers)
        {
            // fade out the sprite renderer
            sequence.Join(FadeOutSprite(spriteRenderer, 1f));
        }


        sequence.OnComplete(() =>
        {
            if (scene.IsValid() && scene.isLoaded)
            {
                GameEndSequence gameEndSequence = FindAnyObjectByType<GameEndSequence>();
                gameEndSequence.PlayEnding();
            }
            else
            {
                new SceneAdditiveLoader().LoadSceneAdditive(sceneName, () =>
                {
                    GameEndSequence gameEndSequence = FindAnyObjectByType<GameEndSequence>();
                    gameEndSequence.PlayEnding();
                });
            }
        });
    }

    private Tween FadeOutSprite(SpriteRenderer spriteRenderer, float duration)
    {
        return spriteRenderer.DOFade(0f, duration);
    }
}




class SceneAdditiveLoader
{
    //Load and wait with callback a scene additive
    public void LoadSceneAdditive(string sceneName, System.Action callback)
    {
        var loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (callback != null)
        {
            loadOperation.completed += _ => callback();
        }
    }
}



public class MiniGameFinishedTracker
{
    public int MiniGameCounts { get; private set; }
    public int MiniGameFinished { get; private set; }

    public MiniGameFinishedTracker(int miniGameCounts)
    {
        MiniGameCounts = miniGameCounts;
        MiniGameFinished = 0;
    }


    public void IncrementFinishedCount()
    {
        MiniGameFinished++;
    }

    public int GetRemainingMiniGames()
    {
        return MiniGameCounts - MiniGameFinished;
    }

    public bool IsAllMiniGamesFinished()
    {
        return MiniGameFinished >= MiniGameCounts;
    }

}