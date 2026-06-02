using System.Collections;
using UnityEngine;
 
public class SistemaMusica : MonoBehaviour
{
    public static SistemaMusica Instance;
 
    [Header("Audio Sources")]
    [SerializeField] AudioSource sourceFase;
    [SerializeField] AudioSource sourcePoder;
 
    [Header("Músicas")]
    [SerializeField] AudioClip musicaFase;
 
    // Indica se a música do poder está no controle agora
    private bool poderAtivo = false;
 
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
 
    void Start()
    {
        sourceFase.clip = musicaFase;
        sourceFase.loop = true;
        sourceFase.Play();
    }
 
    // -------------------------------------------------------
    // Chamado pelo PassivaFase2 ao ativar o Modo Deus
    // Recebe o AudioClip do poder direto da Passiva
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
        // Só retoma a fase se o poder não estiver no controle
        if (!poderAtivo)
            sourceFase.UnPause();
        else
            sourcePoder.UnPause();
    }
 
    // Troca a música da fase (útil ao mudar de área)
    public void TrocarMusicaFase(AudioClip novaMusica)
    {
        sourceFase.Stop();
        sourceFase.clip = novaMusica;
        sourceFase.Play();
    }
}