using UnityEngine;
using System.Collections.Generic;

public class DialogueScriptRuntime : ScriptableObject
{
    public string FirstNodeID;
    public List<RuntimeDialogueNode> nodeList = new List<RuntimeDialogueNode>();
}

public class RuntimeDialogueNode
{
    public string ID;
    public string dialogueText;
    public List<ChoiceData> choices = new List<ChoiceData>();
    public string nextNodeID;
}

public class ChoiceData
{
    public string choiceString;
    public string destNodeID;
}