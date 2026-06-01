using UnityEngine;

public class Pocao : MonoBehaviour
{
    public enum TipoPocao { Cura, Dano }

    [Header("Configurações")]
    public TipoPocao tipo;
    public float tempoDeVidaNoChao = 15f; // Se o player não pegar, ela some

    [Header("Se for Cura")]
    public float valorDeCura = 30f;

    [Header("Se for Dano")]
    public float multiplicador = 3f;
    public float duracaoDoBuff = 10f;

    void Start()
    {
        Destroy(gameObject, tempoDeVidaNoChao);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (tipo == TipoPocao.Cura)
            {
                VidaPlayer vida = collision.GetComponent<VidaPlayer>();
                if (vida != null) vida.Curar(valorDeCura);
            }
            else if (tipo == TipoPocao.Dano)
            {
                // Procura a arma para aplicar o triplo de dano
                SistemaArma arma = FindAnyObjectByType<SistemaArma>();
                if (arma != null) arma.AtivarBuffDeDano(multiplicador, duracaoDoBuff);
            }

            Destroy(gameObject);
        }
    }
}
