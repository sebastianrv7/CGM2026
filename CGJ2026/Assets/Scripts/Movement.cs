using UnityEngine;

public class Movement : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private Vector2 inputs;
    private Vector2 direccionFinal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Captura las entradas puras del teclado (-1, 0, o 1)
        inputs.x = Input.GetAxisRaw("Horizontal"); // A / D
        inputs.y = Input.GetAxisRaw("Vertical");   // W / S

        // Creamos el vector de movimiento directo
        direccionFinal = new Vector2(inputs.x, inputs.y);

        // Si hay movimiento, normalizamos para que las diagonales no vayan más rápido
        if (direccionFinal.magnitude > 0)
        {
            direccionFinal = direccionFinal.normalized;
        }
    }

    void FixedUpdate()
    {
        // Aplica el movimiento en línea recta (X horizontal, Y vertical)
        rb.MovePosition(rb.position + direccionFinal * velocidad * Time.fixedDeltaTime);
    }
}
