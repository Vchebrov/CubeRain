using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private float _hueMin = 0f;
    private float _hueMax = 1f;
    private float _saturationMin = 0.6f;
    private float _saturationMax = 1f;
    private float _valueMin = 0.6f;
    private float _valueMax = 1f;
    
    public void ChangeColor(GameObject cube)
    {
        if (cube.TryGetComponent(out Renderer renderer))
        {
            renderer.material.color = Random.ColorHSV(
                _hueMin, _hueMax,
                _saturationMin, _saturationMax,
                _valueMin, _valueMax
            );                
        }
        else
        {
            Debug.LogWarning($"{cube.name}: Renderer not found.");
        }
    }
}
