using System;
using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class DialogTriggerrer : Area3D
{
    [Export]
    public Godot.Collections.Array<string> Texts { get; private set; }
    private readonly List<string> textsList = new();
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        foreach (string text in Texts)
        {
            textsList.Add(text);
        }
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is Player)
        {
            DialogManager.InvokeOpenDialog();
            DialogManager.InvokeSetDialogText(textsList);
        }
    }
}