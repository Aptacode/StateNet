using Aptacode.StateNet.Events;

namespace Aptacode.StateNet
{
    public class Node
    {
        public Node(string name) => Name = name;

        #region Overrides
        public int GetHashCode(Node obj) => Name.GetHashCode();
        public override bool Equals(object obj) => (obj is Node other) && Equals(other);
        public bool Equals(Node other) => Name.Equals(other.Name);
        #endregion

        #region Internal
        internal void Visit() => OnVisited?.Invoke(this);
        internal void Exit() => OnExited?.Invoke(this);
        internal void UpdateChoosers() => OnUpdateChoosers?.Invoke(this);
        #endregion


        public event NodeEvent OnVisited;
        public event NodeEvent OnUpdateChoosers;
        public event NodeEvent OnExited;

        public override string ToString() => Name;

        //public override string ToString()
        //{
        //    var stringBuilder = new StringBuilder();

        //    var pairs = _Choosers.ToList();
        //    if (pairs.Count > 0)
        //    {
        //        stringBuilder.Append($"({pairs[0].Key}->{pairs[0].Value})");
        //        for (int i = 1; i < pairs.Count; i++)
        //        {
        //            stringBuilder.Append($",({pairs[i].Key}->{pairs[i].Value})");
        //        }
        //    }

        //    return $"{Name}{(IsEndNode ? "" : ":" + stringBuilder.ToString())}";
        //}
        public string Name { get; }
    }
}


