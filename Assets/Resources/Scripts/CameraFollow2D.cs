using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Referências")]
    public Transform player; // O transform do jogador que a câmera deve seguir

    [Header("Configurações de Movimentação")]
    public Vector3 offset = new Vector3(0, 1, -10); // Deslocamento da câmera em relação ao jogador
    public float smoothSpeed = 0.125f; // Velocidade de suavização do movimento da câmera

    [Header("Limites da Câmera")]
    public bool useBounds = false; // Ativar/desativar limites
    public Vector2 minBounds; // Limites mínimos (x, y) da câmera
    public Vector2 maxBounds; // Limites máximos (x, y) da câmera

    void FixedUpdate()
    {
        if (player == null) return;

        // Calcula a posição desejada da câmera

        //Não "Seguir" posição Y/altura do Player
        Vector3 FixPlayerPos = player.position;
        FixPlayerPos.y = -5.404423f;
        Vector3 desiredPosition = FixPlayerPos + offset;
        
        //Vector3 desiredPosition = player.position + offset;

        // Aplica suavização ao movimento da câmera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Aplica os limites se ativados
        if (useBounds)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
            //smoothedPosition.y = 0f;
        }

        // Atualiza a posição da câmera
        transform.position = smoothedPosition;
    }
}