using Godot;
using System;

public class DebugOverlay : Node2D
{
    private struct TrackedStat
    {
        public string propertyName;
        public WeakRef objectRef;
        public bool isFunc;
        public string nameOverride;

        public TrackedStat(string propertyName, WeakRef objectRef, bool isFunc = false, string nameOverride = "")
        {
            this.propertyName = propertyName;
            this.objectRef = objectRef;
            this.isFunc = isFunc;
            this.nameOverride = nameOverride;
        }
    }

    private Font _tinyFont;
    private System.Collections.Generic.List<TrackedStat> _trackedStats = new System.Collections.Generic.List<TrackedStat>();
    public bool ShowFPS = true;
    public bool ShowMemory = true;

    public override void _Ready()
    {
        _tinyFont = GD.Load<Font>("res://font/tiny_font.tres");

        Modulate = new Color(1f, 1f, 1f, .6f);
    }

    public override void _Process(float delta)
    {
        Update();
    }

    public override void _Draw()
    {
        int ind = 0;

        if (ShowFPS)
        {
            DrawRightAlignShadowedString(Engine.GetFramesPerSecond().ToString() + "fps", ind);
            ind++;
        }
        if (ShowMemory)
        {
            DrawRightAlignShadowedString("Memory: " + ToFileSize(OS.GetStaticMemoryUsage()), ind);
            ind++;
        }
        foreach (TrackedStat stat in _trackedStats)
        {
            if (stat.objectRef.GetRef() is Godot.Object statObj)
            {
                if ((stat.isFunc ? statObj.Call(stat.propertyName) : statObj.Get(stat.propertyName)) != null)
                {
                    if ((stat.isFunc ? statObj.Call(stat.propertyName) : statObj.Get(stat.propertyName)) is Color color)
                    {
                        DrawColourProperty((stat.nameOverride == "" ? stat.propertyName : stat.nameOverride) + ": ", ind, color);
                    }
                    else
                        DrawRightAlignShadowedString((stat.nameOverride == "" ? stat.propertyName : stat.nameOverride) + ": " + (stat.isFunc ? statObj.Call(stat.propertyName).ToString() : statObj.Get(stat.propertyName).ToString()), ind);

                    ind++;
                }
                else    // helps with the godot c# problem where some methods like Call and Get use snake_case for builtin names
                {
                    if ((stat.isFunc ? statObj.Call(stat.propertyName.ToLower()) : statObj.Get(stat.propertyName.ToLower())) != null)
                    {
                        DrawRightAlignShadowedString((stat.nameOverride == "" ? stat.propertyName : stat.nameOverride) + ": " + (stat.isFunc ? statObj.Call(stat.propertyName.ToLower()).ToString() : statObj.Get(stat.propertyName.ToLower().ToString())), ind);
                        ind++;
                    }
                }
            }
        }
    }

    public void TrackProperty(string propertyName, Godot.Object objectRef, string nameOverride = "")
    {
        _trackedStats.Add(new TrackedStat(propertyName, WeakRef(objectRef), false, nameOverride));
    }

    public void TrackFunc(string funcName, Godot.Object objectRef, string nameOverride = "")
    {
        _trackedStats.Add(new TrackedStat(funcName, WeakRef(objectRef), true, nameOverride));
    }

    private void DrawRightAlignShadowedString(string str, int idx, Vector2 offset = new Vector2())
    {
        float xOffset = _tinyFont.GetStringSize(str).x;

        DrawString(_tinyFont, offset + new Vector2(1 - xOffset, 5 + (idx * 6)), str, Colors.Black);
        DrawString(_tinyFont, offset + new Vector2(-xOffset, 4 + (idx * 6)), str);

    }

    private void DrawColourProperty(string str, int idx, Color color)
    {
        DrawRightAlignShadowedString(str, idx, new Vector2(-8f, 0f));
        DrawRect(new Rect2(new Vector2(-10, 1 + (idx * 6)), new Vector2(10, 4)), color);
    }



    // Following code from Rod Stephens @ http://csharphelper.com
    private string ToFileSize(double value)
    {
        string[] suffixes = { "bytes", "KB", "MB", "GB",
        "TB", "PB", "EB", "ZB", "YB"};
        for (int i = 0; i < suffixes.Length; i++)
        {
            if (value <= (Math.Pow(1024, i + 1)))
            {
                return ThreeNonZeroDigits(value /
                    Math.Pow(1024, i)) +
                    " " + suffixes[i];
            }
        }

        return ThreeNonZeroDigits(value /
            Math.Pow(1024, suffixes.Length - 1)) +
            " " + suffixes[suffixes.Length - 1];
    }
    private string ThreeNonZeroDigits(double value)
    {
        if (value >= 100)
        {
            return value.ToString("0,0");
        }
        else if (value >= 10)
        {
            return value.ToString("0.0");
        }
        else
        {
            return value.ToString("0.00");
        }
    }
}
