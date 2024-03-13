using PsbCGEnumerator.Models;

namespace PsbCGEnumerator
{
    public static class CGPieceTreeBuilder
    {
        public class Node : Dictionary<char, Node>
        {
            public LayerModel? layer;
        }

        public static List<CGPiece> Build(ResxModel resx, LayerInfoModel layerInfo)
        {
            Node tree = new();

            void Insert(Node node, LayerModel layer, int index)
            {
                // if ((index == layer.Name.Length - 1) || (index + 1 == layer.Name.Length - 1 && layer.Name[index + 1] is 'a' or 'A'))
                if (index == layer.Name.Length)
                {
                    node.layer = layer;
                    return;
                }
                if (!node.TryGetValue(layer.Name[index], out var nextNode))
                {
                    nextNode = new();
                    node.Add(layer.Name[index], nextNode);
                }
                Insert(nextNode, layer, index + 1);
            };

            foreach (var layer in layerInfo.Layers)
            {
                if (layer.Name.Length > 2)
                {
                    throw new NotSupportedException("此CG差分信息不受支持，请提交给开发者");
                }
                Insert(tree, layer, 0);
            }

            var result = new Dictionary<char, CGPiece>();
            void FindAndAdd(char head, KeyValuePair<char, Node> kvp)
            {
                if (kvp.Value.TryGetValue(head, out var first))
                    result[kvp.Key] = new CGPiece(null, first.layer!);
            }
            foreach (var kvp in tree)
            {
                FindAndAdd('a', kvp);
                FindAndAdd('A', kvp);
                foreach (var kvp2 in kvp.Value)
                {
                    if (kvp2.Key is 'a' or 'A')
                        continue;
                    result[kvp.Key].AddChild(new CGPiece(result[kvp.Key], kvp2.Value.layer!));
                }
            }
            return result.Values.ToList();
        }
    }
}
