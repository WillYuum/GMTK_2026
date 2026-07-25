using UnityEngine;

public enum PipeFlowDirection
{
    Up,
    Down,
    Left,
    Right
}


public class Pipe : MonoBehaviour, IHoverInteractable
{

    public PipeFlowDirection CurrentDirection { get; private set; }
    private SpriteRenderer _spriteRenderer;

    public PointerDisplayType PointerType => PointerDisplayType.ToolWrench;

    void Start()
    {

    }

    void Update()
    {

    }



    public void SetDirection(PipeFlowDirection direction)
    {
        CurrentDirection = direction;
        UpdateToDirection();
    }

    public void HandleRotateClockWise()
    {
        CurrentDirection = CurrentDirection switch
        {
            PipeFlowDirection.Up => PipeFlowDirection.Right,
            PipeFlowDirection.Right => PipeFlowDirection.Down,
            PipeFlowDirection.Down => PipeFlowDirection.Left,
            PipeFlowDirection.Left => PipeFlowDirection.Up,
            _ => PipeFlowDirection.Right
        };

        UpdateToDirection();
    }

    //Note: Rotation will with local rotation only from right to up. The left and down 
    // will be handled by flipping the scale of the pipe. This is because the original sprite is facing right.
    // and visually it looks better.
    private void UpdateToDirection()
    {
        // Reset first so the previous direction cannot affect the new one.
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        switch (CurrentDirection)
        {
            case PipeFlowDirection.Right:
                // Original sprite direction.
                break;

            case PipeFlowDirection.Left:
                transform.localScale = new Vector3(-1f, 1f, 1f);
                break;

            case PipeFlowDirection.Up:
                transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                break;

            case PipeFlowDirection.Down:
                transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                transform.localScale = new Vector3(-1f, 1f, 1f);
                break;
        }
    }

    public void OnHoverEnter()
    {

    }

    public void OnHoverExit()
    {

    }
}




