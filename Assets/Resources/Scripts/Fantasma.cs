using UnityEngine;

public class Fantasma : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float moveSpeed, jumpForce;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        this.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y); //Movimentação do Fantasma
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("JumpTrig"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); //Pular :)
        }
    }
}
