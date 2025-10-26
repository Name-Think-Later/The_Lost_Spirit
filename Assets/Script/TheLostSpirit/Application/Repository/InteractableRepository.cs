using System.Collections.Generic;
using MoreLinq;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Interactable;
using TheLostSpirit.Identify;
using UnityEngine;

namespace TheLostSpirit.Application.Repository
{
    public class InteractableRepository : GenericRepository<IInteractableID, IInteractable>
    {
        /// <summary>
        /// if repository is empty then return null
        /// </summary>
        /// <param name="relative"></param>
        /// <returns></returns>
        public IInteractable GetNearest(ITransformProvider relative) {
            if (dictionary.Count == 0) return null;

            return dictionary
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