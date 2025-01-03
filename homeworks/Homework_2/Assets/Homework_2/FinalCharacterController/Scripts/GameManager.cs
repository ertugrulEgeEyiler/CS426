using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton tasarımı

    private int keyCount = 0; // Toplanan anahtar sayısı

    private void Awake()
    {
        // Singleton: Tek bir GameManager örneği olmasını sağlar
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişimlerinde yok edilmez
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Anahtar toplama işlemi
    public void CollectKey(GameObject keyObject)
    {
        keyCount++; // Anahtar sayısını artır
        Debug.Log("Anahtar toplandı! Toplam Anahtar: " + keyCount);
        Destroy(keyObject); // Anahtarı yok et
    }

    // Anahtar sayısını kontrol eden bir fonksiyon
    public int GetKeyCount()
    {
        return keyCount;
    }
}
