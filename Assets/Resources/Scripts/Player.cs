using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimenta��o")]
    [SerializeField] float moveSpeed = 5f; // Velocidade de movimento
    [SerializeField] float jumpForce = 10f; // For�a do pulo    
    private float moveInput; //Input Horizontal do Player

    [Header("Detec��o de Solo")]
    [SerializeField] Transform groundCheck; // Ponto para verificar se est� no ch�o
    private float groundCheckRadius = 0.2f; // Raio para detectar o ch�o (Tamanho)
    [SerializeField] LayerMask groundLayer; // Camada que representa o ch�o (Camada ch�o)

    private Rigidbody2D rb; //.. Do Player
    [SerializeField] private bool isGrounded; //Se esta no ch�o

    [Header("Refer�ncias")]

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
            // Movimenta��o horizontal
            moveInput = Input.GetAxisRaw("Horizontal");
            if (moveInput == 1) sprRender.flipX = false; //Virar sprite do player para direita
            if (moveInput == -1) sprRender.flipX = true; //... esquerda

            if (moveInput != 0 && isGrounded) anim.SetBool("Running", true);
            else anim.SetBool("Running", false);

            // Verificar se est� no ch�o
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            if (isGrounded) anim.SetBool("Jumping", false);
            if (!isGrounded) anim.SetBool("Jumping", true); //Se N�O esta no ch�o, anima��o de Pulando

            // Se teclar pula for chamada e Player esta no ch�o
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
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y); //Movimenta��o do Player
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
        // Desenhar a �rea de detec��o do ch�o para debug
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
