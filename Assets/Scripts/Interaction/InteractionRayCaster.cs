using UnityEngine;
using UnityEngine.UI;

public class InteractionRayCaster : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float _interactionDistance = 3f;
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private KeyCode _interactionKey = KeyCode.Mouse0;
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
    private Transform _interactionPivot;

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        _image = GetComponentInChildren<Image>();
        CreateInteractionPivot();
        InitializeLaserSight();
    }

    private void Start()
    {
        if (_laserSight == null)
        {
            _laserSight = gameObject.AddComponent<LineRenderer>();
            _laserSight.material = new Material(Shader.Find("Sprites/Default")); // ”становка простого материала
            _laserSight.useWorldSpace = true;
        }

        InitializeLaserSight();
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
        UpdateInteractionPivot(); // <-- ƒобавлено
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_heldObject == null)
                TryInteract();
            else
                DropObject();
        }

        if (_heldObject != null)
            UpdateHeldObject();
    }

    private void UpdateInteractionPivot()
    {
        // ќбновл€ем позицию Pivot относительно камеры (на случай, если камера поворачиваетс€)
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
                _heldObject.Interact(_interactionPivot);
            }
        }

    }

    private void UpdateHeldObject()
    {
        // ѕлавное перемещение к Pivot (позици€ и поворот)
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
    private void InitializeLaserSight()
    {
        if (_laserSight != null)
        {
            _laserSight.positionCount = 2;
            _laserSight.startWidth = _defaultLaserWidth;
            _laserSight.endWidth = _defaultLaserWidth;
        }
    }

}

