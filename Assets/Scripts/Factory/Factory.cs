using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] private int maxNumberUnits;
    private int _currentNumberUnits;
    private bool _canGenerateUnit = true;
    private GenerableButtonData buttonPressedData;
    private GameObject _unitToGenerate;

    [SerializeField] private List<GenerableButton> _generableButtons;

    private void Start()
    {
        foreach (var button in _generableButtons)
        {
            button.OnMouseUp += PrepareUnitGeneration;
        }

    }

    public void PrepareUnitGeneration(GenerableButtonData buttonData)
    {
        buttonPressedData = buttonData;
        FactoryTracker.SetCanPlaceUnit(true);
    }

    public void GenerateUnit(Vector2 location)
    {
        if (!_canGenerateUnit) return;
        for (int i = 0; i < buttonPressedData.generablesData.Length; i++)
        {
            GenerableData gDataRef = buttonPressedData.generablesData[i];
            SetupGenerable(ref _unitToGenerate, gDataRef);
            _unitToGenerate.transform.position = location + buttonPressedData.relativeOffsets[i];
            _unitToGenerate.SetActive(true);
        }
        _currentNumberUnits += buttonPressedData.generablesData.Length;
        CheckGenerableButtonAvailability();
        _canGenerateUnit = _currentNumberUnits == maxNumberUnits ? false : true;
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
            g.target.healthBar.SetHealth(newHealth);
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
}
