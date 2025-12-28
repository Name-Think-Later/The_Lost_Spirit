using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Script.TheLostSpirit.Infrastructure.Editor
{
    [DrawerPriority(0.01)]
    public class ListItemSelectorAttributeDrawer : OdinAttributeDrawer<ListItemSelectorAttribute>
    {
        static readonly Color              selectedColor = new Color(0.301f, 0.563f, 1f, 0.1f);
        InspectorProperty                  baseMemberProperty;
        PropertyContext<InspectorProperty> globalSelectedProperty;
        bool                               isListElement;
        Action<object, int>                selectedIndexSetter;
        InspectorProperty                  selectedProperty;

        protected override void Initialize() {
            isListElement =
                Property.Parent != null && Property.Parent.ChildResolver is IOrderedCollectionResolver;
            var isList       = !isListElement;
            var listProperty = isList ? Property : Property.Parent;
            baseMemberProperty = listProperty.FindParent(x => x.Info.PropertyType == PropertyType.Value, true);

            globalSelectedProperty =
                baseMemberProperty.Context.GetGlobal("selectedIndex" + baseMemberProperty.GetHashCode(),
                                                     (InspectorProperty)null);

            if (isList) {
                var parentType = baseMemberProperty.ParentValues[0].GetType();
                selectedIndexSetter =
                    EmitUtilities.CreateWeakInstanceMethodCaller<int>(
                        parentType.GetMethod(Attribute.SetSelectedMethod, Flags.AllMembers));
            }
        }

        protected override void DrawPropertyLayout(GUIContent label) {
            var t = Event.current.type;

            if (isListElement) {
                if (t == EventType.Layout) {
                    CallNextDrawer(label);
                }
                else {
                    var rect       = GUIHelper.GetCurrentLayoutRect();
                    var isSelected = globalSelectedProperty.Value == Property;

                    if (t == EventType.Repaint && isSelected) {
                        EditorGUI.DrawRect(rect, selectedColor);
                    }
                    else if (t == EventType.MouseDown && rect.Contains(Event.current.mousePosition)) {
                        globalSelectedProperty.Value = Property;
                    }

                    CallNextDrawer(label);
                }
            }
            else {
                CallNextDrawer(label);

                if (Event.current.type != EventType.Layout) {
                    var sel = globalSelectedProperty.Value;

                    // Select
                    if (sel != null && sel != selectedProperty) {
                        selectedProperty = sel;
                        Select(selectedProperty.Index);
                    }
                    // Deselect when destroyed
                    else if (selectedProperty != null &&
                             selectedProperty.Index < Property.Children.Count &&
                             selectedProperty != Property.Children[selectedProperty.Index]) {
                        var index = -1;
                        Select(index);
                        selectedProperty             = null;
                        globalSelectedProperty.Value = null;
                    }
                }
            }
        }

        void Select(int index) {
            GUIHelper.RequestRepaint();
            Property.Tree.DelayAction(() => {
                for (var i = 0; i < baseMemberProperty.ParentValues.Count; i++)
                    selectedIndexSetter(baseMemberProperty.ParentValues[i], index);
            });
        }
    }
}