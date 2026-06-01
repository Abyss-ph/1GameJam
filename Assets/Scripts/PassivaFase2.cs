using System.Collections;
using UnityEngine;

public class PassivaFase2 : MonoBehaviour
{
    [Header("Configurações da Passiva")]
    public float chanceBase = 3.7f;
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

            // Tira um número de 0 a 100
            float rolagem = Random.Range(0f, 100f);

            if (rolagem <= chanceAtual)
            {
                // SUCESSO! Ativa o modo e espera ele acabar para continuar
                yield return StartCoroutine(AtivarModoDeus());
                chanceAtual = chanceBase; // Reseta a chance
            }
            else
            {
                // FALHOU. Dobra a chance para a próxima vez
                chanceAtual *= 2f;
                Debug.Log("Passiva falhou. Nova chance: " + chanceAtual + "%");
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
