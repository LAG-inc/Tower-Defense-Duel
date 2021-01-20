using UnityEngine;

public class Projectile : MonoBehaviour
{
	[HideInInspector] public ThinkingGenerable target;
	[HideInInspector] public float damage;
	private float speed = 1f;
	private float progress = 0f;
	private Vector3 offset = new Vector3(0f, 0f, 0f);
	private Vector3 initialPosition;

	private void OnEnable()
	{
		initialPosition = transform.position;
	}

	public float Move()
	{
		progress += Time.deltaTime * speed;
		transform.position = Vector3.Lerp(initialPosition, target.transform.position + offset, progress);

		return progress;
	}

    public void SetInitialPosition(Vector3 initialPosition)
    {
        this.initialPosition = initialPosition;
    }
}
