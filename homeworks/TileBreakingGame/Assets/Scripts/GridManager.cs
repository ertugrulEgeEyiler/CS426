using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject boxPrefab;
    public int rows = 5;
    public int columns = 10;
    public float spacing = 1.1f;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
{
    Color[] colors = { Color.red, Color.blue, Color.green, Color.magenta, Color.black };

    for (int x = 0; x < columns; x++)
    {
        for (int y = 0; y < rows; y++)
        {
            Vector3 position = new Vector3(x * spacing, y * spacing, 0);
            GameObject box = Instantiate(boxPrefab, position, Quaternion.identity, transform);
            Box boxScript = box.GetComponent<Box>();
            boxScript.boxColor = colors[Random.Range(0, colors.Length)];
        }
    }
}

}
