using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Script.Circuit;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using ZLinq;

public class CircuitNode {
    public class Adjacency {
        public Adjacency() {
            SetAdjency(null);
        }

        public void SetAdjency(
            CircuitNode    node,
            AdjacencyState state = AdjacencyState.In
        ) {
            Node  = node;
            State = state;
        }

        public CircuitNode Node { get; private set; }
        public AdjacencyState State { get; private set; }
        public bool IsExist => Node != null;
        public bool IsEmpty => Node == null;
        public bool IsIn => IsExist && State == AdjacencyState.In;
        public bool IsOut => IsExist && State == AdjacencyState.Out;
    }

    public class AdjacencyList : IList<Adjacency> {
        List<Adjacency> _adjacencies = new List<Adjacency>();

        public AdjacencyList(List<Adjacency> adjacencies) {
            _adjacencies = adjacencies;
        }

        public AdjacencyList(int count) {
            for (int i = 0; i < count; i++) {
                _adjacencies.Add(new Adjacency());
            }
        }

        public AdjacencyList In => new AdjacencyList(this.Where(a => a.IsIn).ToList());
        public AdjacencyList Out => new AdjacencyList(this.Where(a => a.IsOut).ToList());
        public AdjacencyList Exist => new AdjacencyList(this.Where(a => a.IsExist).ToList());
        public AdjacencyList Empty => new AdjacencyList(this.Where(a => !a.IsExist).ToList());

        public void Add(Adjacency item) {
            _adjacencies.Add(item);
        }

        public void Clear() {
            _adjacencies.Clear();
        }

        public bool Contains(Adjacency item) {
            return _adjacencies.Contains(item);
        }

        public void CopyTo(Adjacency[] array, int arrayIndex) {
            _adjacencies.CopyTo(array, arrayIndex);
        }

        public bool Remove(Adjacency item) {
            return _adjacencies.Remove(item);
        }

        public int Count => _adjacencies.Count;
        public bool IsReadOnly { get; }

        public bool Contains(CircuitNode item) {
            return _adjacencies.Select(a => a.Node).Contains(item);
        }

        public void Remove(CircuitNode item) {
            var adjacency = _adjacencies.Find(a => a.Node.Equals(item));
            adjacency.SetAdjency(null);
        }


        public int IndexOf(CircuitNode item) {
            return _adjacencies.FindIndex(a => a.Node.Equals(item));
        }

        public IEnumerator<Adjacency> GetEnumerator() {
            return _adjacencies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public int IndexOf(Adjacency item) {
            return _adjacencies.IndexOf(item);
        }

        public void Insert(int index, Adjacency item) {
            _adjacencies.Insert(index, item);
        }

        public void RemoveAt(int index) {
            _adjacencies.RemoveAt(index);
        }

        public Adjacency this[int index] {
            get => _adjacencies[index];
            set => _adjacencies[index] = value;
        }
    }

    readonly Memory        _memory;
    readonly AdjacencyList _adjacencies;


    public AdjacencyList Adjacencies => _adjacencies;
    public int InDegree => _adjacencies.In.Count;
    public int OutDegree => _adjacencies.Out.Count;

    /// <param name="memory">乘載的記憶</param>
    /// <param name="adjacencyCount">枝度，預設為2</param>
    public CircuitNode(Memory memory, int adjacencyCount = 2) {
        _memory      = memory;
        _adjacencies = new AdjacencyList(adjacencyCount);
    }

    public bool TryConnect(CircuitNode target, int myIndex, int targetIndex) {
        /*
        //target是否為自身
        if (this.Equals(target)) return false;

        //是否已連接target
        //if (AdjacencyEmpty.ContainsValue(target)) return false;
        if (_adjacencies.Exist.Contains(target))

            //索引是否正在被使用
            //if (!AdjacencyExist.ContainsKey(myIndex))
            if (_adjacencies[myIndex].IsExist)
                return false;

        //target索引突觸是否可用
        //if (!target.AdjacencyExist.ContainsKey(targetIndex)) return false;

        if (!target._adjacencies[targetIndex].IsExist) return false;

*/
        _adjacencies[myIndex].SetAdjency(target, AdjacencyState.Out);

        target.Adjacencies[targetIndex].SetAdjency(this, AdjacencyState.In);

        return true;
    }
/*
    /// <summary>
    /// 與目標節點取消連結
    /// </summary>
    /// <param name="target">目標節點</param>
    /// <returns>動作的成功與否</returns>
    public bool TryDisconnectOneWay(CircuitNode target) {
        var mySynapse =
            AdjacencyOut.SingleOrDefault(item => target.Equals(item.Value));

        if (mySynapse.Equals(default(KeyValuePair<int, CircuitNode>))) return false;

        var targetSynapse =
            target.AdjacencyIn
                  .SingleOrDefault(item => this.Equals(item.Value));

        if (targetSynapse.Equals(default(KeyValuePair<int, CircuitNode>))) return false;


        _adjacencyOut.Remove(mySynapse.Key);
        _adjacencyIn.Add(mySynapse.Key, null);
        target.AdjacencyIn[targetSynapse.Key] = null;

        return true;
    }

    public bool TryDisconnect(CircuitNode target) {
        if (TryDisconnectOneWay(target)) return true;

        return target.TryDisconnectOneWay(this);
    }

    /// <summary>
    /// 從自身突觸取消連結
    /// </summary>
    /// <param name="myIndex">自身突觸</param>
    /// <returns>動作的成功與否</returns>
    public bool TryDisconnect(int myIndex) {
        //索引是否存在
        if (!AdjacencyEmpty.ContainsKey(myIndex)) return false;

        return TryDisconnect(AdjacencyEmpty[myIndex]);
    }

    public bool TryDisconnectAll() {
        return AdjacencyEmpty.Values.All(cell => TryDisconnect(cell));
    }
*/

    public override string ToString() {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendFormat($"{_memory.Name} |");
        PrintEachAdjance(_adjacencies);

        stringBuilder.Append("\n");

        stringBuilder.Append("In |");
        PrintEachAdjance(_adjacencies.In);
        stringBuilder.Append("\n");
        
        stringBuilder.Append("Out |");
        PrintEachAdjance(_adjacencies.Out);

        return stringBuilder.ToString();

        void PrintEachAdjance(AdjacencyList adjacencyList) {
            foreach (var item in adjacencyList) {
                var name = item.Node == null ? "null" : item.Node._memory.Name;
                stringBuilder.AppendFormat($" -> [{name} | {item.State}]");
            }
        }
    }
}