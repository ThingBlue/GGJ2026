using System;
using UnityEngine;
using UnityEditor;
using Unity.GraphToolkit.Editor;

[Graph(AssetExtension)]
[Serializable]
public class DialogueGraph : Graph
{
    public const string AssetExtension = "simpleg";

    [MenuItem("Assets/Create/Dialogue/DialogueGraph", false)]
    static void CreateAssetFile()
    {
        GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
    }
}

// Start the dialogue. How do we choose what face??
[Serializable]
public class DialogueHeadNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddOutputPort("Output").Build();
        context.AddInputPort<string>("Face").Build();
    }
}

// Exit the dialogue
[Serializable]
public class DialogueExitNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("Input").Build();
        context.AddInputPort<bool>("Killed NPC?").Build();
    }
}

[Serializable]
public class DialogueNode : Node
{
    const string numOutputs = "Number of Outputs";

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption<int>(numOutputs);
    }

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("Input").Build();
        context.AddInputPort<string>("NPC Dialogue").Build();

        var numPortsName = GetNodeOptionByName(numOutputs);
        numPortsName.TryGetValue<int>(out int numPorts);

        for(int i = 0; i < numPorts; i++)
        {
            context.AddInputPort<string>($"Option {i+1}").Build();
            context.AddOutputPort($"Choice {i+1}").Build();
        }
    }
}

// Compare the suspicion of the character to a value
[Serializable]
public class SuspicionComparisonNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("Input").Build();
        context.AddInputPort<float>("Compared Value").Build();
        context.AddInputPort<string>("Comparison Operator").Build();

        context.AddOutputPort("Output if True").Build();
        context.AddOutputPort("Output if False").Build();
    }
}

[Serializable]
public class ExpressionComparisonNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("Input").Build();
        context.AddInputPort<float>("Compared Value").Build();
        context.AddInputPort<string>("Comparison Operator").Build();

        context.AddOutputPort("Output if True").Build();
        context.AddOutputPort("Output if False").Build();
    }
}