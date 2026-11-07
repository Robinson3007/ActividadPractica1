
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MovimientoJugador : MonoBehaviour
{
    // ==========================================
    // CONFIGURACIÓN DEL MOVIMIENTO DEL JUGADOR
    // ==========================================

    public float velocidad = 6f;
    public float fuerzaSalto = 10f;

    private Rigidbody2D rb;
    private bool estaEnSuelo = false;
    private Vector3 localScale;

    // ==========================================
    // SISTEMA DE RECOLECCIÓN DE PASTELES
    // ==========================================

    public int totalPasteles = 10;
    private int pastelesRecogidos = 0;

    // ==========================================
    // INTERFAZ DEL JUEGO
    // ==========================================

    [Header("Interfaz del juego")]
    public TMP_Text contadorPasteles;
    public TMP_Text mensajeNivel;

    // ==========================================
    // SISTEMA DE ELIMINACIÓN
    // ==========================================

    [Header("Sistema de eliminación")]
    public GameObject mensajeEliminado;
    public float segundosAntesDeReiniciar = 2f;

    private bool muerto = false;

    // ==========================================
    // INICIO DEL JUEGO
    // ==========================================

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;

        if (contadorPasteles != null)
        {
            contadorPasteles.text =
                "Pasteles: 0/" + totalPasteles;
        }

        if (mensajeNivel != null)
        {
            mensajeNivel.text =
                "¡Recoge los 10 pasteles para completar el nivel!";
        }

        // Mantener oculto el mensaje de derrota
        if (mensajeEliminado != null)
        {
            mensajeEliminado.SetActive(false);
        }

        Debug.Log("Juego iniciado. Recoge los 10 pasteles.");
    }

    // ==========================================
    // MOVIMIENTO Y SALTO DEL PERSONAJE
    // ==========================================

    void Update()
    {
        // No permitir movimiento cuando Shinobi muere
        if (muerto) return;

        float movimiento = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );

        if (movimiento != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(movimiento) * localScale.x,
                localScale.y,
                localScale.z
            );
        }

        if (Input.GetKeyDown(KeyCode.Space) && estaEnSuelo)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );
        }
    }

    // ==========================================
    // DETECCIÓN DEL SUELO
    // ==========================================

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = false;
        }
    }

    // ==========================================
    // SISTEMA DE RECOLECCIÓN DE PASTELES
    // ==========================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        // No recoger pasteles después de morir
        if (muerto) return;

        if (other.CompareTag("Collectible"))
        {
            pastelesRecogidos++;

            Debug.Log(
                "Pastel recogido: " + other.gameObject.name
            );

            Debug.Log(
                "Pasteles recogidos: " +
                pastelesRecogidos + "/" + totalPasteles
            );

            if (contadorPasteles != null)
            {
                contadorPasteles.text =
                    "Pasteles: " +
                    pastelesRecogidos + "/" + totalPasteles;
            }

            Destroy(other.gameObject);

            if (pastelesRecogidos >= totalPasteles)
            {
                Debug.Log(
                    "¡FELICIDADES! Has recogido todos los pasteles."
                );

                if (mensajeNivel != null)
                {
                    mensajeNivel.text = "¡NIVEL COMPLETADO!";
                }
            }
        }
    }

    // ==========================================
    // PRÁCTICA NO. 4: DETECCIÓN DE ENEMIGOS
    // ==========================================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (muerto) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("¡El enemigo tocó al jugador!");

            Morir();
        }
    }

    // ==========================================
    // SISTEMA DE DERROTA DEL JUGADOR
    // ==========================================

    public void Morir()
    {
        // Evitar que se ejecute varias veces
        if (muerto) return;

        muerto = true;

        // Detener el movimiento del personaje
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // Mostrar el mensaje de eliminación
        if (mensajeEliminado != null)
        {
            mensajeEliminado.SetActive(true);
        }

        Debug.Log("Shinobi ha sido eliminado.");

        // Esperar antes de reiniciar la escena
        Invoke(nameof(ReiniciarEscena), segundosAntesDeReiniciar);
    }

    // ==========================================
    // REINICIAR EL NIVEL
    // ==========================================

    private void ReiniciarEscena()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
