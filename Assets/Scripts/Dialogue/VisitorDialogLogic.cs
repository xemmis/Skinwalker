using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class VisitorDialogLogic : MonoBehaviour
{
    public DialogueNode StartNode;
    private BoxCollider _boxCollider;
    [SerializeField] private DialogueSystem _dialogueSystem;

    private void Awake()
    {
        if (_dialogueSystem == null)
            _dialogueSystem = DialogueSystem.Instance; // Фолбэк

        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        _boxCollider.size = new Vector3(_boxCollider.size.x * 3.5f, _boxCollider.size.y * 3.5f, _boxCollider.size.z * 3.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<FirstPersonMovement>(out FirstPersonMovement component))
        {
            _dialogueSystem.OnDialogueUpdated?.Invoke(StartNode);
        }
    }
}
