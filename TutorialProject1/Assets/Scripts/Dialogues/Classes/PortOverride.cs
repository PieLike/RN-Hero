using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PortOverride : Port
{
    public PortOverride(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base (portOrientation, portDirection, portCapacity, type)
    {
 
    }

    public override void Connect(Edge edge)
    {
        base.Connect(edge); 

        Debug.Log("sdgfsdf");
        node.mainContainer.Add(edge.output);
        //DialogueNode.EditChoice(edge.input, edge.output);       
    }
}
