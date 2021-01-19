using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Camera _mainCamera;
    private Vector3 _target;
    [SerializeField, Range(1, 20)] private float velocity;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _target = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(_target.x, _target.y, 0),
            velocity * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _target = transform.position;
    }
}