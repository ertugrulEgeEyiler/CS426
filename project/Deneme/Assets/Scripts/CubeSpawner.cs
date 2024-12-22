using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI Text sınıfı için gerekli

public class CubeSpawner : MonoBehaviour
{
    public static CubeSpawner Instance;

    public GameObject cubePrefab; // Küp prefab'ı
    public Material[] cubeMaterials; // Rastgele materyaller
    public float cubeSpacingY = 1f; // Küpler arası dikey boşluk
    public float fixedZ = -12.43f; // Sabit Z pozisyonu
    public int maxRows = 5; // Her sütundaki maksimum küp sayısı
    public Text scoreText; // Skor metni için UI öğesi
    public Text timerText; // Süreyi göstermek için UI öğesi
    public float gameDuration = 60f; // Oyun süresi (2 dakika)

    private Dictionary<float, List<GameObject>> columns = new Dictionary<float, List<GameObject>>();
    private Dictionary<string, int> materialNumbers = new Dictionary<string, int>();
    private int totalScore = 0; // Toplam puan
    private float remainingTime; // Kalan süre

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AssignMaterialNumbers();
        SpawnInitialCubes();
        remainingTime = gameDuration; // Süreyi başlat
        UpdateScoreUI();
        UpdateTimerUI();
        StartCoroutine(TimerCountdown());
    }

    void AssignMaterialNumbers()
    {
        for (int i = 0; i < cubeMaterials.Length; i++)
        {
            materialNumbers[cubeMaterials[i].name] = i;
        }
    }

    void SpawnInitialCubes()
    {
        float startX = 5f;
        float endX = -4f;
        float step = (startX - endX) / 9f;

        for (int i = 0; i < 10; i++)
        {
            float xPos = startX - i * step;
            columns[xPos] = new List<GameObject>();

            for (int j = 0; j < maxRows; j++)
            {
                Vector3 position = new Vector3(xPos, 1f + j * cubeSpacingY, fixedZ);
                GameObject cube = SpawnSingleCube(position);
                columns[xPos].Add(cube);
            }
        }
    }

    GameObject SpawnSingleCube(Vector3 position)
    {
        GameObject newCube = Instantiate(cubePrefab, position, Quaternion.identity);
        Material randomMaterial = GetRandomMaterial();
        newCube.GetComponent<Renderer>().material = randomMaterial;

        return newCube;
    }

    public void OnCubeDestroyed(GameObject clickedCube)
    {
        StartCoroutine(DestroyConnectedCubes(clickedCube));
    }

    IEnumerator DestroyConnectedCubes(GameObject startCube)
    {
        HashSet<GameObject> connectedCubes = GetConnectedCubes(startCube); // Bağlı tüm küpleri bul

        int destroyedCount = connectedCubes.Count; // Anlık yok edilen küp sayısını al

        foreach (GameObject cube in connectedCubes)
        {
            float xPosition = cube.transform.position.x;

            StartCoroutine(AnimateCubeBeforeDestroy(cube));
            yield return new WaitForSeconds(0.2f);
            Destroy(cube);
            RemoveCubeFromColumn(xPosition, cube);
        }

        // Yok edilen toplam küp sayısına göre puanı hesapla
        int score = CalculateScore(destroyedCount);
        totalScore += score;
        UpdateScoreUI(); // UI'daki skoru güncelle

        // Eğer 6 ya da daha fazla küp yok edildiyse süreye 10 saniye ekle
        if (destroyedCount >= 6)
        {
            AddTime(5f);
        }
    }

    IEnumerator AnimateCubeBeforeDestroy(GameObject cube)
    {
        Renderer renderer = cube.GetComponent<Renderer>();
        renderer.material.color = Color.black;
        yield return new WaitForSeconds(0.1f);

        Vector3 originalScale = cube.transform.localScale;
        for (float t = 0; t < 1f; t += Time.deltaTime / 0.2f)
        {
            cube.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }
    }

    HashSet<GameObject> GetConnectedCubes(GameObject cube)
    {
        HashSet<GameObject> visited = new HashSet<GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();

        queue.Enqueue(cube);

        while (queue.Count > 0)
        {
            GameObject currentCube = queue.Dequeue();

            if (visited.Contains(currentCube)) continue;
            visited.Add(currentCube);

            foreach (GameObject neighbor in GetNeighbors(currentCube))
            {
                if (!visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        return visited;
    }

    List<GameObject> GetNeighbors(GameObject cube)
    {
        List<GameObject> neighbors = new List<GameObject>();
        Material cubeMaterial = cube.GetComponent<Renderer>().material;
        string cubeMaterialName = cubeMaterial.name;

        float x = cube.transform.position.x;
        float y = cube.transform.position.y;

        foreach (float key in columns.Keys)
        {
            foreach (GameObject other in columns[key])
            {
                if (other == cube) continue;

                float otherX = other.transform.position.x;
                float otherY = other.transform.position.y;
                string otherMaterialName = other.GetComponent<Renderer>().material.name;

                if (otherMaterialName == cubeMaterialName &&
                    ((Mathf.Abs(x - otherX) < 0.1f && Mathf.Abs(y - otherY) < cubeSpacingY + 0.1f) || // Üst-alt
                     (Mathf.Abs(y - otherY) < 0.1f && Mathf.Abs(x - otherX) < 1.1f))) // Sağ-sol
                {
                    neighbors.Add(other);
                }
            }
        }

        return neighbors;
    }

    void RemoveCubeFromColumn(float xPosition, GameObject cube)
    {
        if (columns.ContainsKey(xPosition))
        {
            columns[xPosition].Remove(cube);

            for (int i = 0; i < columns[xPosition].Count; i++)
            {
                Vector3 newPosition = new Vector3(xPosition, 1f + i * cubeSpacingY, fixedZ);
                columns[xPosition][i].transform.position = newPosition;
            }

            if (columns[xPosition].Count < maxRows)
            {
                float newY = 1f + columns[xPosition].Count * cubeSpacingY;
                Vector3 newCubePosition = new Vector3(xPosition, newY, fixedZ);
                GameObject newCube = SpawnSingleCube(newCubePosition);
                columns[xPosition].Add(newCube);
            }
        }
    }

    Material GetRandomMaterial()
    {
        return cubeMaterials[Random.Range(0, cubeMaterials.Length)];
    }

    int CalculateScore(int connectedCubeCount)
    {
        if (connectedCubeCount < 3)
        {
            return connectedCubeCount * 50;
        }
        else if (connectedCubeCount >= 3 && connectedCubeCount <= 6)
        {
            return connectedCubeCount * 100;
        }
        else
        {
            return connectedCubeCount * 150;
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + totalScore;
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(remainingTime).ToString();
        }
    }

    IEnumerator TimerCountdown()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;
            UpdateTimerUI();
        }

        // Süre bittiğinde oyun sona erer
        Debug.Log("Game Over! Final Score: " + totalScore);
        EndGame();
    }

    void AddTime(float timeToAdd)
    {
        remainingTime += timeToAdd;
        UpdateTimerUI(); // Eklenen süreyi UI'da güncelle
    }

    void EndGame()
    {
        // Oyun bitişi için işlemler
        Debug.Log("Game Over!");
        // Buraya oyun bitiş ekranı veya tekrar başlatma mekanizması eklenebilir
    }
}
