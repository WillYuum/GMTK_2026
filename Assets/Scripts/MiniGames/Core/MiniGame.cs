
using UnityEngine;

public abstract class MiniGame : MonoBehaviour, IMiniGame
{
    public bool IsGameActive { get; set; } = false;

    public event System.Action<bool> OnGameFinished;

    protected virtual void TriggerFinishedGame(bool isSuccess)
    {
        if (!IsGameActive)
        {
            Debug.LogWarning("MiniGame is not active. Cannot trigger finished game.");
            return;
        }

        if (isSuccess)
        {
            Debug.Log("MiniGame finished successfully!");
        }
        else
        {
            Debug.Log("MiniGame failed!");
        }

        OnEnd();
        IsGameActive = false;
        enabled = false;
        OnGameFinished?.Invoke(isSuccess);
    }


    public abstract void OnStart();
    public abstract void OnEnd();
    public abstract void OnUpdate();
}


public interface IMiniGame
{
    void OnStart();
    void OnEnd();
    void OnUpdate();
}
