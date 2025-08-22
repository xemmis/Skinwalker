using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Node", fileName = "DN_")]
public class DialogueNode : ScriptableObject
{
    [TextArea(3, 5)]
    public string NPC_Text;

    public DialogueAnswer[] Answers;
    public AudioClip VoiceLine;
    public bool IsEnd;
}