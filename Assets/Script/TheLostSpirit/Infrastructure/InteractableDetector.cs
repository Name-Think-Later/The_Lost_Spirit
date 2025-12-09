using System.Collections.Generic;
using System.Linq;
using MoreLinq.Extensions;
using R3;
using R3.Triggers;
using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Interactable;
using UnityEngine;

namespace TheLostSpirit.Infrastructure
{
    public class InteractableDetector : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly]
        Collider2D _collider;

        List<IInteractableComponent> _candidates;

        public IInteractableComponent Target { get; private set; }

        public void Initialize() {
            _candidates = new List<IInteractableComponent>();
            TriggerEnterBinding();
            TriggerExitBinding();
        }

        void TriggerExitBinding() {
            _collider
                .OnTriggerExit2DAsObservable()
                .Subscribe(other => {
                    var isGet = other.TryGetComponent<IInteractableComponent>(out var interactable);

                    if (!isGet) {
                        return;
                    }

                    _candidates.Remove(interactable);
                    interactable.Undetected();

                    Target = _candidates.Any() ? DetectCloses() : null;
                });
        }

        void TriggerEnterBinding() {
            _collider
                .OnTriggerEnter2DAsObservable()
                .Subscribe(other => {
                    var isGet = other.TryGetComponent<IInteractableComponent>(out var interactable);

                    if (!isGet) {
                        return;
                    }

                    _candidates.Add(interactable);

                    Target = DetectCloses();
                });
        }

        IInteractableComponent DetectCloses() {
            var target =
                _candidates
                    .Minima(mono => Vector2.Distance(mono.ReadOnlyTransform.Position, transform.position))
                    .First();

            target.Detected();

            return target;
        }
    }
}