using TMPro;
using UnityEngine;


public class ddd : MonoBehaviour
{
    public Canvas canvas;
    public TextMeshProUGUI textMeshPro;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        if (textMeshPro != null)
        {
            // Transform의 position(Vector3)을 문자열로 변환해서 TextMeshPro에 표시
            textMeshPro.text = $"Position: {transform.position}";
        }
    }
}
