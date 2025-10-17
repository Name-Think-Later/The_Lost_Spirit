using System.Collections.Generic;
using MoreLinq;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Identify;
using UnityEngine;

namespace TheLostSpirit.Application.Repository {
    public class InteractableRepository : IRepository<IInteractableID, IInteractable> {
        readonly Dictionary<IInteractableID, IInteractable> _dictionary = new();

        public void Add(IInteractable entity) {
            _dictionary[entity.ID] = entity;
        }

        public void Remove(IInteractableID id) {
            _dictionary.Remove(id);
        }

        public IInteractable GetByID(IInteractableID id) {
            return _dictionary[id];
        }

        public bool HasID(IInteractableID id) {
            return _dictionary.ContainsKey(id);
        }

        public void Clear() {
            _dictionary.Clear();
        }


        /// <summary>
        /// if repository is empty then return null
        /// </summary>
        /// <param name="relative"></param>
        /// <returns></returns>
        public IInteractable GetNearest(ITransformProvider relative) {
            if (_dictionary.Count == 0) return null;

            return _dictionary
                   .Values
                   .Minima(interactable => {
                       var interactablePos = interactable.ReadOnlyTransform.Position;
                       var relativePos     = relative.ReadOnlyTransform.Position;

                       return Vector2.Distance(interactablePos, relativePos);
                   })
                   .First();
        }
    }
}