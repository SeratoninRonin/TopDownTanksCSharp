using Godot;
using System;

public class Pickup : Area2D
{
    public enum Items 
    {
        Health=0,
        Ammo=1
    }

    Texture[] iconTextures;
    Sprite icon;

    [Export]
    Items Type = Items.Health;
    [Export]
    Vector2 Amount = new Vector2(10,25);

    public override void _Ready()
    {
        Connect("body_entered",this,"OnPickupEntered");
        iconTextures = new Texture[2];
        iconTextures[0]=GD.Load<Texture>("res://Assets/Effects/wrench_white.png");
        iconTextures[1] = GD.Load<Texture>("res://Assets/Effects/ammo_machinegun.png");
        icon = GetNode<Sprite>("Icon");
        icon.Texture=iconTextures[(int)Type];
    }

    public void OnPickupEntered(Node body)
    {
        switch(Type)
        {
            case Items.Health:
                if(body is IDamagable)
                {
                    var b = body as IDamagable;
                    b.Heal((float)GD.RandRange(Amount.x,Amount.y));
                }
                break;
            case Items.Ammo:
                var t = body as Tank;
                t.Ammo += (int)(GD.RandRange(Amount.x,Amount.y));
                break;
        }
        QueueFree();
    }
}
