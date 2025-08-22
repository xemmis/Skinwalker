using UnityEngine.Events;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private DialogueNode _testNode;
    public DialogueNode CurrentNode { get; private set; }
    public UnityEvent OnDialogueUpdated;
    public UnityEvent<bool> OnDialogueStateChanged;

    public static DialogueSystem Instance { get; private set; }

    private bool _isInDialogue = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void StartDialogue(DialogueNode startNode)
    {
        if (_isInDialogue || startNode == null)
        {
            Debug.LogWarning(_isInDialogue ? "Dialogue already in progress!" : "Start node is null!");
            return;
        }

        _isInDialogue = true;
        CurrentNode = startNode;
        OnDialogueStateChanged?.Invoke(true);
        UpdateDialogue();
    }

    public void SelectAnswer(int answerIndex)
    {
        if (CurrentNode == null || CurrentNode.Answers == null ||
            answerIndex < 0 || answerIndex >= CurrentNode.Answers.Length)
        {
            Debug.LogWarning("Invalid answer selection");
            EndDialogue();
            return;
        }

        var answer = CurrentNode.Answers[answerIndex];
        answer.OnSelect?.Invoke();

        if (answer.NextNode == null)
        {
            EndDialogue();
            return;
        }

        CurrentNode = answer.NextNode;

        if (CurrentNode.IsEnd)
            EndDialogue();
        else
            UpdateDialogue();
    }

    private void UpdateDialogue()
    {
        OnDialogueUpdated?.Invoke();
        PlayVoiceLine(CurrentNode.VoiceLine);
    }

    private void PlayVoiceLine(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }

    public void EndDialogue()
    {
        _isInDialogue = false;
        CurrentNode = null;
        OnDialogueStateChanged?.Invoke(false);
    }

    [ContextMenu("Test Dialogue")]
    private void TestDialogue()
    {
        if (_testNode != null)
            StartDialogue(_testNode);
        else
            Debug.LogWarning("No test node assigned!");
    }
}