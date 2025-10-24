using System.Collections;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{
    [SerializeField] private float _delay = 3f;
    
    private float _delayMax = 5f;
    private float _delayMin = 2f;
    
    private float _hueMin = 0f;
    private float _hueMax = 1f;
    private float _saturationMin = 0.6f;
    private float _saturationMax = 1f;
    private float _valueMin = 0.6f;
    private float _valueMax = 1f;
    
    private bool _isTouched;

    private void OnEnable()
    {
        _isTouched = false;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") &&  !_isTouched)
        {
            ChangeColor(gameObject);
            StartCoroutine(RemoveCube());
            _isTouched =  true;
        }
    }

    private IEnumerator RemoveCube()
    {
        yield return new WaitForSeconds(Random.Range(_delayMin, _delayMax));
        Spawner._pool.Release(gameObject);
    }
    
    private void ChangeColor(GameObject cube)
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