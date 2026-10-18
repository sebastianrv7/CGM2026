using UnityEngine;
using System.Collections;

public class MostrarCanvas : MonoBehaviour
{

    public GameObject canvas;
    public float tiempoEspera = 120f;

    void Start()
    {
        canvas.SetActive(false);
        StartCoroutine(EsperarYMostrar());
    }

    IEnumerator EsperarYMostrar()
    {
        yield return new WaitForSeconds(tiempoEspera);

        canvas.SetActive(true);
    }
}


