using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneGame : MonoBehaviour
{
    public int indexScene;

    private void OnEnable()
    {
        SceneManager.LoadScene(indexScene);
    }
}