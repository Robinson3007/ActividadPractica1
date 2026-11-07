
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    [Header("Configuración del enemigo")]
    public float velocidad = 2f;

    [Header("Objetivo")]
    public Transform jugador;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (jugador == null)
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            if (animator != null)
                animator.SetFloat("Velocidad", 0f);

            return;
        }

        // Calcular la dirección hacia Shinobi
        float diferenciaX =
            jugador.position.x - transform.position.x;

        float direccion = 0f;

        if (Mathf.Abs(diferenciaX) > 0.2f)
        {
            direccion = Mathf.Sign(diferenciaX);
        }

        // Mover al enemigo horizontalmente
        rb.linearVelocity = new Vector2(
            direccion * velocidad,
            rb.linearVelocity.y
        );

        // Orientar el sprite hacia Shinobi
        if (direccion != 0 && spriteRenderer != null)
        {
            spriteRenderer.flipX = direccion < 0;
        }

        // Actualizar la animación según el movimiento
        if (animator != null)
        {
            animator.SetFloat(
                "Velocidad",
                Mathf.Abs(rb.linearVelocity.x)
            );
        }
    }
}
