using UnityEngine;

public class Cube : MonoBehaviour
{
    void Start()
    {
        // CubeSpawner örneğine erişim
        var spawner = CubeSpawner.Instance;
        Debug.Log($"Spawner mevcut, materyal sayısı: {spawner.cubeMaterials.Length}");
    }
}
