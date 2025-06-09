using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private float _pushForce = 5;
    public IngredientType Type;
    private Rigidbody _rb;
    private Collider _collider;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public virtual void PickUp(Transform holdPoint)
    {
        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.interpolation = RigidbodyInterpolation.None;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Item"), LayerMask.NameToLayer("Player"), true);

        }
        _collider.enabled = false;
    }

    public virtual void Interact()
    {

    }

    public virtual void Drop()
    {
        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.AddForce(transform.forward * _pushForce, ForceMode.Impulse);
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Item"), LayerMask.NameToLayer("Player"), false);

        }
        _collider.enabled = true;
    }


}
