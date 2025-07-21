using UnityEngine;

namespace Script.TheLostSpirit.Circuit.SkillSystem.CoreModule {
    [CreateAssetMenu(fileName = "CoreModelWrapper", menuName = "CoreModel", order = 0)]
    public class CoreModelWrapper : ScriptableObject {
        [SerializeField]
        Core.Model _coreModel;
    }
}