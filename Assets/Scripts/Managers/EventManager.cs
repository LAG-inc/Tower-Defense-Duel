using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void VoidEvent();

    public static event VoidEvent OnPause;

    public static void Pause()
    {
        OnPause?.Invoke();
    }

    private void OnDestroy()
    {
        OnPause = null;
    }
}