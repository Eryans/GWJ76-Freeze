using System;
using Godot;

[GlobalClass]
public partial class Health : Node
{
    [Export]
    public float MaxHealth { get; private set; } = 100;

    public float CurrentHealth { get; private set; }
    public event Action HealthDepleted;
    public override void _Ready()
    {
        CurrentHealth = MaxHealth;
    }

    public void ChangeCurrentHealth(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            HealthDepleted?.Invoke();
        }
    }
}