using System;

[AttributeUsage(AttributeTargets.Method)]
public class ButtonSOAttribute : Attribute
{
    public string label = "";
    public int order = 100;

    public ButtonSOAttribute() { }
    public ButtonSOAttribute(string label)
    {
        this.label = label;
    }
    public ButtonSOAttribute(int order)
    {
        this.order = order;
    }
    public ButtonSOAttribute(string label, int order)
    {
        this.label = label;
        this.order = order;
    }
}