using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class VisitorDialogLogic : MonoBehaviour
{
    public DialogueNode StartNode;
    private BoxCollider _boxCollider;
    [SerializeField] private DialogueSystem _dialogueSystem;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float stopThreshold = 0.1f;

    private Transform playerCamera;
    private FirstPersonMovement playerMovement;
    private FirstPersonLook playerLook;
    private Rigidbody playerRigidbody;
    private bool isRotating;
    private Quaternion targetCameraRotation;

    private void Awake()
    {
        if (_dialogueSystem == null)
            _dialogueSystem = DialogueSystem.Instance;

        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        _boxCollider.size *= 3.5f;
    }

    private void Update()
    {
        if (!isRotating) return;

        // Плавный поворот камеры
        playerCamera.rotation = Quaternion.Slerp(
            playerCamera.rotation,
            targetCameraRotation,
            rotationSpeed * Time.deltaTime
        );

        // Проверка завершения поворота
        if (Quaternion.Angle(playerCamera.rotation, targetCameraRotation) < 1f)
        {
            playerCamera.rotation = targetCameraRotation;
            isRotating = false;
            _dialogueSystem.OnDialogueUpdated?.Invoke(StartNode);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<FirstPersonMovement>(out playerMovement))
            return;

        // Получаем все необходимые компоненты
        playerCamera = Camera.main.transform;
        playerLook = playerMovement.GetComponentInChildren<FirstPersonLook>();
        playerRigidbody = playerMovement.GetComponent<Rigidbody>();

        // Немедленно останавливаем игрока
        playerMovement.canWalk = false;
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        // Отключаем управление камерой
        if (playerLook != null)
        {
            playerLook.cameraControlEnabled = false;
        }

        // Вычисляем направление к NPC (игнорируем разницу по высоте)
        Vector3 directionToNPC = transform.position - playerCamera.position;
        directionToNPC.y = 0;
        targetCameraRotation = Quaternion.LookRotation(directionToNPC);

        // Начинаем поворот только если игрок достаточно остановился
        if (playerRigidbody == null || playerRigidbody.velocity.magnitude < stopThreshold)
        {
            StartRotation();
            _dialogueSystem.StartDialogue(StartNode);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            InvokeRepeating(nameof(CheckStopCondition), 0.1f, 0.1f);
        }
    }

    private void CheckStopCondition()
    {
        if (playerRigidbody.velocity.magnitude < stopThreshold)
        {
            CancelInvoke(nameof(CheckStopCondition));
            StartRotation();
        }
    }

    private void StartRotation()
    {
        isRotating = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<FirstPersonMovement>(out _))
            return;

        CancelInvoke(nameof(CheckStopCondition));
        isRotating = false;

        if (playerLook != null)
        {
            playerLook.cameraControlEnabled = true;
        }
        if (playerMovement != null)
        {
            playerMovement.canWalk = true;
        }
    }
}