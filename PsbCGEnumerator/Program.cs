using FreeMote;

using PsbCGEnumerator;
using PsbCGEnumerator.Models;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using System.Text.Json;

Console.WriteLine("""====PsbCGEnumerator====""");
if (args.Length != 1 || !args[0].EndsWith(".resx.json"))
    "请传入一个从PsbDecompiler得到的resx.json文件".E();

var resxJsonFilePath = args[0];
if (!File.Exists(resxJsonFilePath))
    "resx.json文件不存在".E();

ResxModel? resx;
using (var resxFile = File.OpenRead(resxJsonFilePath))
{
    resx = await JsonSerializer.DeserializeAsync<ResxModel>(resxFile);
    if (resx == null)
    {
        "无法读取resx.json文件内容，或许文件已损坏".E();
        return;  // 防止下面报CS8602
    }
}

if (resx.PsbType != PsbType.Pimg)
    $"Psb类型必须为Pimg，当前为{resx.PsbType}".E();

var imagePaths = Utils.GetImagePathDict(resxJsonFilePath, resx);
var layerJsonFilePath = Utils.GetLayerJsonFromResxJson(resxJsonFilePath);

$"正在从 '{layerJsonFilePath}' 读取层级信息".I();
if (!File.Exists(layerJsonFilePath))
    $"无法找到层级信息文件，请确保其存在".E();

LayerInfoModel? layerInfo;
using (var layerInfoFile = File.OpenRead(layerJsonFilePath))
{
    layerInfo = await JsonSerializer.DeserializeAsync<LayerInfoModel>(layerInfoFile);
    if (layerInfo == null)
    {
        "无法读取层级信息，或许文件已损坏".E();
        return;  // 防止下面报CS8602
    }
}

$"正在枚举合并队列".I();
var cgTrees = CGPieceTreeBuilder.Build(resx, layerInfo);
var cfg = Configuration.Default.Clone();
cfg.PreferContiguousImageBuffers = true;
foreach (var cgTree in cgTrees)
{
    await Step(cgTree);
}

async Task Step(CGPiece piece)
{
    if (piece.Image == null)
        piece.Image = await Image.LoadAsync<Rgba32>(imagePaths[piece.Layer.LayerId.ToString()]);
    if (piece.Children.Count != 0)
    {
        foreach (var child in piece.Children)
        {
            await Step(child);
        }
        piece.Image?.Dispose();
        return;
    }

    $"正在合成{piece.Layer.Name}".I();
    var list = new List<CGPiece>();
    var root = piece;
    while (root != null)
    {
        list.Add(root);
        root = root.Parent;
    }

    var image = list[list.Count - 1].Image!.Clone();
    for (var i = list.Count - 2; i >= 0; i--)
    {
        var pos = new Point(list[i].Layer.Left, list[i].Layer.Top);
        image.Mutate(x => x.DrawImage(list[i].Image, pos, piece.Layer.Opacity / 255f));
    }

    var saveName = Path.GetFileNameWithoutExtension(layerJsonFilePath) + $"_{piece.Layer.Name}.png";
    await image.SaveAsPngAsync(saveName);
    image.Dispose();
    piece.Image?.Dispose();
}