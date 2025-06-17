using UnityEngine;

public class UnpackingLogic : Interactable
{
    [SerializeField] private GameObject _packageItemPrefab;
    [SerializeField] private int _packageItemStartCount = 5;
    private int _packageInstanceCount;

    private void Start()
    {
        CanUse = true;
        Type = IngredientType.Package;
        _packageInstanceCount = _packageItemStartCount;
    }

    public override void Interact(Transform position)
    {
        if (_packageInstanceCount == 0) return;
        _packageInstanceCount--;
        GameObject newItem = Instantiate(_packageItemPrefab, transform.position, Quaternion.identity);
        newItem.TryGetComponent<Rigidbody>(out Rigidbody newRb);
        newRb.AddForce(position.forward * _pushForce,ForceMode.Impulse);
    }
}
