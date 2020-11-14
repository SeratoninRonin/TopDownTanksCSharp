using Godot;
using System;

public class UnitDisplay : Node2D
{
    Texture barRed;
    Texture barGreen;
    Texture barYellow;
    Texture barTexture;
    TextureProgress healthBar;
    
    public override void _Ready()
    {
        Load();
        healthBar.Hide();
    }

    public override void _Process(float delta)
    {
        GlobalRotation=0;
    }
    public void Load()
    {
        barRed = (Texture)GD.Load("res://Assets/UI/barHorizontal_red_mid 200.png");
        barYellow = (Texture)GD.Load("res://Assets/UI/barHorizontal_yellow_mid 200.png");
        barGreen = (Texture)GD.Load("res://Assets/UI/barHorizontal_green_mid 200.png");
        healthBar = (TextureProgress)GetNode("HealthBar");
    }

    public void UpdateHealthBar(float value)
    {
        healthBar.TextureProgress_ = barGreen;
        if(value<60)
            healthBar.TextureProgress_=barYellow;
        if(value<25)
            healthBar.TextureProgress_=barRed;
        if(value<100)
            healthBar.Show();
        healthBar.Value = value;
    }
}
