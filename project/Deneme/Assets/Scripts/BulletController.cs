using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject explosionEffect; // (Opsiyonel) Patlama efekti prefab'ı
    public float destroyAfterSeconds = 2f; // Çarpmadan sonra yok olma süresi

    void Start()
    {
        // Kurşunu 2 saniye sonra yok et
        Destroy(gameObject, destroyAfterSeconds);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Çarptığı obje bir "Cube" ise, kutuyu yok et
        if (collision.gameObject.CompareTag("Cube"))
        {
            // Eğer bir patlama efekti varsa, oluştur
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, collision.transform.position, Quaternion.identity);
            }

            Destroy(collision.gameObject); // Kutuyu yok et
        }

        // Çarpmadan sonra kurşunu yok et
        Destroy(gameObject);
    }
}
