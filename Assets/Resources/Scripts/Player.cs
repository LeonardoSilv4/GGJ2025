using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] float moveSpeed = 5f; // Velocidade de movimento
    [SerializeField] float jumpForce = 10f; // Força do pulo    
    private float moveInput; //Input Horizontal do Player

    [Header("Detecção de Solo")]
    [SerializeField] Transform groundCheck; // Ponto para verificar se está no chão
    private float groundCheckRadius = 0.2f; // Raio para detectar o chão (Tamanho)
    [SerializeField] LayerMask groundLayer; // Camada que representa o chão (Camada chão)

    private Rigidbody2D rb; //.. Do Player
    [SerializeField] private bool isGrounded; //Se esta no chão

    [Header("Referências")]

    [SerializeField] GameManager gManager; //Script "Main" que Gerencia o game no Geral
    [SerializeField] Transform spawnPoint; 

    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sprRender;

    bool canControll;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        canControll = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            gManager.GameOver();
        }
        if (canControll)
        {
            // Movimentação horizontal
            moveInput = Input.GetAxisRaw("Horizontal");
            if (moveInput == 1) sprRender.flipX = false; //Virar sprite do player para direita
            if (moveInput == -1) sprRender.flipX = true; //... esquerda

            if (moveInput != 0 && isGrounded) anim.SetBool("Running", true);
            else anim.SetBool("Running", false);

            // Verificar se está no chão
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            if (isGrounded) anim.SetBool("Jumping", false);
            if (!isGrounded) anim.SetBool("Jumping", true); //Se NÃO esta no chão, animação de Pulando

            // Se teclar pula for chamada e Player esta no chão
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                transform.SetParent(null);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); //Pular :)
                gManager.JumpSound();
            }
        }
        if (transform.position.y < -12f) gManager.GameOver();

    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y); //Movimentação do Player
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("bubble"))
        {
            gManager.BubblePow();
            Destroy(coll.gameObject); //Destruir Bolha
        }

        if (coll.CompareTag("LiberFan")) gManager.LiberarFantasma();
        if (coll.gameObject.name == "GameFinal") gManager.finalGame();
    }
    private void OnDrawGizmosSelected()
    {
        // Desenhar a área de detecção do chão para debug
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void OnCollisionEnter2D(Collision2D colll)
    {
        if (colll.gameObject.CompareTag("plataformMove"))
        {
            transform.SetParent(colll.gameObject.transform);
        }

        if (colll.gameObject.CompareTag("FansTag"))
        {
            canControll = false;
            anim.SetTrigger("GoodBye");
            rb.bodyType = RigidbodyType2D.Static;
            GetComponent<CapsuleCollider2D>().enabled = false;
            StartCoroutine(AtrasoReset());
        }

    }

    IEnumerator AtrasoReset()
    {
        yield return new WaitForSeconds(2f);
        gManager.GameOver();
    }
}
