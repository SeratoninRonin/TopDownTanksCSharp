using Godot;
using System;

public class HUD : CanvasLayer
{
    Texture barRed;
    Texture barGreen;
    Texture barYellow;
    Texture barTexture;
    TextureProgress healthBar;
    TextureProgress ammoBar;
    Tween tween;
    AnimationPlayer anim;
    Player player;

    public override void _Ready()
    {
        Load();
        //HACK: IDK wtf is going on with Godot here
        //it wires the scene events from the editor BEFORE _Ready() is called:
        //ie, OnSceneLoad wires the events before the nodes.
        //IDK why it does this- and if I do a null check and then GetNode()
        //it usually works, I just don't know if that's 100% of the time or not (subinstancing?)
        UpdateHealthBar(player.MaxHealth*100/player.MaxHealth);
        UpdateAmmoBar(player.Ammo*100/player.MaxAmmo);
    }

    public void Load()
    {
        barRed = (Texture)GD.Load("res://Assets/UI/barHorizontal_red_mid 200.png");
        barYellow = (Texture)GD.Load("res://Assets/UI/barHorizontal_yellow_mid 200.png");
        barGreen = (Texture)GD.Load("res://Assets/UI/barHorizontal_green_mid 200.png");
        healthBar = (TextureProgress)GetNode("Margin/Container/HealthBar");
        ammoBar = (TextureProgress)GetNode("Margin/Container/AmmoBar");
        tween = (Tween)healthBar.GetNode("Tween");
        anim = (AnimationPlayer)GetNode("AnimationPlayer");
        anim.Connect("animation_finished",this,"AnimationFinished");
        player = GetNode<Player>("../Player");
    }
    public void UpdateHealthBar(float value)
    {
        if(healthBar==null)
          return;
        barTexture = barGreen;
        if(value<60)
            barTexture=barYellow;
        if(value<25)
            barTexture=barRed;
        healthBar.TextureProgress_=barTexture;

        tween.InterpolateProperty(healthBar,"value",healthBar.Value,value,0.2f,
            Tween.TransitionType.Linear,Tween.EaseType.InOut);
        tween.Start();
        anim.Play("HealthBarFlash");
    }

    public void UpdateAmmoBar(float value)
    {
        if(ammoBar==null)
            return;
        ammoBar.Value = value;
    }

    public void AnimationFinished(string animation)
    {
        if(animation=="HealthBarFlash")
            healthBar.TextureProgress_=barTexture;
    }
}
