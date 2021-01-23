using UnityEngine;

public class PrepareLevel : MonoBehaviour
{
    [SerializeField] private LvlRps lvlConfiguration;

    private void OnMouseDown()
    {
        LvlHelper.SingleInstance.SetNewLevel(lvlConfiguration);
    }
}