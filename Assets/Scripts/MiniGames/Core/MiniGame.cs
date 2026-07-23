
public abstract class MiniGame : IMiniGame
{
    public abstract void StartGame();
    public abstract void EndGame();
    public abstract void UpdateGame();
}


public interface IMiniGame
{
    void StartGame();
    void EndGame();
    void UpdateGame();
}
