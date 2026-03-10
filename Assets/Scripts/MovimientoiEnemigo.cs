using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    public float velocidad = 2f;
    private Rigidbody2D rb;
    private int direccion;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
        {
            enabled = false;
            return;
        }
        direccion = Random.value < 0.5f ? -1 : 1;
        ActualizarGiro();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direccion * velocidad, rb.linearVelocity.y);
    }

    void Update()
    {
        ComprobarWrapPantalla();
    }

    void ComprobarWrapPantalla()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.x > 1) viewportPos.x = 0;
        else if (viewportPos.x < 0) viewportPos.x = 1;
        transform.position = Camera.main.ViewportToWorldPoint(viewportPos);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 normal = collision.contacts[0].normal;
            if (normal.y < -0.5f)
            {
                Morir();
                return;
            }
        }

        direccion *= -1;
        ActualizarGiro();
    }

    void ActualizarGiro()
    {
        transform.localScale = new Vector3(direccion, 1, 1);
    }

    public void MorirPorBloque()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 5f);
        rb.angularVelocity = 500f;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 2f);
    }

    void Morir()
    {
        Destroy(gameObject);
    }
}