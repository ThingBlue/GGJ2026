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

        var startNode = editorGraph.GetNodes().OfType<DialogueHeadNode>().FirstOrDefault();

        foreach (var iNode in editorGraph.GetNodes())
        {
            if (iNode is DialogueHeadNode || iNode is DialogueExitNode) continue;

            var runTimeNode = new RuntimeDialogueNode { ID = nodeIDMap[iNode] };
            if(iNode is DialogueNode dialogueNode)
            {
                ProcessDialogueNode(dialogueNode, runTimeNode, nodeIDMap);
            }
            else if(iNode is SuspicionComparisonNode suspicionNode)
            {
                
            }
        }

        ctx.AddObjectToAsset("RuntimeData", runtimeGraph);
        ctx.SetMainObject(runtimeGraph);
    }

    private void ProcessDialogueNode(DialogueNode node, RuntimeDialogueNode runtimeNode, Dictionary<INode, string> nodeIDMap)
    {
        runtimeNode.dialogueText = getPortValue<string>(node.GetInputPortByName("Dialogue"));

        var nextNodePort = node.GetOutputPortByName("output").firstConnectedPort;
        if(nextNodePort != null)
        {
            runtimeNode.nextNodeID = nodeIDMap[nextNodePort.GetNode()];
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
}
