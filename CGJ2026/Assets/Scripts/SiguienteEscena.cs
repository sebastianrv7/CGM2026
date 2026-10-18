using UnityEngine;
using UnityEngine.SceneManagement;

public class SiguienteEscena : MonoBehaviour
{
    public string nombreEscena;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nombreEscena);
         
        }
    }
}
