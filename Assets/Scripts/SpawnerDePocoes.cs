using System.Collections;
using UnityEngine;

public class SpawnerDePocoes : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    public GameObject pocaoCuraPrefab;
    public GameObject pocaoDanoPrefab;
    public float intervaloSpawnCura = 20f;
    public float intervaloSpawnDano = 30f;

    [Header("Limites do Campo de Batalha")]
    public float minX = -245.7f;
    public float maxX = 237.4f;
    public float minY = -103.7f;
    public float maxY = 110.1f;

    void Start()
    {
        if (pocaoCuraPrefab) StartCoroutine(SpawnPocao(intervaloSpawnCura, pocaoCuraPrefab));
        if (pocaoDanoPrefab) StartCoroutine(SpawnPocao(intervaloSpawnDano, pocaoDanoPrefab));
    }

    private IEnumerator SpawnPocao(float intervalo, GameObject pocaoPrefab)
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalo);

            // Sorteia o X entre a ponta esquerda e a ponta direita
            float posX = Random.Range(minX, maxX);

            // Sorteia o Y entre a ponta de baixo e a ponta de cima
            float posY = Random.Range(minY, maxY);

            // Cria a posição final
            Vector3 posAleatoria = new Vector3(posX, posY, 0);

            // Spawna a poção
            Instantiate(pocaoPrefab, posAleatoria, Quaternion.identity);
        }
    }
}
