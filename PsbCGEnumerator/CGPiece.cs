using PsbCGEnumerator.Models;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PsbCGEnumerator
{
    public class CGPiece
    {
        public LayerModel Layer { get; init; }
        public CGPiece? Parent { get; init; }
        public List<CGPiece> Children { get; init; } = new();
        public Image<Rgba32>? Image { get; set; }

        public CGPiece(CGPiece? parent, LayerModel layer)
        {
            Parent = parent;
            Layer = layer;
        }

        public void AddChild(CGPiece piece)
        {
            Children.Add(piece);
        }
    }
}
