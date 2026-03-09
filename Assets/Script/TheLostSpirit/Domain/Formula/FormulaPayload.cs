using System;
using System.Collections.Generic;
using MoreLinq;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Domain.Formula
{
    public class FormulaPayload
    {
        public List<NodeID> NodeRoute { get; private set; }
        public List<AnchorID> Anchors { get; private set; }
        public List<AnchorID> CandidateAnchors { get; private set; }
        public bool IsLastChild { get; set; }


        public FormulaPayload() {
            NodeRoute        = new List<NodeID>();
            Anchors          = new List<AnchorID>();
            CandidateAnchors = new List<AnchorID>();
        }

        public void PromoteCandidateAnchors() {
            (Anchors, CandidateAnchors) = (CandidateAnchors, Anchors);
            CandidateAnchors.Clear();
        }

        public FormulaPayload Clone() {
            var clone = new FormulaPayload();

            clone.NodeRoute.AddRange(NodeRoute);
            clone.Anchors.AddRange(Anchors);
            clone.CandidateAnchors.AddRange(CandidateAnchors);

            return clone;
        }


        public void AddDebugRoute(NodeID nodeID) {
            NodeRoute.Add(nodeID);
        }

        public void ShowRoute() {
            Debug.Log(NodeRoute.ToDelimitedString("\n"));
        }
    }
}