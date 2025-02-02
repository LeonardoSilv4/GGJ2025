using UnityEngine;

public class PlataformMove : MonoBehaviour
{
    [SerializeField] float speed;
    bool canMove;
    [SerializeField] float limitePosX;
    void FixedUpdate()
    {
        if (canMove)
        transform.position = new Vector2(transform.position.x + speed * Time.fixedDeltaTime, transform.position.y);

        if(transform.position.x >= limitePosX) canMove = false;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            canMove = true;
        }
    }
}
