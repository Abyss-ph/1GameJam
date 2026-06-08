using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class SistemaMusica : MonoBehaviour
{
    public static SistemaMusica Instance;
 
    [Header("Audio Sources")]
    [SerializeField] AudioSource sourceFase;
    [SerializeField] AudioSource sourcePoder;
 
    [Header("Músicas por Cena")]
    [Tooltip("Nome exato da cena de menu (deixe vazio para não tocar música)")]
    public string nomeDoMenu = "Menu";
 
    [Tooltip("Música da Fase 1")]
    public AudioClip musicaFase1;
 
    [Tooltip("Nome exato da cena da Fase 1")]
    public string nomeFase1 = "Fase1";
 
    [Tooltip("Música da Fase 2")]
    public AudioClip musicaFase2;
 
    [Tooltip("Nome exato da cena da Fase 2")]
    public string nomeFase2 = "Fase2";
 
    // Indica se a música do poder está no controle agora
    private bool poderAtivo = false;
 
    void Awake()
    {
        // Sem DontDestroyOnLoad: cada cena cria sua própria instância
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
 
    void Start()
    {
        string cenaAtual = SceneManager.GetActiveScene().name;
 
        if (cenaAtual == nomeDoMenu)
        {
            // No menu: não toca nenhuma música (ou adicione uma musicaMenu se quiser)
            sourceFase.Stop();
            sourcePoder.Stop();
        }
        else if (cenaAtual == nomeFase1 && musicaFase1 != null)
        {
            IniciarMusicaFase(musicaFase1);
        }
        else if (cenaAtual == nomeFase2 && musicaFase2 != null)
        {
            IniciarMusicaFase(musicaFase2);
        }
    }
 
    private void IniciarMusicaFase(AudioClip clip)
    {
        sourceFase.clip = clip;
        sourceFase.loop = true;
        sourceFase.Play();
    }
 
    // -------------------------------------------------------
    // Chamado pelo PassivaFase2 ao ativar o Modo Deus
    // -------------------------------------------------------
    public void AtivarMusicaPoder(AudioClip clipPoder)
    {
        if (poderAtivo) return;
        poderAtivo = true;
 
        sourceFase.Pause();
 
        sourcePoder.clip = clipPoder;
        sourcePoder.loop = false;
        sourcePoder.Play();
 
        StartCoroutine(AguardarMusicaPoder());
    }
 
    private IEnumerator AguardarMusicaPoder()
    {
        yield return new WaitWhile(() => sourcePoder.isPlaying);
 
        poderAtivo = false;
        sourceFase.UnPause();
    }
 
    // Cancela o poder antes da música acabar (segurança)
    public void CancelarMusicaPoder()
    {
        if (!poderAtivo) return;
 
        StopAllCoroutines();
        sourcePoder.Stop();
        poderAtivo = false;
        sourceFase.UnPause();
    }
 
    // -------------------------------------------------------
    // Chamado pelo PauseManager
    // -------------------------------------------------------
    public void PausarMusica()
    {
        sourceFase.Pause();
        sourcePoder.Pause();
    }
 
    public void RetomarMusica()
    {
        if (!poderAtivo)
            sourceFase.UnPause();
        else
            sourcePoder.UnPause();
    }
 
    // -------------------------------------------------------
    // Chamado pelo Menu.cs antes de trocar de cena
    // -------------------------------------------------------
    public void PararTudo()
    {
        StopAllCoroutines();
        sourceFase.Stop();
        sourcePoder.Stop();
        poderAtivo = false;
    }
}