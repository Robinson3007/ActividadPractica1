using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    // Velocidad del personaje al caminar
    public float velocidad = 6f;

    // Fuerza utilizada para saltar
    public float fuerzaSalto = 10f;

    // Referencia al Rigidbody2D del personaje
    private Rigidbody2D rb;

    // Indica si el personaje está tocando el suelo o una plataforma
    private bool estaEnSuelo = false;

    void Start()
    {
        // Obtenemos el Rigidbody2D del personaje
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // =====================================
        // MOVIMIENTO HORIZONTAL
        // =====================================

        // A / Flecha izquierda = -1
        // D / Flecha derecha = 1
        float movimiento = Input.GetAxisRaw("Horizontal");

        // Movemos al personaje solamente en el eje X
        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );

        // =====================================
        // SALTO
        // =====================================

        // Solo puede saltar cuando está tocando
        // el suelo o una plataforma.
        if (Input.GetKeyDown(KeyCode.Space) && estaEnSuelo)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );
        }
    }

    // Se ejecuta mientras el personaje está
    // tocando una superficie.
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = true;
        }
    }

    // Se ejecuta cuando el personaje deja
    // de tocar la superficie.
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = false;
        }
    }
}