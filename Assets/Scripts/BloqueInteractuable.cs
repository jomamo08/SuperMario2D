using UnityEngine;

public class BloqueInteractuable : MonoBehaviour
{
    public LayerMask capaEnemigo;
    public float radioDeteccion = 0.5f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 normal = collision.contacts[0].normal;

            if (normal.y > 0.5f)
            {
                GolpeDesdeAbajo();
            }
        }
    }

    void GolpeDesdeAbajo()
    {
        Vector2 posicionDeteccion = (Vector2)transform.position + Vector2.up * 0.6f;
        Collider2D enemigo = Physics2D.OverlapBox(posicionDeteccion, new Vector2(0.8f, 0.5f), 0, capaEnemigo);

        if (enemigo != null)
        {
            MovimientoEnemigo scriptEnemigo = enemigo.GetComponent<MovimientoEnemigo>();
            if (scriptEnemigo != null)
            {
                scriptEnemigo.MorirPorBloque();
            }
        }
    }
}