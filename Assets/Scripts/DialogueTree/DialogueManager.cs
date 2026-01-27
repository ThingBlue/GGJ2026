using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class DialogueManager : MonoBehaviour
{
    public DialogueScriptRuntime RuntimeScript;

    [Header("UI Components")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI option1;
    public TextMeshProUGUI option2;
    public TextMeshProUGUI option3;
    public TextMeshProUGUI option4;

    private Dictionary<string, RuntimeDialogueNode> _nodeLookup = new Dictionary<string, RuntimeDialogueNode>();
    private RuntimeDialogueNode _currentNode;

    private void Start()
    {
        foreach(var node in RuntimeScript.nodeList)
        {
            _nodeLookup[node.ID] = node;
        }

        showNode(RuntimeScript.FirstNodeID);
    }

    private void showNode(string nodeID)
    {
        if(!_nodeLookup.ContainsKey(nodeID))
        {
            endDialogue();
            return;
        }

        _currentNode = _nodeLookup[nodeID];

        dialoguePanel.SetActive(true);
        dialogueText.SetText(_currentNode.dialogueText);
    }

    private void endDialogue()
    {
        dialoguePanel.SetActive(false);
        _currentNode = null;
    }
}
