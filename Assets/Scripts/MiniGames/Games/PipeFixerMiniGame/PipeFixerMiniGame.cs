using UnityEngine;
using UnityEngine.InputSystem;

public class PipeFixerMiniGame : MiniGame
{
    [SerializeField] private Transform _pipeRowsHolder;

    private Transform[] _pipeRows;
    private Pipe[][] _pipesPerRow;
    private PipeFlowDirection[] _goalDirections;

    public override void OnInitialize()
    {
        GetPipeRows();

        _pipesPerRow = new Pipe[_pipeRows.Length][];
        _goalDirections = new PipeFlowDirection[_pipeRows.Length];

        for (int row = 0; row < _pipeRows.Length; row++)
        {
            _pipesPerRow[row] =
                _pipeRows[row].GetComponentsInChildren<Pipe>(true);

            if (_pipesPerRow[row].Length == 0)
            {
                Debug.LogWarning(
                    $"Pipe row '{_pipeRows[row].name}' contains no pipes.");

                continue;
            }

            foreach (var pipe in _pipesPerRow[row])
            {
                pipe.SetDirection(
                    (PipeFlowDirection)Random.Range(0, 4));
            }

            //Had to disable random for design reasons. When game is enhanced, we can add this back later
            // _goalDirections[row] = Random.Range(0, 2) == 0
            //     ? PipeFlowDirection.Left
            //     : PipeFlowDirection.Right;

            _goalDirections[row] = PipeFlowDirection.Right;
        }
    }

    public override void OnStart()
    {

    }

    public override void OnUpdate()
    {
        if (Mouse.current == null ||
            !Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        if (Camera.main == null)
        {
            Debug.LogWarning("No camera is tagged as MainCamera.");
            return;
        }

        Vector3 mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(
                Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Raycast(
            mouseWorldPosition,
            Vector2.zero);

        if (hit.collider == null)
        {
            return;
        }

        Pipe pipe = hit.collider.GetComponentInParent<Pipe>();

        if (pipe == null)
        {
            return;
        }

        AudioManager.Instance.PlaySFX("Rotate_Pipe");
        pipe.HandleRotateClockWise();

        if (CheckIfAllPipesAligned())
        {
            TriggerFinishedGame(true);
        }
    }

    public override void OnEnd()
    {
    }

    private void GetPipeRows()
    {
        if (_pipeRowsHolder == null)
        {
            Debug.LogError("Pipe rows holder has not been assigned.");

            _pipeRows = System.Array.Empty<Transform>();
            return;
        }

        int rowCount = _pipeRowsHolder.childCount;

        _pipeRows = new Transform[rowCount];

        for (int row = 0; row < rowCount; row++)
        {
            _pipeRows[row] = _pipeRowsHolder.GetChild(row);
        }

        Debug.Log($"Found {_pipeRows.Length} pipe rows.");
    }

    private bool CheckIfAllPipesAligned()
    {
        if (_pipesPerRow == null || _pipesPerRow.Length == 0)
        {
            return false;
        }

        for (int row = 0; row < _pipesPerRow.Length; row++)
        {
            Pipe[] pipes = _pipesPerRow[row];

            if (pipes == null || pipes.Length == 0)
            {
                return false;
            }

            foreach (Pipe pipe in pipes)
            {
                if (pipe.CurrentDirection != _goalDirections[row])
                {
                    return false;
                }
            }
        }

        return true;
    }
}