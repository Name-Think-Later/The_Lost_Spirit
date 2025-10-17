using System.Collections;
using System.Collections.Generic;
using TheLostSpirit.Identify;
using TheLostSpirit.ViewModel.Portal;

namespace TheLostSpirit.Application.ViewModelStore {
    public class PortalViewModelStore : IViewModelStore<PortalID, PortalViewModel>,
                                        IEnumerable<KeyValuePair<PortalID, PortalViewModel>> {
        readonly Dictionary<PortalID, PortalViewModel> _dictionary = new();


        public void Add(PortalViewModel viewModel) {
            _dictionary[viewModel.ID] = viewModel;
        }

        public void Remove(PortalID id) {
            _dictionary.Remove(id);
        }

        public PortalViewModel GetByID(PortalID id) {
            return _dictionary[id];
        }

        public bool HasID(PortalID id) {
            return _dictionary.ContainsKey(id);
        }

        public IEnumerator<KeyValuePair<PortalID, PortalViewModel>> GetEnumerator() {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}