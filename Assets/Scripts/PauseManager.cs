using UnityEngine;
 
public class PauseManager : MonoBehaviour
{
    public static bool GameIsPaused = false;
 
    [Header("Elementos de UI")]
    public GameObject pauseMenuUI;
    public GameObject pauseButton;
 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused) Resume();
            else              Pause();
        }
    }
 
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        pauseButton.SetActive(false);
 
        Time.timeScale = 0f;
        GameIsPaused   = true;
 
        // Pausa a música junto com o jogo
        if (SistemaMusica.Instance != null)
            SistemaMusica.Instance.PausarMusica();
    }
 
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        pauseButton.SetActive(true);
 
        Time.timeScale = 1f;
        GameIsPaused   = false;
 
        // Retoma a música que estava tocando
        if (SistemaMusica.Instance != null)
            SistemaMusica.Instance.RetomarMusica();
    }
}