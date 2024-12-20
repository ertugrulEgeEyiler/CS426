using UnityEngine;

public class Box : MonoBehaviour
{
    public Color boxColor;
    private Renderer boxRenderer; // Çakışmayı önlemek için yeni bir isim

    void Start()
    {
        boxRenderer = GetComponent<Renderer>(); // Renderer bileşenine erişim
        boxRenderer.material.color = boxColor; // Materyalin rengini ayarlama
    }
}
