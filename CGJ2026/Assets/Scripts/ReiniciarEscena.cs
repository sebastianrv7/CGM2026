using UnityEngine;
using UnityEngine.SceneManagement;


public class ReiniciarEscena : MonoBehaviour
{
     public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


