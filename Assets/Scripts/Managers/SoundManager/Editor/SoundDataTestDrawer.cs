using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Managers.SoundManager.Test
{
    [CustomPropertyDrawer(typeof(SoundDataTest))]
    public class SoundDataTestDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            var clip = new PropertyField(property.FindPropertyRelative("Clip"));
            var name = new PropertyField(property.FindPropertyRelative("Name"));
            name.SetEnabled(false);
            var id = new PropertyField(property.FindPropertyRelative("_testId"));
            id.SetEnabled(false);
            
            container.Add(clip);
            container.Add(name);
            container.Add(id);

            return container;
        }
    }
}