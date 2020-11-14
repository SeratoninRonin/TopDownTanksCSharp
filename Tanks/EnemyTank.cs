using Godot;
using System;

public class EnemyTank : Tank
{
    Node2D parent;
    Area2D detect;
    Node2D target = null;
    RayCast2D lookAhead1 = null;
    RayCast2D lookAhead2 = null;

    [Export]
    public float TurretSpeed;
    [Export]
    public int DetectRadius;

    float speed=0;

    public override void _Ready()
    {
        parent = GetParent() as Node2D;
        detect = (Area2D)GetNode("DetectRadius");
        var col = (CollisionShape2D)detect.GetNode("CollisionShape2D");
        var circle = new CircleShape2D();
        circle.Radius=DetectRadius;
        col.Shape=circle;
        lookAhead1 = GetNode("LookAhead1") as RayCast2D;
        lookAhead2 = GetNode("LookAhead2") as RayCast2D;
        detect.Connect("body_entered",this,"OnDetectEntered");
        detect.Connect("body_exited",this,"OnDetectExited");

        base._Ready();
    }

    public override void _Process(float delta)
    {
        if(target!=null)
        {
            var target_dir = (target.GlobalPosition - GlobalPosition).Normalized();
            var current_dir = new Vector2(1, 0).Rotated(turret.GlobalRotation);
            turret.GlobalRotation = current_dir.LinearInterpolate(target_dir, TurretSpeed * delta).Angle();
            if(target_dir.Dot(current_dir)>0.9f)
            {
                Fire(GunShots, GunSpread, target);
            }
        }
        base._Process(delta);
    }

    public void OnDetectEntered(Node body)
    {
            target = body as Node2D;
    }

    public void OnDetectExited(Node body)
    {
        if (body == target)
            target = null;
    }

    public override void Control(float delta)
    {
        if(parent is PathFollow2D)
        {
            if(lookAhead1.IsColliding() || lookAhead2.IsColliding())
                speed = Mathf.Lerp(speed,0,0.1f);
            else
                speed = Mathf.Lerp(speed,MaxSpeed,.05f);

        
            var p = parent as PathFollow2D;
            p.Offset = (p.Offset + speed * delta);
            Position = Vector2.Zero;
        }

        base.Control(delta);
    }

    
}
