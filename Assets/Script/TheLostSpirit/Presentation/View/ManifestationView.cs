using Animancer;
using Animancer.FSM;
using Sirenix.OdinInspector;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel;
using UnityEditorInternal;
using UnityEngine;
using State = Animancer.FSM.State;

namespace TheLostSpirit.Presentation.View
{
    public class ManifestationView : MonoBehaviour, IView<ManifestationID, ManifestationViewModel>
    {
        [SerializeField, Required, ChildGameObjectsOnly]
        AnimancerComponent _animator;

        ManifestationViewModel _viewModel;

        public ManifestationView Initialize(AnimationClip clip) {
            return this;
        }

        public void Bind(ManifestationViewModel viewModel) {
            _viewModel = viewModel;
        }

        public void Unbind() { }


        public void AnimationEventCallBack(AnimationEvent animationEvent) {
            var clip         = animationEvent.animatorClipInfo.clip;
            var currentFrame = Mathf.RoundToInt(clip.frameRate * animationEvent.time);

            _viewModel.OnFrame(currentFrame);

            var isFinish = currentFrame == Mathf.RoundToInt(clip.frameRate * clip.length);

            if (!isFinish) return;

            _viewModel.OnFinish();
        }
    }
}