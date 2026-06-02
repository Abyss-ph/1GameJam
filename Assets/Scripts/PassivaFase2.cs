using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PassivaFase2 : MonoBehaviour
{
    [Header("Configurações da Passiva")]
    public float chanceBase = 0.09f;
    public float tempoDeChecagem = 10f;
    public float duracaoAtiva = 67f;
    public float danoNoModoDeus = 167f;

    [Header("Áudio")]
    public AudioSource audioSource;
    public AudioClip musicaModoDeus;

    [Header("Efeitos Visuais")]
    public Image overlayFlash;           // Image UI cobrindo a tela inteira (cor dourada, Alpha = 0)
    public ParticleSystem particulasGloria; // Partículas douradas subindo pela tela
    public GameObject textoModoDeus;     // TextMeshPro "MODO DEUS" no centro da tela
    public Animator animatorUI;          // Animator do HUD (trigger "GodMode" para câmera shake)

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

            float percentualFaltando = 1f - (vidaPlayer.VidaAtual / vidaPlayer.vidaMaxima);
            float bonusComeback = percentualFaltando * 50f;
            float chanceTotal = chanceAtual + bonusComeback;

            float rolagem = Random.Range(0f, 100f);

            if (rolagem <= chanceTotal)
            {
                yield return StartCoroutine(AtivarModoDeus());
                chanceAtual = chanceBase;
            }
            else
            {
                chanceAtual += 0.09f;
                Debug.Log($"Passiva falhou. Acumulada: {chanceAtual:F2}% | Bônus vida: +{bonusComeback:F2}% | Total na próxima: {chanceAtual + bonusComeback:F2}%");
            }
        }
    }

    IEnumerator AtivarModoDeus()
    {
        Debug.Log("Modo Deus Ativado!");

        // --- Mecânicas ---
        vidaPlayer.isImortal = true;
        sistemaArma.danoFixoOverwrite = danoNoModoDeus;

        // --- Áudio ---
        if (audioSource != null && musicaModoDeus != null)
        {
            audioSource.clip = musicaModoDeus;
            audioSource.Play();
        }

        // --- Visuais ---
        StartCoroutine(FlashTela());

        if (particulasGloria != null) particulasGloria.Play();
        if (textoModoDeus != null) textoModoDeus.SetActive(true);
        if (animatorUI != null) animatorUI.SetTrigger("GodMode");

        // --- Aguarda a duração ---
        yield return new WaitForSeconds(duracaoAtiva);

        // --- Desativa tudo ---
        vidaPlayer.isImortal = false;
        sistemaArma.danoFixoOverwrite = 0f;

        if (audioSource != null) audioSource.Stop();
        if (particulasGloria != null) particulasGloria.Stop();
        if (textoModoDeus != null) textoModoDeus.SetActive(false);

        Debug.Log("Modo Deus Acabou. Recomeçando sorteios...");
    }

    // Flash dourado que ilumina a tela no momento do jackpot
    IEnumerator FlashTela()
    {
        if (overlayFlash == null) yield break;

        Color cor = overlayFlash.color;

        // Sobe rápido: 0 → 0.8 em 0.05s
        for (float t = 0; t < 1f; t += Time.deltaTime / 0.3f)
        {
            cor.a = Mathf.Lerp(0f, 0.8f, t);
            overlayFlash.color = cor;
            yield return null;
        }

        // Desce devagar: 0.8 → 0 em 0.3s
        for (float t = 0; t < 1f; t += Time.deltaTime / 0.4f)
        {
            cor.a = Mathf.Lerp(0.8f, 0f, t);
            overlayFlash.color = cor;
            yield return null;
        }

        cor.a = 0f;
        overlayFlash.color = cor;
    }
}