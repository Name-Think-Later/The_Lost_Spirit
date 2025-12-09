using Sirenix.OdinInspector;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel;
using UnityEngine;

namespace TheLostSpirit.Presentation.View
{
    public class ManifestationView : MonoBehaviour, IView<ManifestationID, ManifestationViewModel>
    {
        [SerializeField, Required, ChildGameObjectsOnly]
        Animator _animator;


        ManifestationViewModel _viewModel;

        public void Bind(ManifestationViewModel viewModel) {
            _viewModel = viewModel;
        }

        public void Unbind() { }


        public void AnimationEventCallBack(AnimationEvent animationEvent) {
            var clip         = animationEvent.animatorClipInfo.clip;                   //动画片段名称
            var currentFrame = Mathf.FloorToInt(clip.frameRate * animationEvent.time); //动画片段当前帧 向下取整

            _viewModel.TriggerEffect(0);
        }
    }
}