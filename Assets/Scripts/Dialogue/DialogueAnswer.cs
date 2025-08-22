using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class DialogueAnswer
{
    [TextArea(2, 3)]
    public string Text;

    public DialogueNode NextNode;
    public UnityEvent OnSelect;
}