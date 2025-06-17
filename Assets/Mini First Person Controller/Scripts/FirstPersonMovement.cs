using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("MoveAttributes")]
    public bool canRun = true;
    public bool canWalk = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();
    [SerializeField] private DialogueSystem _dialogueSystem;


    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _dialogueSystem.OnDialogue.AddListener(DialogueMoveLogic);
    }

    private void OnDestroy()
    {
        _dialogueSystem.OnDialogue.RemoveListener(DialogueMoveLogic);
    }

    public void DialogueMoveLogic(bool condition)
    {
        canWalk = !condition;
    }

    void Update()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.
        if(canWalk) rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
    }
}