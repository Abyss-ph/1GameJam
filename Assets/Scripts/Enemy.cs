using System.Collections;
using UnityEngine;
 
public class Enemy : MonoBehaviour
{
    [Header("Configurações de Velocidade")]
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
    private float moveSpeed;
 
    [Header("Ataque e Status")]
    public float danoAoPlayer = 10f;
    public float TakeDamag = 0f;
    public string tagAgressor = "Power";
    public float vida = 30f;
 
    [Header("Knockback ao Tomar Tiro")]
    public float forcaKnockback = 8f;       // Força do empurrão ao ser atingido
    public float duracaoStun = 0.3f;        // Segundos travado após levar tiro
 
    [Header("Knockback ao Colidir com Player")]
    public float forcaKnockbackNoPlayer = 6f;  // Força que empurra o INIMIGO ao bater no player
    public float duracaoStunColisao = 0.5f;    // Segundos travado após bater no player
 
    Rigidbody2D rb;
    public Transform target;
    Vector2 moveDirection;
 
    // Controle interno do stun
    private bool estaAtordoado = false;
 
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
 
    void Start()
    {
        moveSpeed = Random.Range(minSpeed, maxSpeed);
 
        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.RegisterEnemy();
        }
 
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }
 
    void Update()
    {
        // Só atualiza a direção se não estiver atordoado
        if (target && !estaAtordoado)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
        }
    }
 
    private void FixedUpdate()
    {
        // Enquanto atordoado, o Rigidbody já recebeu o impulso do knockback.
        // Não sobrescrevemos a velocidade aqui para o impulso agir livremente.
        if (estaAtordoado) return;
 
        if (target)
        {
            rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ── Levou um tiro ──────────────────────────────────────────────
        if (collision.gameObject.CompareTag(tagAgressor))
        {
            float danoRecebido = 15f;
            Bala scriptBala = collision.gameObject.GetComponent<Bala>();
            if (scriptBala != null)
            {
                danoRecebido = scriptBala.danoDaBala;
            }
 
            // Knockback: empurra o inimigo para LONGE da bala
            Vector2 direcaoKnockback = (transform.position - collision.transform.position).normalized;
            AplicarKnockback(direcaoKnockback, forcaKnockback, duracaoStun);
 
            TakeDamage(danoRecebido);
        }
 
        // ── Bateu no Player ────────────────────────────────────────────
        if (collision.gameObject.CompareTag("Player"))
        {
            VidaPlayer vidaDoPlayer = collision.gameObject.GetComponent<VidaPlayer>();
            if (vidaDoPlayer != null)
            {
                vidaDoPlayer.TomarDano(danoAoPlayer);
            }
 
            // Knockback: empurra o inimigo para LONGE do player após bater
            Vector2 direcaoRepulsao = (transform.position - collision.transform.position).normalized;
            AplicarKnockback(direcaoRepulsao, forcaKnockbackNoPlayer, duracaoStunColisao);
        }
    }
 
    // ─── Aplica o impulso e inicia o stun ───────────────────────────────
    private void AplicarKnockback(Vector2 direcao, float forca, float duracao)
    {
        // Para a velocidade atual antes de aplicar o impulso
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direcao * forca, ForceMode2D.Impulse);
 
        // Reinicia o stun (cancela coroutine anterior se ainda estiver rodando)
        StopCoroutine(nameof(RotinaSairDoStun));
        StartCoroutine(RotinaSairDoStun(duracao));
    }
 
    private IEnumerator RotinaSairDoStun(float duracao)
    {
        estaAtordoado = true;
        yield return new WaitForSeconds(duracao);
        estaAtordoado = false;
    }
 
    // ─── Dano e morte ───────────────────────────────────────────────────
    void TakeDamage(float damageAmount)
    {
        vida -= damageAmount;
        if (vida <= 0)
        {
            Die();
        }
    }
 
    void Die()
    {
        Debug.Log("Objeto destruído!");
 
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.RegistrarAbate();
        }
 
        Destroy(gameObject);
    }
 
    private void OnDestroy()
    {
        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.UnregisterEnemy();
        }
    }
}