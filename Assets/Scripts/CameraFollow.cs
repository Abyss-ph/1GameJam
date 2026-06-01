using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private void FixedUpdate()
    {
        transform.position = player.position + new Vector3(0, 0, -10);

    }

}
