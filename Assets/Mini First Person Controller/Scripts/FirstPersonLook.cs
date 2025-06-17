using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    [SerializeField] private DialogueSystem _dialogueSystem;
    public float sensitivity = 2;
    public float smoothing = 1.5f;
    Vector2 velocity;
    Vector2 frameVelocity;

    public bool cameraControlEnabled = true;

    void Reset()
    {
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        _dialogueSystem.OnDialogue.AddListener(DialogueMoveLogic);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        _dialogueSystem.OnDialogue.RemoveListener(DialogueMoveLogic);
    }

    public void DialogueMoveLogic(bool condition)
    {
        cameraControlEnabled = !condition;
        Cursor.lockState = condition ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void Update()
    {
        if (!cameraControlEnabled) return;

        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }
}