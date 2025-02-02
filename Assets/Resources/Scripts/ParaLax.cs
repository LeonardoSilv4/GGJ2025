using UnityEngine;

public class ParaLax : MonoBehaviour
{
    private float length, posX;
    private Transform cam;

    [SerializeField] float parallaxValor;

    void Start()
    {
        cam = Camera.main.transform;
        posX = transform.transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float RePos = cam.transform.position.x * (1 -  parallaxValor);
        float Distance = cam.transform.position.x * parallaxValor;
        transform.position = new Vector3(posX + Distance,transform.position.y,transform.position.z);

        if(RePos > posX + length)
        {
            posX += length;
        }
        else if(RePos < posX - length)
        {
            posX -= length;
        }
    }
}
