using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;

namespace TheLostSpirit.Application.ViewModelStore {
    public interface IViewModelStore<TId>
        where TId : IIdentity {
        void Save(IViewModelOnlyID<TId> viewModelOnlyID);

        IViewModelOnlyID<TId> GetByID(TId id);

        void Remove(TId id);

        bool HasID(TId id);

        void Clear();
    }
}