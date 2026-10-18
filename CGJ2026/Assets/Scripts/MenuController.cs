using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
  public void CambiarEscena(string niveles)
    {
        SceneManager.LoadScene(niveles);
    }

  public void salir()
    {
       Application.Quit();  
    }
}
