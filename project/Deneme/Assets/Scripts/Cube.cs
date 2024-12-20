using UnityEngine;

public class Cube : MonoBehaviour
{
    public CubeSpawner spawner;

    void OnMouseDown()
    {
        spawner.OnCubeDestroyed(gameObject);
    }
}