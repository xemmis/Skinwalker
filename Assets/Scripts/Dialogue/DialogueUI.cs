using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _npcTextUI;
    [SerializeField] private Transform _answersContainer;
    [SerializeField] private GameObject _answerButtonPrefab;
    [SerializeField] private DialogueSystem _dialogueSystem; // Ручное перетаскивание для надёжности

    private void Awake()
    {
        if (_dialogueSystem == null)
            _dialogueSystem = DialogueSystem.Instance; // Фолбэк

        _dialogueSystem.OnDialogueUpdated.AddListener(UpdateUI);
    }

    private void UpdateUI(DialogueNode node)
    {
        _npcTextUI.text = node.NPC_Text;
        ClearAnswers();

        foreach (DialogueAnswer answer in node.Answers)
        {
            GameObject buttonObj = Instantiate(_answerButtonPrefab, _answersContainer);
            buttonObj.GetComponent<TextMeshProUGUI>().text = answer.Player_Text;

            buttonObj.GetComponent<Button>().onClick.AddListener(() => {
                _dialogueSystem.SelectAnswer(System.Array.IndexOf(node.Answers, answer));
            });
        }
    }

    private void ClearAnswers()
    {
        foreach (Transform child in _answersContainer)
            Destroy(child.gameObject);
    }
}

