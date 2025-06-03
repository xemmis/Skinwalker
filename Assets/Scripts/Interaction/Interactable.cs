using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(BoxCollider))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private float _pushForce = 5;

    private Rigidbody _rb;
    private Collider _collider;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void Interact(Transform holdPoint)
    {
        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.interpolation = RigidbodyInterpolation.None;
        }
        _collider.enabled = false;
    }

    public void Drop()
    {
        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.AddForce(transform.forward * _pushForce,ForceMode.Impulse);
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        _collider.enabled = true;
    }


}

public class Bun : Interactable
{

}