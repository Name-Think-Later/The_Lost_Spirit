using System.Collections;
using System.Collections.Generic;
using MoreLinq;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.DomainDriven;
using UnityEngine;

namespace TheLostSpirit.Domain.Interactable {
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
        
        
        /// <summary>
        /// if repository is empty then return null
        /// </summary>
        /// <param name="relative"></param>
        /// <returns></returns>
        public IInteractable GetNearest(IPositionProvider relative) {
            if (_dictionary.Count == 0) return null;

            return _dictionary
                   .Values
                   .Minima(interactable => {
                       var interactablePos = interactable.Position;
                       var relativePos     = relative.Position;

                       return Vector2.Distance(interactablePos, relativePos);
                   })
                   .First();
        }

        public IEnumerator<KeyValuePair<IInteractableID, IInteractable>> GetEnumerator() {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}