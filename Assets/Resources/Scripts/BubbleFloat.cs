using UnityEngine;

public class BubbleFloat : MonoBehaviour
{
    public float floatForce = 0.5f;  // Força de flutuação (muito mais suave)
    public float bounceFactor = 0.3f;  // Fator de "recuo" mais suave
    public float drag = 0.1f;         // Resistência aumentada para desacelerar mais

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Aplica uma força de flutuação muito mais suave
        rb.AddForce(Vector2.up * floatForce, ForceMode2D.Force);

        // Aplica resistência à velocidade vertical para criar uma desaceleração mais suave
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Lerp(rb.linearVelocity.y, 0, drag));

        // Limita a velocidade da bolha para que ela não suba ou desça rapidamente
        if (rb.linearVelocity.y > 1) rb.linearVelocity = new Vector2(rb.linearVelocity.x, 1);  // Limita a velocidade para cima
        if (rb.linearVelocity.y < -1) rb.linearVelocity = new Vector2(rb.linearVelocity.x, -1);  // Limita a velocidade para baixo
    }

    // Quando a bolha bate no chão ou no teto, ela vai "quicar"
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y > 0) // Quando a bolha bate de cima para baixo (no teto)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Abs(rb.linearVelocity.y) * bounceFactor); // Aplica um "bounce" (recuo)
        }

        if (collision.relativeVelocity.y < 0) // Quando a bolha bate de baixo para cima (no chão)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -Mathf.Abs(rb.linearVelocity.y) * bounceFactor); // Aplica um "bounce" (recuo)
        }
    }
}