using System.Collections.Generic;

namespace Models
{
    public class TNode
    {
        public string Name { get; set; }

        public TNode ParentNode { get; set; }
        public List<TNode> ChildNodes { get; set; }
    }
}
