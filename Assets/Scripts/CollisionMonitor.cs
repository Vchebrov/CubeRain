using System;
using UnityEngine;

[RequireComponent(typeof(ColorChanger))]
public class CollisionMonitor : MonoBehaviour
{
    private string _layerName = "TouchNGo";

    private int _targetLayer;
    
    private ColorChanger _colorChanger;

    private bool _isTouched = false;
    
    public event Action<Transform> OnTouched;

    private void Awake()
    {
        _colorChanger = new  ColorChanger();
        _targetLayer = LayerMask.NameToLayer(_layerName);
    }

    public void ResetState()
    {
        _isTouched = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"OnCollisionEnter. {collision.gameObject.name} met");
        if (collision.gameObject.layer == _targetLayer && !_isTouched)
        {
            Debug.Log("Check passed for collision");
            _colorChanger.ChangeColor(this.gameObject);
            OnTouched?.Invoke(gameObject.transform);
            _isTouched = true;
        }
    }
}