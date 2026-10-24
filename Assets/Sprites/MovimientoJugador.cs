
using UnityEngine;
using TMPro;

public class MovimientoJugador : MonoBehaviour
{
    
    public float velocidad = 6f;
    public float fuerzaSalto = 10f;

    private Rigidbody2D rb;
    private bool estaEnSuelo = false;
    private Vector3 localScale;

    public int totalPasteles = 10;
    private int pastelesRecogidos = 0;

    [Header("Interfaz del juego")]
    public TMP_Text contadorPasteles;
    public TMP_Text mensajeNivel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;

        if (contadorPasteles != null)
        {
            contadorPasteles.text = "Pasteles: 0/" + totalPasteles;
        }

        if (mensajeNivel != null)
        {
            mensajeNivel.text =
                "¡Recoge los 10 pasteles para completar el nivel!";
        }

        Debug.Log("Juego iniciado. Recoge los 10 pasteles.");
    }

    void Update()
    {
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

   
    private void OnTriggerEnter2D(Collider2D other)
    {
        
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
}
