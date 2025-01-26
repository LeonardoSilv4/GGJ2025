using UnityEngine;

public class Player : MonoBehaviour
{
    float inputH; //Teclas do Player
    [SerializeField] float speed; // Velocida...
    [SerializeField] float jumpForce; // Altura do Pulo...

    bool onGrouded; //Player no Ch�o

    Rigidbody2D rb;
    Animator anim;
    [SerializeField] SpriteRenderer sprRender;

    //Outras coisas que provavelmente n�o fica aqui
    [SerializeField] SpriteRenderer sprCity;
    [SerializeField] Sprite cityOn, cityOff;
    void Awake()
    {
        onGrouded = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        bool inputJump = Input.GetKeyDown(KeyCode.Space);
        
        //Player chamando pulo E esta no ch�o
        if (inputJump && onGrouded)
        {
            Jump();
            onGrouded = false;
        }

        //Trocar lado do Sprite do Player de acordo com a dire��o...
        switch (inputH)
        {
            case 1: sprRender.flipX = false; break;
            case -1: sprRender.flipX = true; break;

        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        transform.position += new Vector3(inputH * speed, 0f, 0f) * Time.fixedDeltaTime;
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Se o Player tocar no Ch�o
        if (coll.gameObject.CompareTag("floor"))
        {
            onGrouded = true; //Esta no Ch�o
        }
        //Se o Jogador tocar em uma Bolha
        if (coll.gameObject.CompareTag("bubble"))
        {
            //... E a bolha estiver a direita
            if(coll.gameObject.GetComponent<Transform>().position.x > 0)
            {
                //Trocar Pos. da Bolha (vai para o lado esquerdo)
                coll.gameObject.GetComponent<Transform>().position = new Vector2(-6f, -3.5f);
                sprCity.sprite = cityOff; //Fundo cidade cinza
            }
            //... E a Bolha estiver a esquerda
            else
            {
                //Trocar Pos. da Bolha (vai para o lado esquerdo)
                coll.gameObject.GetComponent<Transform>().position = new Vector2(6f, -3.5f);
                sprCity.sprite = cityOn; //Fundo cidade colorida

            }

        }
    }
}
