using System;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public string NPC_Text; // Текст NPC
    public DialogueAnswer[] Answers; // Варианты ответов
    public AudioClip VoiceLine; // Опционально: звуковая дорожка
    public bool IsEnd;
}
