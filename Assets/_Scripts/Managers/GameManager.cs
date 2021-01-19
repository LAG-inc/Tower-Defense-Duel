using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static event EventManager.VoidEvent OnPause;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
    }

    /// <summary>
    /// Invoke Game Pause.
    /// To add a behavior to the event---- GameManager.OnPause+= [void func] / () => [single behavior]  
    /// </summary>
    public static void Pause()
    {
        OnPause?.Invoke();
    }

    private void OnDestroy()
    {
        OnPause = null;
    }    
}