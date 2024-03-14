using PsbCGEnumerator.Models;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using SImage = SixLabors.ImageSharp.Image;

namespace PsbCGEnumerator
{
    public class CGPiece
    {
        public LayerModel Layer { get; init; }
        public CGPiece? Parent { get; init; }
        public List<CGPiece> Children { get; init; } = new();
        public Image<Rgba32>? MergedImage { get; set; }
        private Image<Rgba32>? image;
        private int referCount = 0;
        private Task? mergeTask;
        private Task? loadTask;

        public CGPiece(CGPiece? parent, LayerModel layer)
        {
            Parent = parent;
            Layer = layer;
        }

        public void AddChild(CGPiece piece)
        {
            Children.Add(piece);
        }

        public Task LoadImage(Dictionary<string, string> imagePaths)
        {
            if (image != null)
                return Task.CompletedTask;
            if (loadTask == null)
            {
                loadTask = Task.Run(() =>
                {
                    image = SImage.Load<Rgba32>(imagePaths[Layer.LayerId.ToString()]);
                });
            }
            return loadTask;
        }

        public Task MergeImage(Dictionary<string, string> imagePaths)
        {
            if (MergedImage != null)
                return Task.CompletedTask;
            if (mergeTask == null)
            {
                mergeTask = Task.Run(async () =>
                {
                    var loadSelfTask = LoadImage(imagePaths);
                    var loadParentTask = Parent != null ? Parent.MergeImage(imagePaths) : Task.CompletedTask;
                    await loadSelfTask.ConfigureAwait(false);
                    await loadParentTask.ConfigureAwait(false);

                    if (Parent == null)
                        MergedImage = image!.Clone();
                    else
                    {
                        MergedImage = Parent.MergedImage!.Clone();
                        var pos = new Point(Layer.Left, Layer.Top);
                        MergedImage.Mutate(x => x.DrawImage(image!, pos, Layer.Opacity / 255f));
                        Parent.DecreaseMergeCount();
                    }
                });
                referCount = Children.Count;
            }
            return mergeTask;
        }

        public void Dispose()
        {
            image?.Dispose();
            MergedImage?.Dispose();
        }

        public void SavePngAndDispose(string savePath)
        {
            if (Children.Count != 0)
                throw new InvalidOperationException();
            $"正在保存{Layer.Name}".I();
            MergedImage.SaveAsPng(savePath);
            Dispose();
        }

        public void DecreaseMergeCount()
        {
            if (Interlocked.Decrement(ref referCount) == 0)
            {
                Dispose();
            }
        }
    }
}
