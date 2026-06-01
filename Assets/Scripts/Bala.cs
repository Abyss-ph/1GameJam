using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float tempoDeVida;

    // Variável pública para a arma definir o dano antes de atirar
    [HideInInspector] public float danoDaBala = 15f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Inimigo") || collision.gameObject.CompareTag("Boss") || collision.gameObject.CompareTag("Cenario"))
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    private void FixedUpdate()
    {
        transform.Translate(transform.up * speed * Time.fixedDeltaTime, Space.World);
    }
}

