using System;
using System.Collections.Generic;
using MoreLinq;
using TheLostSpirit.Domain.Formula.Node;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Domain.Formula
{
    public class FormulaPayload
    {
        public Guid FormulaStreamID { get; private set; }
        public List<NodeID> NodeRoute { get; private set; }
        public List<AnchorID> Anchors { get; private set; }
        public List<AnchorID> CandidateAnchors { get; private set; }
        public TraversalPolicy TraversalPolicy { get; set; }


        public FormulaPayload(Guid formulaStreamID) {
            FormulaStreamID  = formulaStreamID;
            NodeRoute        = new List<NodeID>();
            Anchors          = new List<AnchorID>();
            CandidateAnchors = new List<AnchorID>();
            TraversalPolicy  = TraversalPolicy.Sequential;
        }

        public void PromoteCandidateAnchors() {
            Anchors.Clear();
            Anchors.AddRange(CandidateAnchors);
            CandidateAnchors.Clear();
        }

        public FormulaPayload Clone() {
            var clone = new FormulaPayload(FormulaStreamID);
            clone.NodeRoute.AddRange(NodeRoute);
            clone.Anchors.AddRange(Anchors);
            clone.CandidateAnchors.AddRange(CandidateAnchors);
            clone.TraversalPolicy = TraversalPolicy;

            return clone;
        }


        public void AddRoute(NodeID nodeID) {
            NodeRoute.Add(nodeID);
        }

        public void ShowRoute() {
            Debug.Log(NodeRoute.ToDelimitedString("\n"));
        }
    }
}