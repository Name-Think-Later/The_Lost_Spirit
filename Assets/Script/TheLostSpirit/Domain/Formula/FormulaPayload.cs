using System;
using System.Collections.Generic;
using MoreLinq;
using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Domain.Formula
{
    public class FormulaPayload
    {
        readonly NodeID[] _nodeRoute;


        public readonly PayloadSeq seq;
        public          bool       isLastChild;

        public List<AnchorID> LastAnchors { get; private set; }
        public List<AnchorID> NewAnchors { get; private set; }


        public FormulaPayload() {
            seq         = PayloadSeq.New();
            _nodeRoute  = new NodeID[1];
            LastAnchors = new List<AnchorID>();
            NewAnchors  = new List<AnchorID>();
        }


        FormulaPayload(FormulaPayload origin) {
            this.seq = origin.seq;

            var newNodeRoute = origin._nodeRoute;
            Array.Resize(ref newNodeRoute, newNodeRoute.Length + 1);
            _nodeRoute = newNodeRoute;

            this.LastAnchors = new List<AnchorID>(origin.LastAnchors);
            this.NewAnchors  = new List<AnchorID>(origin.NewAnchors);
        }

        public void PushAnchors() {
            (LastAnchors, NewAnchors) = (NewAnchors, LastAnchors);
            NewAnchors.Clear();
        }

        public FormulaPayload Clone() {
            return new FormulaPayload(this);
        }


        public void AddRoute(NodeID nodeID) {
            _nodeRoute[^1] = nodeID;
        }

        public void ShowRoute() {
            Debug.Log(_nodeRoute.ToDelimitedString("\n"));
        }
    }

    public record PayloadSeq : RuntimeID<PayloadSeq>;
}