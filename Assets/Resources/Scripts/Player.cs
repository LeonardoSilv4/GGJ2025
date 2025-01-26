using UnityEngine;

public class Player : MonoBehaviour
{
    float inputH;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    [SerializeField] bool onGrouded;

    Rigidbody2D rb;
    [SerializeField] SpriteRenderer sprRender;

    //Outras coisas que provavelmente não fica aqui
    [SerializeField] SpriteRenderer sprCity;
    [SerializeField] Sprite cityOn, cityOff;
    void Awake()
    {
        onGrouded = true;
        rb = GetComponent<Rigidbody2D>();
        //sprRender = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        bool inputJump = Input.GetKeyDown(KeyCode.Space);
        //print(Input.GetKeyDown(KeyCode.Escape));
        
        if (inputJump && onGrouded)
        {
            Jump();
            onGrouded = false;
        }

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

        if (coll.gameObject.CompareTag("floor"))
        {
            onGrouded = true;
        }
        if (coll.gameObject.CompareTag("bubble"))
        {
            //float posX = coll.gameObject.GetComponent<Transform>().position.x;
            if(coll.gameObject.GetComponent<Transform>().position.x > 0)
            {
                coll.gameObject.GetComponent<Transform>().position = new Vector2(-6f, -3.5f);
                sprCity.sprite = cityOff;
            }
            else
            {
                coll.gameObject.GetComponent<Transform>().position = new Vector2(6f, -3.5f);
                sprCity.sprite = cityOn;

            }

        }
    }
}
