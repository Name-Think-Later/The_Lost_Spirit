using UnityEngine;

public class PlaygroundEntryPoint : MonoBehaviour {
    [SerializeField] PlayerReference _playerReference;

    ActionMap _actionMap;

    void Awake() {
        _actionMap = new ActionMap();
    }
}