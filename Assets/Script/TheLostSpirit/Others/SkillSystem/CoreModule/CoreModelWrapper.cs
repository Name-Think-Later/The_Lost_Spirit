using UnityEngine;

namespace TheLostSpirit.Others.SkillSystem.CoreModule {
    [CreateAssetMenu(fileName = "CoreModelWrapper", menuName = "CoreModel", order = 0)]
    public class CoreModelWrapper : ScriptableObject {
        [SerializeField]
        CoreModel _model;
    }
}