using System;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public string NPC_Text; // ����� NPC
    public DialogueAnswer[] Answers; // �������� �������
    public AudioClip VoiceLine; // �����������: �������� �������
    public bool IsEnd;
}
