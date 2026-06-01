using UnityEngine;
using TMPro; // Usado para textos modernos na Unity

public class QuestManager : MonoBehaviour
{
    // Permite que outros scripts achem esse facilmente
    public static QuestManager Instance { get; private set; }

    [Header("Configurações da Missão")]
    public int metaDeAbates = 1000;
    private int abatesAtuais = 0;

    [Header("Elementos de UI")]
    public TextMeshProUGUI textoMissao; // Arraste o seu Texto do contador aqui
    public GameObject painelVitoria;    // Arraste o seu Painel de Vitória aqui

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        painelVitoria.SetActive(false); // Garante que o painel começa escondido
        AtualizarTexto();
    }

    // Função que será chamada sempre que um inimigo morrer
    public void RegistrarAbate()
    {
        if (abatesAtuais < metaDeAbates)
        {
            abatesAtuais++;
            AtualizarTexto();

            // Verifica se bateu a meta
            if (abatesAtuais >= metaDeAbates)
            {
                CompletarMissao();
            }
        }
    }

    private void AtualizarTexto()
    {
        textoMissao.text = "Inimigos: " + abatesAtuais + " / " + metaDeAbates;
    }

    private void CompletarMissao()
    {
        painelVitoria.SetActive(true); // Mostra a tela de vitória

        // Pausa o jogo usando a mesma lógica que já fizemos!
        Time.timeScale = 0f;
        PauseManager.GameIsPaused = true;
    }
}
