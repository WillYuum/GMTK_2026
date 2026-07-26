using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimation : MonoBehaviour
{
    public Image image;
    public Sprite[] frames;
    public float frameRate = 12f;

    private int currentFrame;
    private float timer;

    void Update()
    {
        if (frames.Length == 0)
            return;

        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer = 0f;

            currentFrame++;
            if (currentFrame >= frames.Length)
                currentFrame = 0;

            image.sprite = frames[currentFrame];
        }
    }
}