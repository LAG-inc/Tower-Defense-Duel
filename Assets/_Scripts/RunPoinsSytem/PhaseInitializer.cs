using UnityEngine;

public class PhaseInitializer : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("Starting Phase");
        RpsManager.SingleInstance.PreparePhase();
        EnemyGenerator.StartGenerating();
        if (!RpsManager.SingleInstance.IsAnotherPhase())
        {
            gameObject.SetActive(false);
        }
    }
}