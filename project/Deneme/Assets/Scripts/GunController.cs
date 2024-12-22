using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab; // Kurşun prefab'ı
    public Transform firePoint; // Kurşunun çıkış noktası
    public float bulletSpeed = 20f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = firePoint.forward * bulletSpeed;
        }
    }
}
