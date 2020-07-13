using Godot;
using System;

public class DebugOverlay : CanvasLayer
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

    private Label _label;
    private System.Collections.Generic.List<TrackedStat> _trackedStats = new System.Collections.Generic.List<TrackedStat>();
    public bool ShowFPS = true;
    public bool ShowMemory = true;

    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
        _label.Modulate = new Color(1f, 1f, 1f, .6f);
    }

    public override void _Process(float delta)
    {
        String labelText = "";

        if (ShowFPS)
            labelText += Engine.GetFramesPerSecond().ToString() + "fps\n";

        if (ShowMemory)
            labelText += "Memory: " + ToFileSize(OS.GetStaticMemoryUsage()) + "\n";

        foreach (TrackedStat stat in _trackedStats)
        {
            if (stat.objectRef.GetRef() is Godot.Object statObj)
            {
                if ((stat.isFunc ? statObj.Call(stat.propertyName) : statObj.Get(stat.propertyName)) != null)
                {
                    labelText += (stat.nameOverride == "" ? stat.propertyName : stat.nameOverride) + ": " + (stat.isFunc ? statObj.Call(stat.propertyName).ToString() : statObj.Get(stat.propertyName).ToString()) + "\n";
                }
                else    // helps with the godot c# problem where some methods like Call and Get use snake_case for builtin names
                {
                    if ((stat.isFunc ? statObj.Call(stat.propertyName.ToLower()) : statObj.Get(stat.propertyName.ToLower())) != null)
                    {
                        labelText += (stat.nameOverride == "" ? stat.propertyName : stat.nameOverride) + ": " + (stat.isFunc ? statObj.Call(stat.propertyName.ToLower()).ToString() : statObj.Get(stat.propertyName.ToLower().ToString())) + "\n";
                    }
                }
            }
        }

        _label.Text = labelText;
    }

    public void TrackProperty(string propertyName, Godot.Object objectRef, string nameOverride = "")
    {
        _trackedStats.Add(new TrackedStat(propertyName, WeakRef(objectRef), false, nameOverride));
    }

    public void TrackFunc(string funcName, Godot.Object objectRef, string nameOverride = "")
    {
        _trackedStats.Add(new TrackedStat(funcName, WeakRef(objectRef), true, nameOverride));
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
