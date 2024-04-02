using UnityEngine;
using System.Collections;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField]
    private float minimumDistance = .2f;
    [SerializeField]
    private float maximumTime = 1f;
    [SerializeField, Range(0f,1f)]
    private float directionThreshold = .9f;

    public GameObject cube;

    private InputManager _inputManager;
    private Vector2 _startPosition;
    private float _startTime;
    private Vector2 _endPosition;
    private float _endTime;
    
    // private Animator _cubeAnimator;


    private void Awake() {
        _inputManager = InputManager.Instance;
        cube = GameObject.Find("Chest");
        // _cubeAnimator = cube.GetComponent<Animator>();
    }

    private void OnEnable() {
        _inputManager.OnStartTouch += SwipeStart;
        _inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable() {
        _inputManager.OnStartTouch -= SwipeStart;
        _inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time) {
        _startPosition = position;
        _startTime = time;
    }

    private void SwipeEnd(Vector2 position, float time) {
        _endPosition = position;
        _endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe() {
        if (Vector3.Distance(_startPosition, _endPosition) >= minimumDistance &&
            _endTime -_startTime <= maximumTime) {
            Debug.DrawLine(_startPosition, _endPosition, Color.red, 5f);
            Vector3 direction = _endPosition - _startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }


    private void SwipeDirection(Vector2 direction) {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Debug.Log("Swipe up");
            RotateCube(Vector3.right); // Play the animation around the x-axis
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.Log("Swipe down");
            RotateCube(Vector3.right); // Play the animation around the x-axis
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.Log("Swipe right");
            RotateCube(Vector3.up); // Play the animation around the y-axis
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log("Swipe left");
            RotateCube(Vector3.up); // Play the animation around the y-axis
        }
    }

    // private void PlayAnimation(Vector3 axis) {
    //     cube.transform.localRotation = Quaternion.LookRotation(axis);
    //     _cubeAnimator.Play("TurnCube");
    // }

    private void RotateCube(Vector3 axis) {    
        StartCoroutine(RotateOverTime(axis, 1f));

    }
    
    private IEnumerator RotateOverTime(Vector3 axis, float duration) {
        Quaternion startRotation = cube.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(axis * 90);

        for (float t = 0; t < duration; t += Time.deltaTime) {
            cube.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            yield return null;
        }

        cube.transform.rotation = endRotation;
    }
    
}