using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TheLostSpirit.Domain;
using TheLostSpirit.Identity.ConfigID;
using TheLostSpirit.Infrastructure.Domain.ConfigWrapper;
using UnityEngine;

namespace TheLostSpirit.Infrastructure.Database
{
    [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
    public abstract class GenericDatabase<TConfigID, TConfig, TConfigWrapper> : ScriptableObject
        where TConfigID : IConfigID
        where TConfig : IConfig<TConfigID>
        where TConfigWrapper : IConfigWrapper<TConfigID, TConfig>
    {
        [SerializeField, AssetList(AutoPopulate = true), InlineEditor, ReadOnly, PropertySpace(SpaceAfter = 20)]
        List<TConfigWrapper> _database = new List<TConfigWrapper>();


        public TConfigWrapper GetByID(TConfigID id) {
            return _database.First(wrapper => wrapper.ID.Equals(id));
        }

        public bool HasID(TConfigID id) {
            return _database.Any(wrapper => wrapper.ID.Equals(id));
        }
    }
}