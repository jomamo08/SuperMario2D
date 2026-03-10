using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float velocidadMovimiento = 5f;
    public float fuerzaSaltoInicial = 8f;
    public float fuerzaMantenida = 15f;
    public float tiempoMaximoSalto = 0.35f;
    public LayerMask capaSuelo;

    public Vector3 escalaGrande = new Vector3(0.9f, 0.9f, 1f);
    public Vector3 escalaPequena = new Vector3(0.45f, 0.45f, 1f);
    public float tiempoInmortal = 2f;

    private Rigidbody2D rb;
    private bool grounded;
    private bool esGrande = true;
    private bool inmortal = false;

    private bool saltando;
    private float cronometroSalto;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.localScale = escalaGrande;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
        {
            saltando = true;
            grounded = false;
            cronometroSalto = tiempoMaximoSalto;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSaltoInicial);
        }

        if (Input.GetKey(KeyCode.UpArrow) && saltando)
        {
            if (cronometroSalto > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSaltoInicial);
                cronometroSalto -= Time.deltaTime;
            }
            else
            {
                saltando = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            saltando = false;
        }
    }

    void FixedUpdate()
    {
        float movimiento = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(movimiento * velocidadMovimiento, rb.linearVelocity.y);

        if (movimiento > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        else if (movimiento < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & capaSuelo) != 0)
        {
            grounded = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 normal = collision.contacts[0].normal;

            if (normal.y > 0.5f)
            {
                Destroy(collision.gameObject);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * (fuerzaSaltoInicial * 1.2f), ForceMode2D.Impulse);
            }
            else
            {
                RecibirDanio();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & capaSuelo) != 0)
        {
            grounded = false;
        }
    }

    void RecibirDanio()
    {
        if (inmortal) return;

        if (esGrande)
        {
            esGrande = false;
            transform.localScale = escalaPequena;
            StartCoroutine(Inmunidad());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Inmunidad()
    {
        inmortal = true;
        yield return new WaitForSeconds(tiempoInmortal);
        inmortal = false;
    }
}