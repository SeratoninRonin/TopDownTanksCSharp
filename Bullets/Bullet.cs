using Godot;
using System;
public interface IDamagable
{
    void TakeDamage(float damage);
    void Heal(float amount);
}
public class Bullet : Area2D
{
    protected Timer lifetimeTimer;
    protected CollisionShape2D shape2D;
    protected Sprite sprite;
    protected AnimatedSprite explosion;

   [Export]
   public int Speed;
   [Export]
   public int Damage;
   [Export]
   public float Lifetime;
   [Export]
   public float SteerForce;

   Vector2 velocity = Vector2.Zero;
   Vector2 acceleration = Vector2.Zero;
   Node2D target = null;

   public override void _Ready()
   {
       lifetimeTimer=(Timer)GetNode("Lifetime");
       Connect("body_entered",this,"OnBodyEntered");
       lifetimeTimer.Connect("timeout",this,"OnLifetimeTimeout");
       sprite = GetNode<Sprite>("Sprite");
       explosion = GetNode<AnimatedSprite>("Explosion");
       explosion.Connect("animation_finished",this,"OnExplosionFinished");
   }

    public void OnBodyEntered(Node body)
    {
        Explode();
        if(body is IDamagable)
        {
            var b = body as IDamagable;
            b.TakeDamage(Damage);
        }
    }

    public void OnLifetimeTimeout()
    {
        Explode();
    }

    public void Explode()
    {
        velocity=Vector2.Zero;
        sprite.Hide();
        explosion.Show();
        explosion.Play("Smoke");
    }
   public void Start(Vector2 position, Vector2 direction, Node2D target = null)
   {
       Position=position;
       Rotation = direction.Angle();
       lifetimeTimer.WaitTime = Lifetime;
       lifetimeTimer.Start();
       velocity = direction * Speed;
       this.target=target;
   }

   public override void _Process(float delta)
   {
       if(target!=null)
       {
           acceleration += Seek();
           velocity += acceleration * delta;
           velocity = velocity.Clamped(Speed);
           Rotation = velocity.Angle();
       }
       Position += velocity * delta;
   }

    public Vector2 Seek()
    {
        var desired=(target.Position - Position).Normalized() * Speed;
        var steer = (desired - velocity).Normalized() * SteerForce;
        return steer;
    }    
   public void OnExplosionFinished()
   {
       QueueFree();
   }
}
