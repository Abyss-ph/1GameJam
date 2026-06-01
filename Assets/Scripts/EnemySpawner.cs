using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField] private GameObject torcedor1BrPrefab;
    //[SerializeField] private GameObject torcedor2BrPrefab;
    //[SerializeField] private GameObject torcedor3BrPrefab;
    [SerializeField] private GameObject torcedor1WPrefab;
    [SerializeField] private GameObject jogador1WPrefab;
    //[SerializeField] private GameObject jogador1BrPrefab;
    //[SerializeField] private GameObject jogador2BrPrefab;
    //[SerializeField] private GameObject jogador3BrPrefab;

    //[SerializeField] private float torcedor1BrInterval = 1.0f;
    //[SerializeField] private float torcedor2BrInterval = 1.0f;
    //[SerializeField] private float torcedor3BrInterval = 1.0f;
    [SerializeField] private float torcedor1WInterval = 1.5f;
    [SerializeField] private float jogador1WInterval = 2.5f;
    //[SerializeField] private float jogador1BrInterval = 1.0f;
    //[SerializeField] private float jogador2BrInterval = 1.0f;
    //[SerializeField] private float jogador3BrInterval = 1.0f;

    [SerializeField] private int maxEnemies = 22; // maximo de Npcs
    private int currentEnemyCount = 0; // Contador atual

    [Header("Limites do Campo de Batalha")]
    [SerializeField] private float minX = -245.7f;
    [SerializeField] private float maxX = 237.4f;
    [SerializeField] private float minY = -103.7f;
    [SerializeField] private float maxY = 110.1f;


    // Propriedade estática para que os inimigos consigam aceder facilmente ao Spawner
    public static EnemySpawner Instance { get; private set; }

    private void Awake()
    {
        // Configura o Singleton para o Spawner
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(spawnEnemy(torcedor1WInterval, torcedor1WPrefab));
        StartCoroutine(spawnEnemy(jogador1WInterval, jogador1WPrefab));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);

        // SÓ CRIA O INIMIGO SE ESTIVER ABAIXO O LIMITE
        if (currentEnemyCount < maxEnemies)
        {
            // Sorteia as posições X e Y dentro das dimensões reais da sua arena
            float posX = Random.Range(minX, maxX);
            float posY = Random.Range(minY, maxY);

            // Cria o inimigo na posição sorteada gigantesca
            GameObject newEnemy = Instantiate(enemy, new Vector3(posX, posY, 0), Quaternion.identity);

            // O próprio método do inimigo vai aumentar o contador (vê o script Enemy abaixo)
        }

        // Continua o loop do Spawner normalmente
        StartCoroutine(spawnEnemy(interval, enemy));
    }

    // Funções públicas para os inimigos alterarem o contador
    public void RegisterEnemy()
    {
        currentEnemyCount++;
    }

    public void UnregisterEnemy()
    {
        currentEnemyCount--;
        // Garante que o contador nunca fique negativo por erro
        if (currentEnemyCount < 0) currentEnemyCount = 0;
    }
}
