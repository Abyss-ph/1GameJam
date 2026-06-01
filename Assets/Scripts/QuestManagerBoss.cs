using UnityEngine;
using TMPro; // Usado para textos modernos na Unity

public class QuestManagerBoss : MonoBehaviour
{
    // Permite que o Boss ache esse script facilmente
    public static QuestManagerBoss Instance { get; private set; }

    [Header("Elementos de UI")]
    public TextMeshProUGUI textoMissao; // Ex: "Objetivo: Derrote o Boss!"
    public GameObject painelVitoria;    // Arraste o seu Painel de Vitória aqui

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        painelVitoria.SetActive(false); // Garante que o painel começa escondido

        // Atualiza o texto logo no começo
        if (textoMissao != null)
        {
            textoMissao.text = "Derrote você mesmo no PRIME!";
        }
    }

    // Função que será chamada APENAS quando o Boss morrer
    public void BossDerrotado()
    {
        painelVitoria.SetActive(true); // Mostra a tela de vitória

        // Pausa o jogo
        Time.timeScale = 0f;
        PauseManager.GameIsPaused = true;
    }
}
