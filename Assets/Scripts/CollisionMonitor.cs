using UnityEngine;

public class CollisionMonitor : MonoBehaviour
{
    private ColorChanger _colorChanger;
    private Spawner _spawner;

    private bool _isTouched = false;

    private void Awake()
    {
        _colorChanger = FindObjectOfType<ColorChanger>();
        _spawner = FindObjectOfType<Spawner>();
    }

    public void ResetState()
    {
        _isTouched = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"OnCollisionEnter. {collision.gameObject.name} met");
        if (collision.gameObject.layer == LayerMask.NameToLayer("TouchNGo") && !_isTouched)
        {
            Debug.Log("Check passed for collision");
            _colorChanger.ChangeColor(this.gameObject);
            StartCoroutine(_spawner.RemoveCube(this.gameObject));
            _isTouched = true;
        }
    }
}