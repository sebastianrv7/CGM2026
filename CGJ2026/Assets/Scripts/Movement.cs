using UnityEngine;
using System.Collections;   

public class Movement : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private bool isWalking = false;


    [SerializeField] private float dashVelocity = 20f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 1f;

    private bool canDash = true;
    private bool isDashing = false;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector2 inputs;
    private Vector2 direccionFinal;
    private Vector2 direccionDash;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (isDashing) return;

        // Captura las entradas puras del teclado (-1, 0, o 1)
        inputs.x = Input.GetAxisRaw("Horizontal"); // A / D
        inputs.y = Input.GetAxisRaw("Vertical");   // W / S

        direccionFinal = new Vector2(inputs.x, inputs.y);

        if (direccionFinal.magnitude > 0)
        {
            // Si se está moviendo y antes estaba quieto, actualiza el estado
            if (!isWalking)
            {
                isWalking = true;
                animator.SetBool("IsWalking", true);
            }

            // Normalizamos para que las diagonales no vayan más rápido
            direccionFinal = direccionFinal.normalized;

            // LOGICA DEL FLIP: 
            // Si presiona Izquierda (A), inputs.x es -1 -> Activamos flip
            if (inputs.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            // Si presiona Derecha (D), inputs.x es 1 -> Desactivamos flip
            else if (inputs.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                // Guardamos la dirección actual del movimiento para el impulso
                direccionDash = direccionFinal;
                StartCoroutine(EjecutarDash());


            }

        }
        else if (direccionFinal.magnitude == 0 && isWalking)
        {
            // Si se detiene, cambia el estado a quieto
            isWalking = false;
            animator.SetBool("IsWalking", false);
        }
    }
    

    
    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.MovePosition(rb.position + direccionDash * dashVelocity * Time.fixedDeltaTime);
        }
        else
        {
            // Aplica el movimiento normal en línea recta
            rb.MovePosition(rb.position + direccionFinal * velocidad * Time.fixedDeltaTime);
        }
    }

    private IEnumerator EjecutarDash()
    {
        canDash = false;
        isDashing = true;

        // Opcional: Aquí puedes activar un trigger de animación si tienes una para el dash
        // animator.SetTrigger("Dash"); 

        // Espera lo que dura el impulso
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        // Espera el tiempo de recarga antes de poder usarlo otra vez
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
