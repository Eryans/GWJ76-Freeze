using System;
using Godot;

[GlobalClass]
public partial class FuelManager : Node
{
    [Export]
    public float MaxFuelAmount { get; private set; } = 100;
    public float FuelAmount { get; private set; }
    public static event Action NoMoreFuel;

    public override void _Ready()
    {
        FuelAmount = MaxFuelAmount;
    }
    public override void _Process(double delta)
    {
        FuelAmount -= FuelAmount > 0 ? (float)delta : 0;
        if (FuelAmount <= 0)
        {
            NoMoreFuel?.Invoke();
        }
    }
}