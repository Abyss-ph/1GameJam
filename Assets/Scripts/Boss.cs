using System.Collections;
using UnityEngine;
using UnityEngine.UI;
 
public class Boss : MonoBehaviour
{
    [Header("Atributos do Boss")]
    public float vidaMaxima = 500f;
    private float vidaAtual;
    public float moveSpeed = 2f;
    public string tagAgressor = "Power";
    public float danoAoPlayer = 20f;
 
    [Header("Elementos de UI")]
    public Slider barraDeVida;
 
    [Header("Mecânica de Spawn")]
    public float tempoDeSpawn = 5f;
    public GameObject[] minionsPrefabs;
 
    [Header("Knockback ao Tomar Tiro")]
    public float forcaKnockback = 5f;       // Boss é pesado: força menor que o inimigo comum
    public float duracaoStun = 0.2f;        // Stun curto para o boss não ficar muito parado
 
    [Header("Knockback ao Colidir com Player")]
    public float forcaKnockbackNoPlayer = 4f;  // Empurrão ao bater no player
    public float duracaoStunColisao = 0.4f;    // Stun ao bater no player
 
    private Transform target;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
 
    // Controle interno do stun
    private bool estaAtordoado = false;
 
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
 
    void Start()
    {
        vidaAtual = vidaMaxima;
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaAtual;
        }
 
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            target = player.transform;
        }
 
        StartCoroutine(SpawnHelpersRoutine());
    }
 
    void Update()
    {
        // Só atualiza a direção se não estiver em pausa ou atordoado
        if (target && !PauseManager.GameIsPaused && !estaAtordoado)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
        }
    }
 
    private void FixedUpdate()
    {
        // Enquanto atordoado, não sobrescreve a velocidade para o knockback agir
        if (estaAtordoado) return;
 
        if (target && !PauseManager.GameIsPaused)
        {
            rb.linearVelocity = moveDirection * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
 
    private IEnumerator SpawnHelpersRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(tempoDeSpawn);
 
            if (!PauseManager.GameIsPaused && minionsPrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, minionsPrefabs.Length);
                GameObject lacaioEscolhido = minionsPrefabs[randomIndex];
 
                if (lacaioEscolhido != null)
                {
                    Vector3 offsetSpawn = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
                    Instantiate(lacaioEscolhido, transform.position + offsetSpawn, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("O Boss tentou invocar um lacaio, mas o espaço " + randomIndex + " está vazio no Inspector!");
                }
            }
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
 
            // Knockback: empurra o boss para longe da bala
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
 
            // Knockback: empurra o boss para longe do player após bater
            Vector2 direcaoRepulsao = (transform.position - collision.transform.position).normalized;
            AplicarKnockback(direcaoRepulsao, forcaKnockbackNoPlayer, duracaoStunColisao);
        }
    }
 
    // ─── Aplica o impulso e inicia o stun ───────────────────────────────
    private void AplicarKnockback(Vector2 direcao, float forca, float duracao)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direcao * forca, ForceMode2D.Impulse);
 
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
        vidaAtual -= damageAmount;
 
        if (barraDeVida != null)
        {
            barraDeVida.value = vidaAtual;
        }
 
        if (vidaAtual <= 0)
        {
            Die();
        }
    }
 
    void Die()
    {
        if (barraDeVida != null)
        {
            barraDeVida.gameObject.SetActive(false);
        }
 
        if (QuestManagerBoss.Instance != null)
        {
            QuestManagerBoss.Instance.BossDerrotado();
        }
 
        Destroy(gameObject);
    }
}