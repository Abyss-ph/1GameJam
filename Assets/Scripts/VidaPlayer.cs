using UnityEngine;
using UnityEngine.UI;

public class VidaPlayer : MonoBehaviour
{
    [Header("Atributos")]
    public float vidaMaxima = 100f;
    private float vidaAtual;

    [Header("Mecânicas Especiais")]
    public bool isImortal = false; // Usado para a passiva

    [Header("Elementos de UI")]
    public Slider barraDeVida;
    public GameObject painelDerrota;

    void Start()
    {
        vidaAtual = vidaMaxima;
        barraDeVida.maxValue = vidaMaxima;
        barraDeVida.value = vidaAtual;
        painelDerrota.SetActive(false);
    }

    public void TomarDano(float quantidadeDeDano)
    {
        // Se estiver imortal, ignora o dano
        if (isImortal) return;

        vidaAtual -= quantidadeDeDano;
        barraDeVida.value = vidaAtual;

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    // NOVA FUNÇÃO: Para a poção de vida
    public void Curar(float quantidade)
    {
        vidaAtual += quantidade;

        // Garante que a vida não passe do máximo
        if (vidaAtual > vidaMaxima) vidaAtual = vidaMaxima;

        barraDeVida.value = vidaAtual;
    }

    void Morrer()
    {
        painelDerrota.SetActive(true);
        Time.timeScale = 0f;
        PauseManager.GameIsPaused = true;
    }
}
