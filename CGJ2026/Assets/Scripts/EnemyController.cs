using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{

    private enum EstadoEnemigo { Inactivo, Persiguiendo, PreparandoEmbestida, Embestiendo, Exhausto }
    private EstadoEnemigo estadoActual = EstadoEnemigo.Inactivo;

    [Header("Movimiento Normal")]
    [SerializeField] private float velocidadPersiguiento = 3.5f;

    [Header("Configuración de la Embestida (Dash)")]
    [SerializeField] private float velocidadEmbestida = 15f;
    [SerializeField] private float duracionEmbestida = 0.25f; // Cuánto tiempo dura el impulso

    private Transform targetJugador;
    private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Vector2 direccionEmbestida;

    [SerializeField] private GameObject flor;
    [SerializeField] private Movement movement;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        // Orientar el sprite hacia el jugador (solo si no está embestiendo en una dirección fija)
        if (targetJugador != null && estadoActual != EstadoEnemigo.Embestiendo)
        {
            if (targetJugador.position.x < transform.position.x)
                spriteRenderer.flipX = true;
            else if (targetJugador.position.x > transform.position.x)
                spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {
        if (targetJugador == null) return;

        // Ejecutamos las físicas dependiendo del estado actual
        switch (estadoActual)
        {
            case EstadoEnemigo.Persiguiendo:
                Vector2 direccionNormal = (targetJugador.position - transform.position).normalized;
                rb.MovePosition(rb.position + direccionNormal * velocidadPersiguiento * Time.fixedDeltaTime);
                break;

            case EstadoEnemigo.PreparandoEmbestida:
                rb.linearVelocity = Vector2.zero;
                break;

            case EstadoEnemigo.Embestiendo:
                rb.MovePosition(rb.position + direccionEmbestida * velocidadEmbestida * Time.fixedDeltaTime);
                break;

            case EstadoEnemigo.Exhausto:
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    // Se activa en el fotograma que el jugador entra al área textualmente
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detectar si el cuerpo físico del enemigo tocó el agujero
        if (other.CompareTag("Agujero"))
        {
            CaerEnAgujero();
        }
    }

    private IEnumerator BucleDeComportamiento()
    {
        while (targetJugador != null)
        {
            // 1. ESTADO: PERSIGUIENDO (Dura 3 segundos)
            estadoActual = EstadoEnemigo.Persiguiendo;
            yield return new WaitForSeconds(3.0f);

            // 2. ESTADO: PREPARANDO EMBESTIDA (Se detiene 0.5 segundos)
            estadoActual = EstadoEnemigo.PreparandoEmbestida;
            direccionEmbestida = (targetJugador.position - transform.position).normalized;
            yield return new WaitForSeconds(0.5f); // <--- CORREGIDO A 0.5f para que coincida con tu diseño

            // 3. ESTADO: EMBESTIENDO (Aplica el dash por el tiempo configurado)
            estadoActual = EstadoEnemigo.Embestiendo;
            yield return new WaitForSeconds(duracionEmbestida);

            // 4. ESTADO: EXHAUSTO (Se queda quieto 1 segundo)
            estadoActual = EstadoEnemigo.Exhausto;
            yield return new WaitForSeconds(1.0f);
        }
    }


    private void CaerEnAgujero()
    {
        // Frenamos el bucle de embestidas
        StopAllCoroutines();

        rb.linearVelocity = Vector2.zero;

        // <--- CORREGIDO: Ahora sí iniciamos la corrutina que lo encoge antes de destruirlo
        StartCoroutine(AnimacionCaida());
    }

    public void IniciarPersecucion(Transform transformJugador)
    {
        if (targetJugador == null)
        {
            targetJugador = transformJugador;
            StartCoroutine(BucleDeComportamiento());
        }
    }

    private IEnumerator AnimacionCaida()
    {
        rb.simulated = false; // Desactiva físicas para que no se mueva mientras cae
        float tiempo = 0;
        Vector3 escalaInicial = transform.localScale;

        while (tiempo < 0.3f)
        {
            tiempo += Time.deltaTime;
            transform.localScale = Vector3.Lerp(escalaInicial, Vector3.zero, tiempo / 0.3f);
            yield return null;
        }

        ActivateItem();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificamos si el objeto con el que chocamos tiene el script "Movement" del jugador
        if (collision.gameObject.TryGetComponent<Movement>(out Movement jugador))
        {
            MatarAlJugador(collision.gameObject);
        }
    }

    private void MatarAlJugador(GameObject objetoJugador)
    {
        // Opcional: Aquí puedes instanciar partículas de sangre, explosión 
        // o reproducir un sonido antes de hacer desaparecer al jugador.

        // Destruye el objeto del jugador de la escena
        movement.Death();
        Destroy(objetoJugador);

        // Como el jugador ya no existe, hacemos que el enemigo vuelva a estar inactivo
        targetJugador = null;
        estadoActual = EstadoEnemigo.Inactivo;
        StopAllCoroutines();
        rb.linearVelocity = Vector2.zero;
    }

    private void ActivateItem()
    {
        flor.SetActive(true);
    }

}
