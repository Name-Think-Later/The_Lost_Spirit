using Script.TheLostSpirit.Presentation.ViewModel.Port;
using TheLostSpirit.Identify;

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