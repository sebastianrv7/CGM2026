using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
  public void CambiarEscena(string niveles)
    {
        SceneManager.LoadScene("Nivel3");
    }

  public void salir()
    {
       Application.Quit();  
    }
}
