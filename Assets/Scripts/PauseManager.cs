using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // Variável estática para sabermos em qualquer script se o jogo está pausado
    public static bool GameIsPaused = false;

    [Header("Elementos de UI")]
    public GameObject pauseMenuUI; // O painel grande de pause
    public GameObject pauseButton; // O botão físico de pausar que fica na tela

    void Update()
    {
        // Atalho opcional pelo teclado (ESC)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Esta função será chamada pelo BOTÃO DE PAUSE da tela
    public void Pause()
    {
        pauseMenuUI.SetActive(true);  // Mostra o painel grande de pause
        pauseButton.SetActive(false); // FAZ O BOTÃO DE PAUSE DESAPARECER

        Time.timeScale = 0f;          // Congela o tempo do jogo
        GameIsPaused = true;
    }

    // Esta função será chamada pelo seu NOVO BOTÃO DE DESPAUSAR (dentro do painel)
    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Esconde o painel grande de pause
        pauseButton.SetActive(true);  // FAZ O BOTÃO DE PAUSE REAPARECER NA TELA

        Time.timeScale = 1f;          // Volta o tempo ao normal
        GameIsPaused = false;
    }
}
