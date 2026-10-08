using System.Reflection.Metadata;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    
    public float velocidad = 6f;

    
    public float fuerzaSalto = 10f;

    
    private Rigidbody2D rb;

    
    private bool estaEnSuelo = false;

    private Vector3 localScale;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
    }

    void Update()
    {
        

        
        float movimiento = Input.GetAxisRaw("Horizontal");

        
        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );

        if(movimiento != 0)
        {
            
            transform.localScale = new Vector3(
                Mathf.Sign(movimiento) * localScale.x, localScale.y, localScale.z);
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
}