using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch; 
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch; 

    private TouchControls _touchControls;
    private Camera _mainCamera;

    private void Awake() {
        _touchControls = new TouchControls();
        _mainCamera = Camera.main;
    }    

    private void OnEnable() {
        _touchControls.Enable();
    }

    private void OnDisable() {
        _touchControls.Disable();
    }

    void Start()
    {
        Debug.Log("Start");
        _touchControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        _touchControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    private void StartTouchPrimary(InputAction.CallbackContext context) {
        Debug.Log("Start touch");
        OnStartTouch?.Invoke(Utils.ScreenToWorld(_mainCamera, _touchControls.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.startTime);
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        OnEndTouch?.Invoke(Utils.ScreenToWorld(_mainCamera, _touchControls.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.time);
    }

    public Vector2 PrimaryPosition() {
        return Utils.ScreenToWorld(_mainCamera, _touchControls.Touch.PrimaryPosition.ReadValue<Vector2>());

    }
}
