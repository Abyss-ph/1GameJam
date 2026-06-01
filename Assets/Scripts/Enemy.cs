using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Configurações de Velocidade")]
    public float minSpeed = 2f;  // Velocidade mínima do sorteio
    public float maxSpeed = 5f;  // Velocidade máxima do sorteio
    private float moveSpeed;     // Agora é privado, pois será sorteado automaticamente

    [Header("Ataque e Status")]
    public float danoAoPlayer = 10f;
    public float TakeDamag = 0f;
    public string tagAgressor = "Power";
    public float vida = 30f;

    Rigidbody2D rb;
    public Transform target;
    Vector2 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Sorteia uma velocidade aleatória entre o mínimo e o máximo assim que nasce!
        moveSpeed = Random.Range(minSpeed, maxSpeed);

        // 1. Avisa o Spawner que este inimigo nasceu
        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.RegisterEnemy();
        }

        // 2. Procura pelo Player
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    void Update()
    {
        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
        }
    }

    private void FixedUpdate()
    {
        if (target)
        {
            rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    // Chamado quando a colisão física ocorre
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto que tocou tem a tag correta (Tiro do jogador)
        if (collision.gameObject.CompareTag(tagAgressor))
        {
            float danoRecebido = 15f; // Valor padrão
            Bala scriptBala = collision.gameObject.GetComponent<Bala>();

            if (scriptBala != null)
            {
                danoRecebido = scriptBala.danoDaBala; // Lê o dano exato da bala!
            }

            TakeDamage(danoRecebido);
        }

        // Verifica se tocou no Jogador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Tenta achar o script de Vida no objeto que encostou
            VidaPlayer vidaDoPlayer = collision.gameObject.GetComponent<VidaPlayer>();

            // Se achou o script, causa o dano
            if (vidaDoPlayer != null)
            {
                vidaDoPlayer.TomarDano(danoAoPlayer);
            }
        }
    }

    // Método para reduzir a vida
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

        // AVISA O QUEST MANAGER QUE ESTE INIMIGO MORREU
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.RegistrarAbate();
        }

        Destroy(gameObject);
    }

    // ESTA FUNÇÃO É CHAMADA AUTOMATICAMENTE QUANDO O INIMIGO MORRE/É DESTRUÍDO
    private void OnDestroy()
    {
        // Avisa o Spawner que um inimigo saiu do jogo, abrindo vaga para outro
        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.UnregisterEnemy();
        }
    }
}
