#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

[CustomEditor(typeof(ScriptableObject), true)]
public class ButtonSOInjection : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawButtons();
    }

    private void DrawButtons()
    {
        // Read ButtonSOAttribute.
        var buttonMethods = AttributeReader.ReadMethods<ButtonSOAttribute>(target.GetType());
        int n = buttonMethods.Count();
        if (n > 0)
        {
            // Order it.
            buttonMethods = buttonMethods.OrderBy(m =>
            {
                ButtonSOAttribute attr = AttributeReader.GetMethodAttribute<ButtonSOAttribute>(m);
                return attr.order;
            });

            // Draw.
            for (int i = 0; i < n; i++)
            {
                MethodInfo method = buttonMethods.ElementAt(i);
                ButtonSOAttribute attr = AttributeReader.GetMethodAttribute<ButtonSOAttribute>(method);

                string label = attr.label;
                if (string.IsNullOrEmpty(attr.label))
                    label = Regex.Replace(method.Name, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");

                if (GUILayout.Button(label))
                {
                    method.Invoke(target, null);
                }
            }
        }
    }
}
#endif