using UnityEngine;

public class MergeLogic : MonoBehaviour
{
    [SerializeField] private GameObject _nextStage;
    [SerializeField] private IngredientType _requiredIngredient;
    private bool _interacted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Interactable>(out var component) &&
            component.Type == _requiredIngredient && !_interacted)
        {
            _interacted = true;
            Instantiate(_nextStage, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
