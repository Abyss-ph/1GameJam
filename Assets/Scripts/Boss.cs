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

    private Transform target;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

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
        if (target && !PauseManager.GameIsPaused)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
        }
    }

    private void FixedUpdate()
    {
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
        // Verifica se o Boss tomou um tiro
        if (collision.gameObject.CompareTag(tagAgressor))
        {
            // --- CORREÇÃO: lê o dano direto do script da Bala, igual ao Enemy.cs ---
            float danoRecebido = 15f; // valor de segurança caso a bala não tenha o script
            Bala scriptBala = collision.gameObject.GetComponent<Bala>();

            if (scriptBala != null)
            {
                danoRecebido = scriptBala.danoDaBala; // Pega o dano real (pode ser 167 no Modo Deus!)
            }

            TakeDamage(danoRecebido);
        }

        // Verifica se o Boss encostou no Player
        if (collision.gameObject.CompareTag("Player"))
        {
            VidaPlayer vidaDoPlayer = collision.gameObject.GetComponent<VidaPlayer>();
            if (vidaDoPlayer != null)
            {
                vidaDoPlayer.TomarDano(danoAoPlayer);
            }
        }
    }

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