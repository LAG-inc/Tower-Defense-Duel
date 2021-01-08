using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager SI;

    [SerializeField] private GameObject barPrefab;
    [SerializeField] private GameObject gameOverUI;

    private List<Bar> _bars;
    private Transform _barContainer;

    private void Awake()
    {
        SI = SI == null ? this : SI;

        _bars = new List<Bar>();
        _barContainer = new GameObject("HealthBarContainer").transform;
    }

    public void AddBar(dynamic obj)
    {
        GameObject newUIObject = Instantiate<GameObject>(barPrefab, obj.transform.position, Quaternion.identity, _barContainer);
        obj.bar = newUIObject.GetComponent<Bar>(); 

        obj.bar.Initialise(obj.gameObject);

        _bars.Add(obj.bar);
    }

    public void RemoveBar(dynamic obj)
    {
        _bars.Remove(obj.bar);
        Destroy(obj.bar.gameObject);
    }

    public void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    private void LateUpdate()
    {
        for (int i = 0; i < _bars.Count; i++)
        {
            _bars[i].Move();
        }
    }
}
