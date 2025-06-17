using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Interactable : MonoBehaviour
{
    [Space(15)]
    [Header("Может ли объект использоваться на 'Е'")]
    public bool CanUse;
    [Space(15)]
    [SerializeField] private protected float _pushForce = 5;
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

    public virtual void Interact(Transform position)
    {

    }

    public virtual void Drop(Transform position)
    {
        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.AddForce(position.forward * _pushForce, ForceMode.Impulse);
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Item"), LayerMask.NameToLayer("Player"), false);

        }
        _collider.enabled = true;
    }


}
