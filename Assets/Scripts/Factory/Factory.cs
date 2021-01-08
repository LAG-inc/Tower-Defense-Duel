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
        //_creationTime = _buttonPressedData.generablesData[0].creationTime;
        CancelUnit = false;
        FactoryTracker.SetCanPlaceUnit(true);
    }

    public IEnumerator GenerateUnit(Vector2 location)
    {
        if (!_canGenerateUnit) yield break;

        GenerableButtonData bData = _buttonPressed.GetButtonData();

        yield return StartCoroutine(TimerBar(bData.generablesData[0].creationTime, _buttonPressed));

        if (CancelUnit) yield break;

        for (int i = 0; i < bData.generablesData.Length; i++)
        {
            GenerableData gDataRef = bData.generablesData[i];
            SetupGenerable(ref _unitToGenerate, gDataRef);
            _unitToGenerate.transform.position = location + bData.relativeOffsets[i];
            _unitToGenerate.SetActive(true);
        }
        _currentNumberUnits += bData.generablesData.Length;
        CheckGenerableButtonAvailability();
        _canGenerateUnit = _currentNumberUnits == maxNumberUnits ? false : true;

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

    private void SetupGenerable(ref GameObject go, GenerableData gDataRef)
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
                uScript.Activate(gDataRef); 
                uScript.OnDealDamage += OnGenerableDealtDamage;
                //uScript.OnProjectileFired += OnProjectileFired;
                //AddPlaceableToList(uScript); 
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

    //private void OnProjectileFired(ThinkingGenerable g)
    //{
    //    Vector3 adjTargetPos = g.target.transform.position;
    //    adjTargetPos.y = 1.5f;
    //    Quaternion rot = Quaternion.LookRotation(adjTargetPos - g.projectileSpawnPoint.position);

    //    Projectile prj = Instantiate<GameObject>(g.projectilePrefab, g.projectileSpawnPoint.position, rot).GetComponent<Projectile>();
    //    prj.target = g.target;
    //    prj.damage = g.damage;
    //    allProjectiles.Add(prj);
    //}

    private void OnGenerableDead(Generable g)
    {
        g.OnDie -= OnGenerableDead; //remove the listener

        switch (g.gType)
        {
            case Generable.GenerableType.Unit:
                Unit u = (Unit)g;
                //RemovePlaceableFromList(u);
                u.OnDealDamage -= OnGenerableDealtDamage;
                //u.OnProjectileFired -= OnProjectileFired;
                //UIManager.RemoveHealthUI(u);
                StartCoroutine(Dispose(u));
                break;
        }
    }

    private IEnumerator Dispose(ThinkingGenerable g)
    {
        yield return new WaitForSeconds(3f);

        Destroy(g.gameObject);
    }

    //No estoy seguro si deberia ir en este script
    public IEnumerator TimerBar(float time, dynamic obj)
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

    
}
