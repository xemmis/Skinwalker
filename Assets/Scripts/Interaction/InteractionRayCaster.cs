using UnityEngine;
using UnityEngine.UI;

public class InteractionRayCaster : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float _interactionDistance = 3f;
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private KeyCode _pickUpKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _interactionKey = KeyCode.E;
    [SerializeField] private Vector3 _holdOffset = new Vector3(0.5f, -0.3f, 1f); // Offset в локальных координатах камеры

    [Header("Visual Feedback")]
    [SerializeField] private LineRenderer _laserSight;
    [SerializeField] private Color _activeColor = Color.green;
    [SerializeField] private Color _inactiveColor = Color.red;
    [SerializeField] private float _defaultLaserWidth = 0.01f;
    [SerializeField] private float _pressedLaserWidth = 0.05f;
    [SerializeField] private bool _canInteract;
    [SerializeField] private Image _image;
    [SerializeField] private float _rayRadius = 0.05f;


    private Camera _mainCamera;
    private Interactable _heldObject;
    private Interactable _objectInRange;
    private Transform _interactionPivot;

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        _image = GetComponentInChildren<Image>();
        CreateInteractionPivot();
    }

    private void CreateInteractionPivot()
    {
        _interactionPivot = new GameObject("InteractionPivot").transform;
        _interactionPivot.SetParent(_mainCamera.transform);
        _interactionPivot.localPosition = _holdOffset;
        _interactionPivot.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        UpdateInteractionPivot(); // <-- Добавлено
        if (Input.GetKeyDown(_pickUpKey))
        {
            if (_heldObject == null)
                TryInteract();
            else
                DropObject();
        }

        if (Input.GetKeyDown(_interactionKey) && _heldObject != null)
        {
            _heldObject.Interact();
        }

        if (_heldObject != null)
            UpdateHeldObject();
    }


    private void UpdateInteractionPivot()
    {
        // Обновляем позицию Pivot относительно камеры (на случай, если камера поворачивается)
        _interactionPivot.localPosition = _holdOffset;
        _interactionPivot.localRotation = Quaternion.identity;
    }

    private void TryInteract()
    {
        Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.SphereCast(ray, _rayRadius, out RaycastHit hit, _interactionDistance))
        {
            if (hit.collider.TryGetComponent<Interactable>(out var interactable))
            {
                _heldObject = interactable;
                _heldObject.PickUp(_interactionPivot);
            }
        }

    }

    private void UpdateHeldObject()
    {
        // Плавное перемещение к Pivot (позиция и поворот)
        _heldObject.transform.position = Vector3.Lerp(
            _heldObject.transform.position,
            _interactionPivot.position,
            10f * Time.deltaTime
        );

        _heldObject.transform.rotation = Quaternion.Slerp(
            _heldObject.transform.rotation,
            _interactionPivot.rotation,
            10f * Time.deltaTime
        );
    }

    private void DropObject()
    {
        if (_heldObject != null)
        {
            _heldObject.Drop();
            _heldObject = null;
        }
    }

    private void UpdateLaserSight()
    {
        if (_laserSight == null) return;

        Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        _canInteract = Physics.Raycast(ray, _interactionDistance, _interactionLayer) && _heldObject == null;


    }
}

