using System.Collections;
using UnityEngine;

public class PassivaFase2 : MonoBehaviour
{
    [Header("Configurações da Passiva")]
    public float chanceBase = 0.09f;
    public float tempoDeChecagem = 10f;
    public float duracaoAtiva = 67f;
    public float danoNoModoDeus = 167f;

    [Header("Áudio")]
    public AudioSource audioSource; // Arraste o AudioSource do Player aqui
    public AudioClip musicaModoDeus; // Arraste a música épica aqui

    private float chanceAtual;
    private VidaPlayer vidaPlayer;
    private SistemaArma sistemaArma;

    void Start()
    {
        vidaPlayer = GetComponent<VidaPlayer>();
        sistemaArma = FindAnyObjectByType<SistemaArma>();
        chanceAtual = chanceBase;

        StartCoroutine(CicloDeSorteio());
    }

    IEnumerator CicloDeSorteio()
    {
        while (true)
        {
            yield return new WaitForSeconds(tempoDeChecagem);

            // Bônus de comeback: quanto menos vida, maior o bônus (máx +50%)
            // Exemplo: 10% de vida restante → 90% de vida faltando → +45% de bônus
            float percentualFaltando = 1f - (vidaPlayer.VidaAtual / vidaPlayer.vidaMaxima);
            float bonusComeback = percentualFaltando * 50f;

            float chanceTotal = chanceAtual + bonusComeback;

            // Tira um número de 0 a 100
            float rolagem = Random.Range(0f, 100f);

            if (rolagem <= chanceTotal)
            {
                // SUCESSO! Ativa o modo e espera ele acabar para continuar
                yield return StartCoroutine(AtivarModoDeus());
                chanceAtual = chanceBase; // Reseta a chance acumulada
            }
            else
            {
                // FALHOU. Soma 0.09 para a próxima checagem
                chanceAtual += 0.09f;
                Debug.Log($"Passiva falhou. Acumulada: {chanceAtual:F2}% | Bônus vida: +{bonusComeback:F2}% | Total na próxima: {chanceAtual + bonusComeback:F2}%");
            }
        }
    }

    IEnumerator AtivarModoDeus()
    {
        Debug.Log("Modo Deus Ativado!");

        // Ativa Imortalidade e Dano
        vidaPlayer.isImortal = true;
        sistemaArma.danoFixoOverwrite = danoNoModoDeus;

        // Troca a música se tiver
        if (audioSource != null && musicaModoDeus != null)
        {
            audioSource.clip = musicaModoDeus;
            audioSource.Play();
        }

        // Aguarda os 67 segundos
        yield return new WaitForSeconds(duracaoAtiva);

        // Desativa tudo
        vidaPlayer.isImortal = false;
        sistemaArma.danoFixoOverwrite = 0f;

        if (audioSource != null)
        {
            audioSource.Stop();
        }
        Debug.Log("Modo Deus Acabou. Recomeçando sorteios...");
    }
}
