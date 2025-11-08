using Script.TheLostSpirit.Presentation.View.Input;
using UnityEngine;

namespace TheLostSpirit.Context
{
    public class UserInputContext : MonoBehaviour
    {
        public GeneralInputView GeneralInputView { get; private set; }

        public UserInputContext Construct() {
            var actionMap = new ActionMap();

            GeneralInputView = new GeneralInputView(actionMap.General);

            actionMap.Enable();

            return this;
        }
    }
}