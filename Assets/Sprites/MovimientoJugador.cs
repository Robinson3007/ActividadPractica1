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
    // INICIO DEL JUEGO
    // ==========================================

    void Start()
    {
        // Obtener el Rigidbody2D del personaje
        rb = GetComponent<Rigidbody2D>();

        // Guardar la escala original del personaje
        localScale = transform.localScale;


        // Inicializar el contador de pasteles
        if (contadorPasteles != null)
        {
            contadorPasteles.text =
                "Pasteles: 0/" + totalPasteles;
        }


        // Mostrar mensaje inicial
        if (mensajeNivel != null)
        {
            mensajeNivel.text =
                "¡Recoge los 10 pasteles para completar el nivel!";
        }


        Debug.Log(
            "Juego iniciado. Recoge los 10 pasteles."
        );
    }


    // ==========================================
    // MOVIMIENTO Y SALTO DEL PERSONAJE
    // ==========================================

    void Update()
    {
        // Detectar movimiento horizontal
        float movimiento = Input.GetAxisRaw("Horizontal");


        // Aplicar movimiento al personaje
        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );


        // Cambiar orientación del personaje
        if (movimiento != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(movimiento) * localScale.x,
                localScale.y,
                localScale.z
            );
        }


        // Detectar salto con la barra espaciadora
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
        // Comprobar si el personaje está tocando el suelo
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = true;
        }
    }


    // ==========================================
    // DETECTAR CUANDO EL PERSONAJE SALE DEL SUELO
    // ==========================================

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
        // Detectar objetos coleccionables
        if (other.CompareTag("Collectible"))
        {
            // Aumentar contador
            pastelesRecogidos++;


            // Mostrar pastel recogido en consola
            Debug.Log(
                "Pastel recogido: " + other.gameObject.name
            );


            // Mostrar cantidad de pasteles recogidos
            Debug.Log(
                "Pasteles recogidos: " +
                pastelesRecogidos + "/" + totalPasteles
            );


            // Actualizar contador en pantalla
            if (contadorPasteles != null)
            {
                contadorPasteles.text =
                    "Pasteles: " +
                    pastelesRecogidos + "/" + totalPasteles;
            }


            // Eliminar el pastel recogido
            Destroy(other.gameObject);


            // Comprobar si se recogieron todos los pasteles
            if (pastelesRecogidos >= totalPasteles)
            {
                Debug.Log(
                    "¡FELICIDADES! Has recogido todos los pasteles."
                );


                // Mostrar mensaje de nivel completado
                if (mensajeNivel != null)
                {
                    mensajeNivel.text =
                        "¡NIVEL COMPLETADO!";
                }
            }
        }
    }


    // ==========================================
    // PRÁCTICA NO. 4: DETECCIÓN DE ENEMIGOS
    // ==========================================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprobar si el personaje tocó un enemigo
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(
                "¡El enemigo tocó al jugador!"
            );


            Debug.Log(
                "Reiniciando el nivel..."
            );


            // Reiniciar la escena actual
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }
    }
}