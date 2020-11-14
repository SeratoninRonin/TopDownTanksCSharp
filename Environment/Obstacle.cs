using Godot;
using System;
using Godot.Collections;

[Tool]
public class Obstacle : StaticBody2D
{
    Sprite sprite;
    CollisionShape2D shape;
    public enum Items 
    {
        barrelBlack_side,
        barrelBlack_top,
        barrelGreen_side,
        barrelGreen_top,
        barrelRed_side,
        barrelRed_top,
        barrelRust_side,
        barrelRust_top,
        barricadeMetal,
        barricadeWood,
        fenceRed,
        fenceYellow,
        sandbagBeige,
        sandbagBeige_open,
        sandbagBrown,
        sandbagBrown_open,
        treeBrown_large,
        treeBrown_small,
        treeGreen_large,
        treeGreen_small,
    }

    private Items _type;
    [Export]
    public Items Type
    {
        get {return _type;}
        set
        {
            UpdateType(value);
        }
    }

    private async void UpdateType(Items val)
    {
            _type=val;
            if(!Engine.EditorHint)
                await ToSignal(this,"ready");
            sprite.RegionRect=(Rect2)regions[_type];
            var rect = new RectangleShape2D();
            rect.Extents = sprite.RegionRect.Size/2;
            shape.Shape=rect;
    }

    Dictionary regions;
    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        shape = GetNode<CollisionShape2D>("CollisionShape2D");

        regions = new Dictionary();
        regions.Add(Items.barrelBlack_side, new Rect2(532,90,56,40));
        regions.Add(Items.barrelBlack_top, new Rect2(220,89,48,48));
        regions.Add(Items.barrelGreen_side, new Rect2(476,90,56,40));
        regions.Add(Items.barrelGreen_top, new Rect2(220,137,48,48));
        regions.Add(Items.barrelRed_side, new Rect2(420,94,56,40));
        regions.Add(Items.barrelRed_top, new Rect2(172,89,48,48));
        regions.Add(Items.barrelRust_side, new Rect2(588,90,56,40));
        regions.Add(Items.barrelRust_top, new Rect2(172,137,48,48));
        regions.Add(Items.barricadeMetal, new Rect2(532,130,56,56));
        regions.Add(Items.barricadeWood, new Rect2(72,130,56,56));
        regions.Add(Items.fenceRed, new Rect2(336,443,32,96));
        regions.Add(Items.fenceYellow, new Rect2(216,550,32,104));
        regions.Add(Items.sandbagBeige, new Rect2(164,282,44,64));
        regions.Add(Items.sandbagBeige_open, new Rect2(518,350,55,84));
        regions.Add(Items.sandbagBrown, new Rect2(622,278,44,64));
        regions.Add(Items.sandbagBrown_open, new Rect2(596,450,55,84));
        regions.Add(Items.treeBrown_large, new Rect2(0,654,128,128));
        regions.Add(Items.treeBrown_small, new Rect2(694,118,72,72));
        regions.Add(Items.treeGreen_large, new Rect2(128,654,128,128));
        regions.Add(Items.treeGreen_small, new Rect2(694,190,72,72));
    }
}
