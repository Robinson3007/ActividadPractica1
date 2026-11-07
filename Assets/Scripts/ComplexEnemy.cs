
using UnityEngine;

public class ComplexEnemy : MonoBehaviour
{
    [Header("Configuración del enemigo")]
    public float velocidad = 3f;

    [Header("Sistema de detección")]
    public float distanciaDeteccion = 6f;

    [Header("Objetivo")]
    public Transform jugador;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // Si no hay jugador, detener al enemigo.
        if (jugador == null)
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            ActualizarAnimacion();
            return;
        }

        // Calcular la distancia hasta Shinobi.
        float distancia = Vector2.Distance(
            transform.position,
            jugador.position
        );

        float direccion = 0f;

        // Perseguir solamente si Shinobi está cerca.
        if (distancia <= distanciaDeteccion)
        {
            float diferenciaX =
                jugador.position.x - transform.position.x;

            if (Mathf.Abs(diferenciaX) > 0.2f)
            {
                direccion = Mathf.Sign(diferenciaX);
            }
        }

        // Movimiento horizontal.
        rb.linearVelocity = new Vector2(
            direccion * velocidad,
            rb.linearVelocity.y
        );

        // Girar visualmente hacia Shinobi.
        if (direccion != 0f && spriteRenderer != null)
        {
            spriteRenderer.flipX = direccion < 0f;
        }

        // Actualizar Idle o Run.
        ActualizarAnimacion();
    }

    void ActualizarAnimacion()
    {
        if (animator != null)
        {
            animator.SetFloat(
                "Velocidad",
                Mathf.Abs(rb.linearVelocity.x)
            );
        }
    }

    // Mostrar el rango de detección en Unity.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            distanciaDeteccion
        );
    }
}
