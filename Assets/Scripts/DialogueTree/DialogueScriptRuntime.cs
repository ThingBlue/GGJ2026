using UnityEngine;
using System.Collections.Generic;

public class DialogueScriptRuntime : ScriptableObject
{
    public string FirstNodeID;
    public List<RuntimeDialogueNode> nodeList = new List<RuntimeDialogueNode>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class RuntimeDialogueNode
{
    public string ID;
    public string dialogueText;

    public string nextNodeID;
}