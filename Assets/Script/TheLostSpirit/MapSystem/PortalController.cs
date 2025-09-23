using JetBrains.Annotations;
using R3;
using UnityEngine;

namespace Script.TheLostSpirit.MapSystem {
    public class PortalController {
        readonly PortalReference _portalReference;

        public Transform Anchor => _portalReference.transform;

        [CanBeNull]
        public RoomController Room { get; private set; }

        public bool IsActive { get; private set; }
        public PortalController Destination { get; private set; }

        public PortalController(
            PortalReference portalReference,
            RoomController  roomController = null
        ) {
            _portalReference = portalReference;
            Room             = roomController;
            IsActive         = false;

            Observable
                .EveryUpdate()
                .Subscribe(_ => DebugPortal())
                .AddTo(_portalReference);
        }

        public void Connect(PortalController another) {
            this.Destination    = another;
            another.Destination = this;

            this.IsActive    = true;
            another.IsActive = true;

            this._portalReference.gameObject.SetActive(true);
            another._portalReference.gameObject.SetActive(true);
        }

        public void Disconnect() {
            var another = Destination;

            this.Destination    = null;
            another.Destination = null;

            this.IsActive    = false;
            another.IsActive = false;

            this._portalReference.gameObject.SetActive(false);
            another._portalReference.gameObject.SetActive(false);
        }

        public void Teleport(Transform target) {
            target.position = Destination.Anchor.position;
        }

        void DebugPortal() {
            if (Destination == null) return;
            Debug.DrawLine(Anchor.position, Destination.Anchor.position);
        }
    }
}