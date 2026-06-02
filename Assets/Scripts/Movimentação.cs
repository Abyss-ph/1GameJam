using UnityEngine;
 
public class Movimentação : MonoBehaviour
{
    // Variável pública que o valor poderá ser trocado quando quiser na engine
    public float Speed;
 
    // Inicialização das variáveis que irão pegar os componentes no Player
    private Rigidbody2D Rig;
    private Animator anim;
 
    // Parâmetro usado no Animator para detectar movimento
    private static readonly int EstaMovendoParam = Animator.StringToHash("estaMovendo");
 
    // Limiar mínimo de velocidade para considerar que está se movendo
    [SerializeField] float limiarMovimento = 0.05f;
 
    void Start()
    {
        Rig  = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
 
    void Update()
    {
        if (PauseManager.GameIsPaused)
        {
            Rig.linearVelocity = Vector2.zero;
            AtualizarAnimacao(false);
            return;
        }
 
        #region Movement and animation
 
        // Movimentação básica do Player
        Rig.linearVelocity = new Vector2(
            Input.GetAxis("Horizontal") * Speed,
            Input.GetAxis("Vertical")   * Speed
        );
 
        // Atualiza a animação com base na velocidade atual
        bool estaMovendo = Rig.linearVelocity.magnitude > limiarMovimento;
        AtualizarAnimacao(estaMovendo);
 
        // Direção horizontal (caso queira espelhar o sprite no futuro)
        if (Rig.linearVelocity.x > 0f)  { /* indo para direita */ }
        else if (Rig.linearVelocity.x < 0f) { /* indo para esquerda */ }
 
        // Direção vertical
        if (Rig.linearVelocity.y > 0f)  { /* indo para cima   */ }
        else if (Rig.linearVelocity.y < 0f) { /* indo para baixo  */ }
 
        #endregion
    }
 
    // Envia o parâmetro "estaMovendo" (bool) para o Animator
    private void AtualizarAnimacao(bool movendo)
    {
        if (anim != null)
            anim.SetBool(EstaMovendoParam, movendo);
    }
}