using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public ThinkingGenerable target;
    [HideInInspector] public float damage;
    private float speed = 1f;
    private float _progress = 0f;
    private Vector3 offset = new Vector3(0f, 0f, 0f);
    private Vector3 _initialPosition;

    private void OnEnable()
    {
        _initialPosition = transform.position;
    }

    public float Move()
    {
        _progress += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(_initialPosition, target.transform.position + offset, _progress);

        return _progress;
    }

    public void SetInitialPosition(Vector3 initialPosition)
    {
        this._initialPosition = initialPosition;
    }

    private void OnDisable()
    {
        _progress = 0;
    }
}