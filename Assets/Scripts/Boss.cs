using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Necessário para acessar o Slider

public class Boss : MonoBehaviour
{
    [Header("Atributos do Boss")]
    public float vidaMaxima = 500f;
    private float vidaAtual;
    public float moveSpeed = 2f;
    public string tagAgressor = "Power";
    public float danoAoPlayer = 20f;

    [Header("Elementos de UI")]
    public Slider barraDeVida; // A barra de vida que ficará na tela

    [Header("Mecânica de Spawn")]
    public float tempoDeSpawn = 5f;
    public GameObject[] minionsPrefabs; // Arraste os prefabs de jogadores/torcedores aqui

    private Transform target;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // 1. Configura a vida e a UI
        vidaAtual = vidaMaxima;
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaAtual;
        }

        // 2. Procura pelo Player
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            target = player.transform;
        }

        // 3. Inicia a rotina de invocar os ajudantes
        StartCoroutine(SpawnHelpersRoutine());
    }

    void Update()
    {
        // Mira na direção do player (se o jogo não estiver pausado)
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
            rb.linearVelocity = Vector2.zero; // Para de mover se pausar ou player sumir
        }
    }

    // Coroutine que cria os inimigos a cada 5 segundos
    private IEnumerator SpawnHelpersRoutine()
    {
        while (true) // Loop contínuo enquanto o boss estiver vivo
        {
            yield return new WaitForSeconds(tempoDeSpawn);

            // Só spawna se o jogo não estiver pausado e houver prefabs na lista
            if (!PauseManager.GameIsPaused && minionsPrefabs.Length > 0)
            {
                // Sorteia um ajudante da lista
                int randomIndex = Random.Range(0, minionsPrefabs.Length);
                GameObject lacaioEscolhido = minionsPrefabs[randomIndex];

                // TRAVA DE SEGURANÇA: Só cria o lacaio se o espaço não estiver vazio no Inspector
                if (lacaioEscolhido != null)
                {
                    // Sorteia uma posição um pouco em volta do boss (para não nascer dentro dele)
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
            TakeDamage(15); // Pode ajustar o dano recebido aqui
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

        // Atualiza a barra de vida na tela
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
        // Desliga a barra de vida do boss quando ele morre
        if (barraDeVida != null)
        {
            barraDeVida.gameObject.SetActive(false);
        }

        // AVISA O QUEST MANAGER DO BOSS QUE ELE FOI DERROTADO!
        if (QuestManagerBoss.Instance != null)
        {
            QuestManagerBoss.Instance.BossDerrotado();
        }

        Destroy(gameObject);
    }
}
