using UnityEngine;
using UnityEditor.AssetImporters;
using Unity.GraphToolkit.Editor;
using System;
using System.Collections.Generic;
using System.Linq;


[ScriptedImporter(1, DialogueGraph.AssetExtension)]
public class DialogueImporterScript : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        DialogueGraph editorGraph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath);
        DialogueScriptRuntime runtimeGraph = ScriptableObject.CreateInstance<DialogueScriptRuntime>();
        var nodeIDMap = new Dictionary<INode, string>();

        foreach (var node in editorGraph.GetNodes())
        {
            nodeIDMap[node] = Guid.NewGuid().ToString();
        }

        // TODO: Implement multiple head nodes
        var startNode = editorGraph.GetNodes().OfType<DialogueHeadNode>().FirstOrDefault();

        foreach (var iNode in editorGraph.GetNodes())
        {
            if (iNode is DialogueHeadNode || iNode is DialogueExitNode) continue;

            var runTimeNode = new RuntimeDialogueNode { ID = nodeIDMap[iNode] };
            if(iNode is DialogueNode dialogueNode)
            {
                ProcessDialogueNode(dialogueNode, runTimeNode, nodeIDMap);
            }
            else if(iNode is DialogueChoiceNode dChoiceNode)
            {
                ProcessChoiceNode(dChoiceNode, runTimeNode, nodeIDMap);
            }
            

            // TODO: Implement process functions for each type of node
        }

        ctx.AddObjectToAsset("RuntimeData", runtimeGraph);
        ctx.SetMainObject(runtimeGraph);
    }


    #region Helper Functions
    private void ProcessDialogueNode(DialogueNode node, RuntimeDialogueNode runtimeNode, Dictionary<INode, string> nodeIDMap)
    {
        runtimeNode.dialogueText = getPortValue<string>(node.GetInputPortByName("NPC Dialogue"));

        var nextNodePort = node.GetOutputPortByName("output").firstConnectedPort;
        if(nextNodePort != null)
        {
            runtimeNode.nextNodeID = nodeIDMap[nextNodePort.GetNode()];
        }
    }

    private void ProcessChoiceNode(DialogueChoiceNode node, RuntimeDialogueNode runtimeNode, Dictionary<INode, string> nodeIDMap)
    {
        runtimeNode.dialogueText = getPortValue<string>(node.GetInputPortByName("NPC Dialogue"));

        // Load choices
        node.GetNodeOptionByName("Number of Outputs").TryGetValue<int>(out int numChoices);
        for(int i = 0; i < numChoices; i++)
        {
            node.GetInputPortByName("Option "+i.ToString()).TryGetValue(out string choiceText);
            var choiceConnectedPort = node.GetOutputPortByName("Choice" + i.ToString()).firstConnectedPort;

            runtimeNode.choices.Add(new ChoiceData { choiceString = choiceText, destNodeID = nodeIDMap[choiceConnectedPort.GetNode()] });
        }
    }

    private T getPortValue<T>(IPort port)
    {
        if (port == null) return default;

        if(port.isConnected)
        {
            if(port.firstConnectedPort.GetNode() is IVariableNode variableNode)
            {
                variableNode.variable.TryGetDefaultValue(out T value);
                return value;
            }
        }

        port.TryGetValue(out T fallbackValue);
        return fallbackValue;
    }
    #endregion
}