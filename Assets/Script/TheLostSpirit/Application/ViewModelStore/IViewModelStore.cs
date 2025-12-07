using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;

namespace TheLostSpirit.Application.ViewModelStore
{
    public interface IViewModelStore<TId>
        where TId : IRuntimeID
    {
        void Save(IViewModelReference<TId> viewModelReference);

        IViewModelReference<TId> GetByID(TId id);

        void Remove(TId id);

        bool HasID(TId id);

        void Clear();
    }
}