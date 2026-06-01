using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadScenes(string cena)
    {
        // 1. Devolve o tempo ao normal (caso o jogo estivesse pausado)
        Time.timeScale = 1f;

        // 2. Avisa os scripts de movimento/arma que o jogo NÃO está mais pausado
        // Como a variável é estática, podemos alterá-la de qualquer lugar
        PauseManager.GameIsPaused = false;

        // 3. Carrega a nova cena normalmente
        SceneManager.LoadScene(cena);
    }
}

