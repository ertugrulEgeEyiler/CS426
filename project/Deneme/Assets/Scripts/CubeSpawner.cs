using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab; // Küp prefab'ı
    public Material[] cubeMaterials; // Rastgele materyaller
    public float cubeSpacingY = 1.1f; // Küpler arası dikey boşluk
    public float fixedZ = -12.43f; // Sabit Z pozisyonu

    private Dictionary<float, List<GameObject>> columns = new Dictionary<float, List<GameObject>>();
    private Dictionary<string, int> materialNumbers = new Dictionary<string, int>();

    void Start()
    {
        AssignMaterialNumbers();
        SpawnInitialCubes();
    }

    // Her materyale bir sayı atar
    void AssignMaterialNumbers()
    {
        for (int i = 0; i < cubeMaterials.Length; i++)
        {
            materialNumbers[cubeMaterials[i].name] = i;
        }
    }

    // Oyunun başında küpleri oluştur
    void SpawnInitialCubes()
    {
        float startX = 5f;
        float endX = -4f;
        float step = (startX - endX) / 9f;

        for (int i = 0; i < 10; i++)
        {
            float xPos = startX - i * step;
            columns[xPos] = new List<GameObject>();

            for (int j = 0; j < 5; j++)
            {
                Vector3 position = new Vector3(xPos, 1f + j * cubeSpacingY, fixedZ);
                GameObject cube = SpawnSingleCube(position);
                columns[xPos].Add(cube);
            }
        }
    }

    // Tek bir küp oluştur
    GameObject SpawnSingleCube(Vector3 position)
    {
        GameObject newCube = Instantiate(cubePrefab, position, Quaternion.identity);
        Material randomMaterial = GetRandomMaterial();
        newCube.GetComponent<Renderer>().material = randomMaterial;
        newCube.GetComponent<Cube>().spawner = this;

        // Materyalin numarasını yazdır
        int materialNumber = materialNumbers[randomMaterial.name];
        Debug.Log($"Spawned cube with material: {randomMaterial.name}, Number: {materialNumber}");

        return newCube;
    }

    // Tıklanan küp ve bağlı küpler yok edilir
    public void OnCubeDestroyed(GameObject clickedCube)
    {
        StartCoroutine(DestroyConnectedCubes(clickedCube));
    }

    // Bağlı küpleri yok et
    IEnumerator DestroyConnectedCubes(GameObject startCube)
    {
        HashSet<GameObject> visited = new HashSet<GameObject>();
        Queue<GameObject> toProcess = new Queue<GameObject>();
        toProcess.Enqueue(startCube);

        while (toProcess.Count > 0)
        {
            GameObject currentCube = toProcess.Dequeue();
            if (visited.Contains(currentCube)) continue;

            visited.Add(currentCube);
            List<GameObject> connectedCubes = GetConnectedCubes(currentCube);

            foreach (GameObject cube in connectedCubes)
            {
                if (!visited.Contains(cube)) toProcess.Enqueue(cube);
            }
        }

        // Tüm bağlı küpleri sırayla yok et
        foreach (GameObject cube in visited)
        {
            float xPosition = cube.transform.position.x;

            // Küpü yok etmeden önce görsel efekt uygula
            StartCoroutine(AnimateCubeBeforeDestroy(cube));

            // Yok etmeden önce kısa bir süre bekle
            yield return new WaitForSeconds(0.2f);
            Destroy(cube);
            RemoveCubeFromColumn(xPosition, cube);
        }
    }

    // Küp yok edilmeden önce animasyon uygula
    IEnumerator AnimateCubeBeforeDestroy(GameObject cube)
    {
        Renderer renderer = cube.GetComponent<Renderer>();
        Color originalColor = renderer.material.color;

        // Renk değişimi efekti
        renderer.material.color = Color.black; // Yok olmadan önce siyah yap
        yield return new WaitForSeconds(0.1f);
        renderer.material.color = originalColor; // Eski rengine dön

        // Küpü küçültme efekti
        Vector3 originalScale = cube.transform.localScale;
        for (float t = 0; t < 1f; t += Time.deltaTime / 0.2f)
        {
            cube.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }
    }

    // Bağlı küpleri bul
    List<GameObject> GetConnectedCubes(GameObject cube)
    {
        List<GameObject> connectedCubes = new List<GameObject>();
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
                    connectedCubes.Add(other);
                }
            }
        }

        return connectedCubes;
    }

    // Sütundan küpü çıkar ve güncelle
    void RemoveCubeFromColumn(float xPosition, GameObject cube)
    {
        if (columns.ContainsKey(xPosition))
        {
            columns[xPosition].Remove(cube);

            // Küpleri aşağı kaydır
            for (int i = 0; i < columns[xPosition].Count; i++)
            {
                Vector3 newPosition = new Vector3(xPosition, 1f + i * cubeSpacingY, fixedZ);
                columns[xPosition][i].transform.position = newPosition;
            }

            // En üste yeni küp ekle
            float newY = 1f + columns[xPosition].Count * cubeSpacingY;
            Vector3 newCubePosition = new Vector3(xPosition, newY, fixedZ);
            GameObject newCube = SpawnSingleCube(newCubePosition);
            columns[xPosition].Add(newCube);
        }
    }

    // Rastgele materyal seç
    Material GetRandomMaterial()
    {
        return cubeMaterials[Random.Range(0, cubeMaterials.Length)];
    }
}
