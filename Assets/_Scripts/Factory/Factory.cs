using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] private int maxNumberUnits;
    private int _currentNumberUnits;
    private bool _canGenerateUnit = true;
    private GenerableButton _buttonPressed;
    private GameObject _unitToGenerate;
    public bool CancelUnit { get; set; }

    //private float _creationTime;

    [SerializeField] private List<GenerableButton> _generableButtons;

    private void Start()
    {
        foreach (var button in _generableButtons)
        {
            button.OnMouseUp += PrepareUnitGeneration;
        }
    }

    public void PrepareUnitGeneration(GenerableButton button)
    {
        _buttonPressed = button;
        if (_buttonPressed.bar)
        {
            return;
        }
        CancelUnit = false;
        FactoryTracker.SetCanPlaceUnit(true);
    }

    public IEnumerator GenerateUnit(Vector2 location)
    {
        if (!_canGenerateUnit) yield break;

        GenerableButtonData bData = _buttonPressed.GetButtonData();

        //Timer para el tiempo de creacion
        yield return StartCoroutine(TimerBar(bData.generablesData[0].creationTime, _buttonPressed));

        if (CancelUnit) yield break;

        for (int i = 0; i < bData.generablesData.Length; i++)
        {
            GenerableData gDataRef = bData.generablesData[i];
            SetupGenerable(ref _unitToGenerate, gDataRef, gDataRef.unitFaction);
            _unitToGenerate.transform.position = location + bData.relativeOffsets[i];
            _unitToGenerate.SetActive(true);
        }
        _currentNumberUnits += bData.generablesData.Length;
        CheckGenerableButtonAvailability();
        _canGenerateUnit = _currentNumberUnits == maxNumberUnits ? false : true;

        //Timer para la 'energia' del aliado
        yield return StartCoroutine(TimerBar(bData.generablesData[0].hitPoints, _unitToGenerate.GetComponent<ThinkingGenerable>()));
    }

    public bool GetCanGenerateUnit() => _canGenerateUnit;

    private void CheckGenerableButtonAvailability()
    {
        foreach (var button in _generableButtons)
        {
            if((button.GetButtonData().generablesData.Length + _currentNumberUnits) > maxNumberUnits)
            {
                button.DisableButton();
            }
        }
    }

    private void SetupGenerable(ref GameObject go, GenerableData gDataRef, Generable.Faction gFaction)
    {
        switch (gDataRef.gType)
        {
            case Generable.GenerableType.Unit:

                switch (gDataRef.unityType)
                {
                    case Unit.UnitType.Robot1:
                        go = PoolManager.SI.GetRobot1Pool().ExtractFromQueue();
                        break;
                    case Unit.UnitType.Robot2:
                        go = PoolManager.SI.GetRobot2Pool().ExtractFromQueue();
                        break;
                    case Unit.UnitType.Robot3:
                        break;
                    default:
                        go = null;
                        break;
                }

                Unit uScript = go.GetComponent<Unit>();
                uScript.Activate(gFaction, gDataRef);
                uScript.OnDealDamage += OnGenerableDealtDamage;
                uScript.OnProjectileFired += OnProjectileFired;
                GameManager.Instance.AddPlaceableToList(uScript);
                //UIManager.AddHealthUI(uScript);

                break;
            case Generable.GenerableType.Resource:
                //TODO
                break;
            default:
                go = null;
                break;
        }

        go.GetComponent<Generable>().OnDie += OnGenerableDead;

    }

    private void OnGenerableDealtDamage(ThinkingGenerable g)
    {
        if (g.target.state != ThinkingGenerable.States.Dead)
        {
            float newHealth = g.target.SufferDamage(g.damage);
            g.target.bar.SetHealth(newHealth);
        }
    }

    private void OnProjectileFired(ThinkingGenerable g)
    {
        Vector3 adjTargetPos = g.target.transform.position;
        Vector3 vectorToTarget = adjTargetPos - g.projectileSpawnPoint.position;

        Quaternion rot = Quaternion.LookRotation(Vector3.forward, vectorToTarget);

        GameObject prj = PoolManager.SI.GetBulletPool().ExtractFromQueue();
        prj.transform.position = g.projectileSpawnPoint.position;
        prj.GetComponent<Projectile>().SetInitialPosition(g.projectileSpawnPoint.position);
        prj.transform.rotation = rot;

        prj.GetComponent<Projectile>().target = g.target;
        prj.GetComponent<Projectile>().damage = g.damage;

        prj.gameObject.SetActive(true);
        GameManager.Instance.GetAllProjectiles().Add(prj.GetComponent<Projectile>());
    }

    private void OnGenerableDead(Generable g)
    {
        g.OnDie -= OnGenerableDead; //remove the listener

        switch (g.gType)
        {
            case Generable.GenerableType.Unit:
                Unit u = (Unit)g;
                //RemovePlaceableFromList(u);
                u.OnDealDamage -= OnGenerableDealtDamage;
                u.OnProjectileFired -= OnProjectileFired;
                UIManager.SI.RemoveBar(u);
                StartCoroutine(DisposeUnit(u));
                break;
        }
    }

    private IEnumerator DisposeUnit(Unit g)
    {
        //time for animation
        yield return new WaitForSeconds(0f);

        g.gameObject.SetActive(false);

        switch (g.GetUnitType())
        {
            case Unit.UnitType.Robot1:
                PoolManager.SI.GetRobot1Pool().EnqueueObj(g.gameObject);
                break;
            case Unit.UnitType.Robot2:
                PoolManager.SI.GetRobot2Pool().EnqueueObj(g.gameObject);
                break;
            case Unit.UnitType.Robot3:
                break;
            default:
                break;
        }
    }

    //No estoy seguro si deberia ir en este script
    public IEnumerator TimerBar(float time, GenerableButton obj)
    {
        UIManager.SI.AddBar(obj);

        for (float i = time*10; i > 0; i--)
        {
            if (CancelUnit) break;

            obj.bar.SetHealth((i/10) - 0.1f);

            yield return new WaitForSecondsRealtime(0.1f);
        }

        UIManager.SI.RemoveBar(obj);
    }

    public IEnumerator TimerBar(float time, ThinkingGenerable obj)
    {
        for (float i = time * 10; i > 0; i--)
        {
            if (CancelUnit) break;

            obj.bar.SetHealth(obj.SufferDamage(0.1f));

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
