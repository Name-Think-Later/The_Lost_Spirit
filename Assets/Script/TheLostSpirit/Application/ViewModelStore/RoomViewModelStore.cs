using System.Collections;
using System.Collections.Generic;
using TheLostSpirit.Identify;
using TheLostSpirit.ViewModel.Room;

namespace TheLostSpirit.Application.ViewModelStore {
    public class RoomViewModelStore : IViewModelStore<RoomID, RoomViewModel> {
        readonly Dictionary<RoomID, RoomViewModel> _dictionary = new();

        public IEnumerator<KeyValuePair<RoomID, RoomViewModel>> GetEnumerator() {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(RoomViewModel viewModel) {
            _dictionary[viewModel.ID] = viewModel;
        }

        public void Remove(RoomID id) {
            _dictionary.Remove(id);
        }

        public RoomViewModel GetByID(RoomID id) {
            return _dictionary[id];
        }

        public bool HasID(RoomID id) {
            return _dictionary.ContainsKey(id);
        }
    }
}