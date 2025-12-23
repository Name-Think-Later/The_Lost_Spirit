using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TheLostSpirit.Domain;
using TheLostSpirit.Identity.SpecificationID;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Database
{
    [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
    public abstract class GenericDatabase<TSpecificationID, TSpecification>
        : ScriptableObject
        where TSpecificationID : ISpecificationID
        where TSpecification : ISpecification<TSpecificationID>
    {
        [SerializeField, AssetList(AutoPopulate = true), InlineEditor, ReadOnly, PropertySpace(SpaceAfter = 20)]
        List<TSpecification> _database = new List<TSpecification>();


        public TSpecification GetByID(TSpecificationID id) {
            return _database.First(asset => asset.ID.Equals(id));
        }

        public bool HasID(TSpecificationID id) {
            return _database.Any(asset => asset.ID.Equals(id));
        }
    }
}