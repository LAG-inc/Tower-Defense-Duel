using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager SI;

    [Header("Allies")]
    [SerializeField] private BaseObjectPool _robot1Pool;
    [SerializeField] private BaseObjectPool _robot2Pool;

    [Header("Enemies")]
    [SerializeField] private BaseObjectPool _alien1Pool;

    [Header("Common Objects")]
    [SerializeField] private BaseObjectPool _bulletPool;

    private void Awake()
    {
        _robot1Pool.FillQueue();
        _robot2Pool.FillQueue();
        _bulletPool.FillQueue();
        //_alien1Pool.FillQueue();
    }

    // Start is called before the first frame update
    void Start()
    {
        SI = SI == null ? this : SI;
    }

    public BaseObjectPool GetRobot1Pool()
    {
        return _robot1Pool;
    }

    public BaseObjectPool GetRobot2Pool()
    {
        return _robot2Pool;
    }

    public BaseObjectPool GetBulletPool()
    {
        return _bulletPool;
    }
}
