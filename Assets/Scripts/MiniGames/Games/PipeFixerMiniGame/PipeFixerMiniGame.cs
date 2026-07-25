using UnityEngine;
using UnityEngine.InputSystem;

public class PipeFixerMiniGame : MiniGame
{


    [SerializeField] private Transform _pipesHolder;

    private Pipe[] _referencePipes;

    private PipeFlowDirection _goalFlowDirection;


    public override void OnInitialize()
    {
        _referencePipes = _pipesHolder.GetComponentsInChildren<Pipe>(true);

        foreach (var pipe in _referencePipes)
        {
            pipe.SetDirection((PipeFlowDirection)Random.Range(0, 4));
        }

        // allowed assigning for direction are left and right only for now
        _goalFlowDirection = Random.Range(0, 2) == 0 ? PipeFlowDirection.Left : PipeFlowDirection.Right;
    }
    public override void OnStart()
    {

    }

    public override void OnUpdate()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        var hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()),
            Vector2.zero);

        if (hit.collider == null)
        {
            return;
        }

        if (hit.collider.TryGetComponent<Pipe>(out var pipe))
        {
            pipe.HandleRotateClockWise();

            if (CheckIfAllPipesAligned())
            {
                TriggerFinishedGame(true);
            }
        }
    }

    public override void OnEnd()
    {

    }

    private bool CheckIfAllPipesAligned()
    {
        foreach (var pipe in _referencePipes)
        {
            if (pipe.CurrentDirection != _goalFlowDirection)
            {
                return false;
            }
        }

        return true;
    }

}
