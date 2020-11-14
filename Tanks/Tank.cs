using Godot;
using System;

public class Tank : KinematicBody2D, IDamagable
{
    protected Sprite turret;
    protected Sprite body;
    protected Timer gunTimer;
    protected Vector2 velocity;
    protected Position2D muzzle;
    protected AnimationPlayer player;
    protected CollisionShape2D hitbox;
    protected AnimatedSprite explosion;
    protected bool canShoot = true;
    protected bool alive = true;
    protected float health;
    
    [Signal]
    public delegate void Shoot(PackedScene bullet, Vector2 position, Vector2 direction);
    [Signal]
    public delegate void HealthChanged(float value);
    [Signal]
    public delegate void AmmoChanged(float value);
    [Signal]
    public delegate void Dead();

    [Export]
    public PackedScene Bullet;
    [Export]
    public int MaxSpeed;
    [Export]
    public float RotationSpeed;
    [Export]
    public float GunCooldown;
    [Export]
    public int MaxHealth;
    [Export]
    public int GunShots=1;
    [Export(PropertyHint.Range,"0,1.5")]
    public float GunSpread = .2f;
    [Export]
    public int MaxAmmo=20;
    private int _ammo = -1;
    [Export]
    public int Ammo
    {
       get {return _ammo;}
       set {SetAmmo(value);}
    }

     public Tank()
     {
         
     }

     public override void _Ready()
     {
        gunTimer = (Timer)GetNode("GunTimer");
        gunTimer.WaitTime=GunCooldown;
        turret = (Sprite)GetNode("Turret");
        muzzle = (Position2D)turret.GetNode("Muzzle");
        gunTimer.Connect("timeout",this,"OnGunTimer");
        player = GetNode<AnimationPlayer>("AnimationPlayer");
        hitbox = GetNode<CollisionShape2D>("CollisionShape2D");
        explosion = GetNode<AnimatedSprite>("Explosion");
        body = GetNode<Sprite>("Body");
        health=MaxHealth;
        EmitSignal("HealthChanged",health*100f/MaxHealth);
        EmitSignal("AmmoChanged",Ammo *100/MaxAmmo);
        explosion.Connect("animation_finished",this,"ExplosionFinished");
     }

     public virtual void Control(float delta)
     {

     }

     public override void _PhysicsProcess(float delta)
     {
         if(!alive)
            return;
        Control(delta);
        MoveAndSlide(velocity);
     }


      public void OnGunTimer()
      {
         canShoot=true;
      }
     public void Fire(int number, float spread, Node2D target = null)
     {
        if(canShoot && Ammo!=0)
        {
           Ammo--;
           canShoot=false;
           gunTimer.Start();
           var dir = new Vector2(1,0).Rotated(turret.GlobalRotation);
           if(number>1)
           {
              for(int i=0; i<number; i++)
              {
                 var a = -spread + i * (2*spread)/(number-1);
                 EmitSignal("Shoot", Bullet,muzzle.GlobalPosition, dir.Rotated(a), target);
              }
           }
           else
              EmitSignal("Shoot", Bullet,muzzle.GlobalPosition, dir, target);
           player.Play("MuzzleFlash");
        }
     }

    public void TakeDamage(float damage)
    {
        health-=damage;
        EmitSignal("HealthChanged",health*100f/MaxHealth);
        if(health<=0)
         Explode();
    }

    public void Heal(float amount)
    {
         health+=amount;
         health = Mathf.Clamp(health,0,MaxHealth);
         EmitSignal("HealthChanged", health*100f/MaxHealth);
    }

    public void Explode()
    {
       EmitSignal("Dead");
       hitbox.Disabled = true;
       alive=false;
       turret.Hide();
       body.Hide();
       explosion.Show();
       explosion.Play();
    }

    public void ExplosionFinished()
    {
       QueueFree();
    }

    public void SetAmmo(int value)
    {
       if(value>MaxAmmo)
       {
          value=MaxAmmo;
       }
       _ammo=value;
       EmitSignal("AmmoChanged",Ammo *100/MaxAmmo);
    }
}
