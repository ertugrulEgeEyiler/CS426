using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float destroyAfterSeconds = 3f; // Çarpmadan sonra yok olma süresi

    void Awake()
    {
        // Kurşunu belli bir süre sonra yok et
        Destroy(gameObject, destroyAfterSeconds);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Eğer çarpılan obje "Cube" ise CubeSpawner'ı tetikle
        if (collision.gameObject.CompareTag("Cube"))
        {
            CubeSpawner.Instance.OnCubeDestroyed(collision.gameObject);

            // Kurşunu yok et
            Destroy(gameObject);
        }
    }
}
