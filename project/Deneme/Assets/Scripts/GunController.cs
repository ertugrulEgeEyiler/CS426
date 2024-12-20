using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab; // Kurşun prefab'ı
    public Transform firePoint; // Kurşunun çıkış noktası
    public float bulletSpeed = 20f;

    private GameObject currentBullet; // Mevcut kurşunu takip etmek için

    void Update()
    {
        // Mouse sol tık ile ateş et
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Eğer sahnede bir kurşun varsa, onu yok et
        if (currentBullet != null)
        {
            Destroy(currentBullet);
        }

        // Yeni kurşunu oluştur
        currentBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = currentBullet.GetComponent<Rigidbody>();

        // Kurşuna hız ver
        rb.velocity = firePoint.forward * bulletSpeed;
    }
}
