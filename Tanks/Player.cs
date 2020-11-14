using Godot;
using System;

public class Player : Tank
{
    public override void _Ready()
    {
        base._Ready();
    }

    public override void Control(float delta)
    {
        turret.LookAt(GetGlobalMousePosition());
        var rot_dir = 0;
        if (Input.IsActionPressed("turn_left"))
            rot_dir -= 1;
        if (Input.IsActionPressed("turn_right"))
            rot_dir += 1;
        Rotation += RotationSpeed * rot_dir * delta;
        velocity = Vector2.Zero;
        if (Input.IsActionPressed("forward"))
            velocity = new Vector2(MaxSpeed, 0).Rotated(Rotation);
        if (Input.IsActionPressed("back"))
            velocity = new Vector2(-MaxSpeed / 2, 0).Rotated(Rotation);
        if(Input.IsActionJustPressed("click"))
            Fire(GunShots,GunSpread);

        base.Control(delta);
    }
}
