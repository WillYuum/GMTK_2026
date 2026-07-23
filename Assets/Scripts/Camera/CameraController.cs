using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    private Camera _mainCamera;

    [Header("Camera Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 10f;

    private Vector2 _currentVelocity = Vector2.zero;

    [Header("Camera Bounds")]
    [SerializeField] private Bounds _cameraBounds;
    [SerializeField] private float MovementZone = 100f;


    void Awake()
    {
        _mainCamera = Camera.main;
    }


    void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        if (!CanMoveCamera(mousePosition))
        {
            _currentVelocity = Vector2.zero;
            return;
        }

        HandleMoveCamera(mousePosition);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_cameraBounds.center, _cameraBounds.size);
    }

    private void HandleMoveCamera(Vector2 mousePosition)
    {
        Vector2 targetVelocity = GetMovement(mousePosition) * _moveSpeed;

        _currentVelocity = Vector2.Lerp(_currentVelocity, targetVelocity, _acceleration * Time.deltaTime);


        Vector3 newPosition = _mainCamera.transform.position + new Vector3(_currentVelocity.x, _currentVelocity.y, 0f) * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, _cameraBounds.min.x, _cameraBounds.max.x);
        newPosition.y = Mathf.Clamp(newPosition.y, _cameraBounds.min.y, _cameraBounds.max.y);

        _mainCamera.transform.position = newPosition;
    }

    private Vector2 GetMovement(Vector2 mousePosition)
    {
        Vector2 movement = Vector2.zero;

        if (mousePosition.x < MovementZone)
        {
            movement.x = -(1f - mousePosition.x / MovementZone);
        }

        else if (mousePosition.x > Screen.width - MovementZone)
        {
            movement.x = (mousePosition.x - (Screen.width - MovementZone)) / MovementZone;
        }

        if (mousePosition.y < MovementZone)
        {
            movement.y = -(1f - mousePosition.y / MovementZone);
        }

        else if (mousePosition.y > Screen.height - MovementZone)
        {
            movement.y = (mousePosition.y - (Screen.height - MovementZone)) / MovementZone;
        }

        return Vector2.ClampMagnitude(movement, 1f);
    }



    private bool CanMoveCamera(Vector2 mousePosition)
    {
        //This is perfect for Unity editor & when in game the mouse is out of viewport
        Vector2 view = _mainCamera.ScreenToViewportPoint(mousePosition);
        bool isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;

        bool isFocused = Application.isFocused;


        return !isOutside && isFocused;
    }

}
