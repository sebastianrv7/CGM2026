using UnityEngine;

public class Pausa : MonoBehaviour

{
    [Header("Referencias")]
    [Tooltip("Arrastra aquí el objeto Canvas de tu menú de pausa.")]
    public GameObject canvasPausa;

    // Estado actual del juego (verdadero si está pausado)
    private bool juegoPausado = false;

    void Update()
    {
        // Detecta cuando el jugador presiona la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Reanudar()
    {
        // Oculta el Canvas de pausa
        canvasPausa.SetActive(false);

        // Restaura el tiempo del juego a la normalidad (1 = velocidad normal)
        Time.timeScale = 1f;

        // Cambia el estado
        juegoPausado = false;

        // Opcional: Desbloquear o bloquear el cursor según tu juego
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pausar()
    {
        // Muestra el Canvas de pausa
        canvasPausa.SetActive(true);

        // Congela el tiempo del juego (0 = detenido por completo)
        Time.timeScale = 0f;

        // Cambia el estado
        juegoPausado = true;

        // Opcional: Mostrar el cursor para que pueda hacer clic en los botones
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Método útil si tienes un botón de "Salir al menú principal"
    public void SalirAlMenu(string nombreEscenaMenu)
    {
        // ¡Importante! Asegúrate de restablecer el tiempo antes de cambiar de escena
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nombreEscenaMenu);
    }
}

