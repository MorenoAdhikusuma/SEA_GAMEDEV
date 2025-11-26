using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string spawnID;  // ID unik untuk spawn point
    public string SceneName;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.3f);
        }
}
