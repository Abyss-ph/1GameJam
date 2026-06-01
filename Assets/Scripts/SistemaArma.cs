using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class SistemaArma : MonoBehaviour
{
    Vector2 mousePosi;
    Vector2 dirArma;
    float angle;

    [SerializeField] SpriteRenderer srGun;
    [SerializeField] float tempoEntreTiros;
    bool podeAtirar = true;

    [SerializeField] Transform pontoDeFogo;
    [SerializeField] GameObject tiro;

    [Header("Atributos de Dano")]
    public float danoBase = 15f;
    [HideInInspector] public float multiplicadorDano = 1f; // Poção de dano
    [HideInInspector] public float danoFixoOverwrite = 0f; // Passiva Fase 2 (167)

    void Update()
    {
        if (PauseManager.GameIsPaused) return;

        mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0) && podeAtirar == true)
        {
            podeAtirar = false;

            // Cria a bala
            GameObject novaBala = Instantiate(tiro, pontoDeFogo.position, pontoDeFogo.rotation);

            // Passa o dano atual para a bala
            Bala scriptBala = novaBala.GetComponent<Bala>();
            if (scriptBala != null)
            {
                if (danoFixoOverwrite > 0)
                    scriptBala.danoDaBala = danoFixoOverwrite; // Passiva ativa (167)
                else
                    scriptBala.danoDaBala = danoBase * multiplicadorDano; // Normal ou Poção (x3)
            }

            Invoke("CDTiro", tempoEntreTiros);
        }
    }

    private void FixedUpdate()
    {
        if (PauseManager.GameIsPaused) return;

        dirArma = mousePosi - (Vector2)transform.position;
        angle = Mathf.Atan2(dirArma.y, dirArma.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void CDTiro()
    {
        podeAtirar = true;
    }

    // Chamado pela poção de dano
    public void AtivarBuffDeDano(float multiplicador, float duracao)
    {
        StartCoroutine(BuffDanoRoutine(multiplicador, duracao));
    }

    private IEnumerator BuffDanoRoutine(float multiplicador, float duracao)
    {
        multiplicadorDano = multiplicador;
        yield return new WaitForSeconds(duracao);
        multiplicadorDano = 1f; // Retorna ao normal
    }
}