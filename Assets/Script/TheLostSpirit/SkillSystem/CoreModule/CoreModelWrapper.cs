using UnityEngine;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    [CreateAssetMenu(fileName = "CoreModelWrapper", menuName = "CoreModel", order = 0)]
    public class CoreModelWrapper : ScriptableObject {
        [SerializeField]
        CoreModel _model;
    }
}