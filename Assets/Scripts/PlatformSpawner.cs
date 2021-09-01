using UnityEngine;

public partial class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private MovingPlatform platformPrefab;
    [SerializeField] private MoveDirection moveDirection;

    public void SpawnPlatform()
    {
        MovingPlatform platform = Instantiate(platformPrefab);

        if (MovingPlatform.lastPlatform != null && MovingPlatform.lastPlatform.gameObject != GameObject.Find("StartPlatform"))
        {
            float xDirection = moveDirection == MoveDirection.X || 
                               moveDirection == MoveDirection.Xnegative ? 
            transform.position.x : MovingPlatform.lastPlatform.transform.position.x;

            float zDirection = moveDirection == MoveDirection.Z || 
                               moveDirection == MoveDirection.Znegative ? 
            transform.position.z : MovingPlatform.lastPlatform.transform.position.z;

            platform.transform.position = new Vector3(xDirection,
               MovingPlatform.lastPlatform.transform.position.y + platform.transform.localScale.y,
               zDirection);
        }

        else
        {
            platform.transform.position = transform.position;
        }
        platform.MoveDirection = moveDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, platformPrefab.transform.localScale);
    }
}
