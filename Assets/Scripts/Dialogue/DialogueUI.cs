using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class DialogueUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _npcTextUI;
    [SerializeField] private Transform _answersContainer;
    [SerializeField] private GameObject _answerButtonPrefab;
    [SerializeField] private CanvasGroup _dialogueCanvasGroup;

    [Header("Settings")]
    [SerializeField] private float _fadeDuration = 0.3f;

    private DialogueSystem _dialogueSystem;
    private List<GameObject> _currentAnswerButtons = new List<GameObject>();
    private Tweener _fadeTweener;
    private bool _isDialogueActive = false;

    private void Start()
    {
        _dialogueSystem = DialogueSystem.Instance;

        if (_dialogueSystem == null)
        {
            Debug.LogError("DialogueSystem instance not found!");
            enabled = false;
            return;
        }

        _dialogueSystem.OnDialogueUpdated.AddListener(UpdateUI);
        _dialogueSystem.OnDialogueStateChanged.AddListener(SetDialogueActive);

        InitializeUI();
    }

    private void InitializeUI()
    {
        if (_dialogueCanvasGroup != null)
        {
            _dialogueCanvasGroup.alpha = 0f;
            _dialogueCanvasGroup.interactable = false;
            _dialogueCanvasGroup.blocksRaycasts = false;
        }

        ClearUI();
    }

    private void SetDialogueActive(bool isActive)
    {
        if (_dialogueCanvasGroup == null || _isDialogueActive == isActive)
            return;

        _isDialogueActive = isActive;

        if (isActive)
        {
            ShowDialogue();
        }
        else
        {
            HideDialogue();
        }
    }   


    private void OnDestroy()
    {
        if (_dialogueSystem != null)
        {
            _dialogueSystem.OnDialogueUpdated.RemoveListener(UpdateUI);
            _dialogueSystem.OnDialogueStateChanged.RemoveListener(SetDialogueActive);
        }

        _fadeTweener?.Kill();
    }

    private void UpdateUI()
    {
        if (_dialogueSystem == null || _dialogueSystem.CurrentNode == null)
        {
            Debug.LogWarning("Cannot update UI - no current node");
            return;
        }

        var currentNode = _dialogueSystem.CurrentNode;
        _npcTextUI.text = currentNode.NPC_Text;
        CreateAnswerButtons(currentNode);
    }

    private void CreateAnswerButtons(DialogueNode node)
    {
        ClearAnswers();

        for (int i = 0; i < node.Answers.Length; i++)
        {
            var answer = node.Answers[i];
            if (!IsAnswerAvailable(answer)) continue;

            CreateAnswerButton(answer, i);
        }
    }

    private void CreateAnswerButton(DialogueAnswer answer, int index)
    {
        GameObject buttonObj = Instantiate(_answerButtonPrefab, _answersContainer);
        Button button = buttonObj.GetComponent<Button>();
        TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

        buttonText.text = answer.Text;
        int answerIndex = index;

        button.onClick.AddListener(() => OnAnswerSelected(answerIndex));

        _currentAnswerButtons.Add(buttonObj);

        // Анимация появления
        buttonObj.transform.localScale = Vector3.zero;
        buttonObj.transform.DOScale(Vector3.one, 0.3f).SetDelay(index * 0.1f);
    }

    private void OnAnswerSelected(int answerIndex)
    {
        _dialogueSystem.SelectAnswer(answerIndex);
    }

    private bool IsAnswerAvailable(DialogueAnswer answer)
    {
        return answer != null && !string.IsNullOrEmpty(answer.Text);
    }

    private void ShowDialogue()
    {
        _fadeTweener?.Kill();
        _fadeTweener = _dialogueCanvasGroup.DOFade(1f, _fadeDuration)
            .SetEase(Ease.OutQuad)
            .OnStart(() => {
                _dialogueCanvasGroup.blocksRaycasts = true;
            })
            .OnComplete(() => {
                _dialogueCanvasGroup.interactable = true;
            });
    }

    private void HideDialogue()
    {
        _dialogueCanvasGroup.interactable = false;

        _fadeTweener?.Kill();
        _fadeTweener = _dialogueCanvasGroup.DOFade(0f, _fadeDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() => {
                _dialogueCanvasGroup.blocksRaycasts = false;
                ClearUI();
            });
    }

    private void ClearUI()
    {
        _npcTextUI.text = "";
        ClearAnswers();
    }

    private void ClearAnswers()
    {
        foreach (var button in _currentAnswerButtons)
        {
            if (button != null)
            {
                Destroy(button);
            }
        }
        _currentAnswerButtons.Clear();
    }
}