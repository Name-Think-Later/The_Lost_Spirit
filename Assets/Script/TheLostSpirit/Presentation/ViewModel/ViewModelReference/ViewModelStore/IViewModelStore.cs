using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Presentation.ViewModel
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