using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    [SerializeField] private string sscena;

    public void Restart()
    {
        SceneManager.LoadScene(sscena);
    }
}
