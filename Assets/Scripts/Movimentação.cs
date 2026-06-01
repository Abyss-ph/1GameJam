using UnityEngine;

public class Movimentação : MonoBehaviour
{

    //Variável pública que o valor poderá ser trocado quando quiser na engine
    public float Speed;
    //Inicialização das variáveis que irão pegar os componentes no Player
    private Rigidbody2D Rig;

    //Método que é chamado uma vez ao iniciar

    void Start()
    {
        //As variáveis pegam o componente do Player
        Rig = GetComponent<Rigidbody2D>();

    }
    void Update()
    {

        if (PauseManager.GameIsPaused)
        {
            Rig.linearVelocity = Vector2.zero;
            return;
        }

        #region Movement and animation

        //Essa linha irá fazer a movimentação básica do Player
        Rig.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * Speed, Input.GetAxis("Vertical") * Speed);

        //Se a velocidade X do Player for maior que 0 (indo pra direita) a variável recebe 1
        if (Rig.linearVelocity.x > 0f) { }
        //Ou se a velocidade X do Player for menor que 0 (indo pra esquerda) a variável recebe 3
        else if (Rig.linearVelocity.x < 0f) { }
        ;
        //Se a velocidade Y do Player for maior que 0 (indo pra cima) a variável recebe 0
        if (Rig.linearVelocity.y > 0f) { }
        //Ou se a velocidade Y do Player for menor que 0 (indo pra baixo) a variável recebe 2
        else if (Rig.linearVelocity.y < 0f) { }
        ;

        #endregion
    }
}
