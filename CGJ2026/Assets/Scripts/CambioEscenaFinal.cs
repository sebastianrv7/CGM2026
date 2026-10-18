using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CambioEscenaFinal : MonoBehaviour
{


    public string nombreEscena;
    public float tiempoEspera = 5f;

    void Start()
    {
        StartCoroutine(CambiarEscena());
    }

    IEnumerator CambiarEscena()
    {
        yield return new WaitForSeconds(tiempoEspera);

        SceneManager.LoadScene(nombreEscena);
    }
}
