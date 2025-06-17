using UnityEngine;
using UnityEngine.Events;

public class DialogueSystem : MonoBehaviour
{
    public DialogueNode CurrentNode;
    public UnityEvent<DialogueNode> OnDialogueUpdated; // Событие обновления UI
    public static DialogueSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else

            Destroy(gameObject);
    }
    // Начать диалог
    public void StartDialogue(DialogueNode startNode)
    {
        CurrentNode = startNode;
        UpdateDialogue();
    }

    // Выбрать ответ
    public void SelectAnswer(int answerIndex)
    {
        if (CurrentNode == null || answerIndex < 0 || answerIndex >= CurrentNode.Answers.Length)
        {
            EndDialogue();
            return;
        }

        DialogueAnswer selectedAnswer = CurrentNode.Answers[answerIndex];
        CurrentNode = selectedAnswer.NextNode;

        if (CurrentNode != null)
            UpdateDialogue();
        else
            EndDialogue();
    }

    private void UpdateDialogue()
    {
        OnDialogueUpdated.Invoke(CurrentNode); // Уведомляем UI
    }

    private void EndDialogue()
    {
        CurrentNode = null;
        Debug.Log("Диалог завершён");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<InteractionRayCaster>(out InteractionRayCaster component))
        {
            StartDialogue(CurrentNode);
        }
    }
}
