using System;

[Serializable]
public class DialogueAnswer
{
    public string Player_Text; // Текст ответа игрока
    public DialogueNode NextNode; // Следующий узел диалога
    public bool IsImportantChoice; // Влияет ли выбор на сюжет?
}