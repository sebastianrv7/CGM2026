using UnityEngine;

public class VisionBurak : MonoBehaviour
{
    [SerializeField] private BurakController scriptPadre;



    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si detecta al jugador, le pasa la referencia al padre para que empiece a perseguir
        if (other.TryGetComponent<Movement>(out Movement jugador))
        {
            scriptPadre.IniciarPersecucion(jugador.transform);

            // Opcional: Desactivamos este colisionador gigante porque ya fijamos al jugador 
            // para siempre y ya no necesitamos seguir gastando recursos procesándolo.
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
