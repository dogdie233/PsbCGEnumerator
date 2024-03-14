using FreeMote;

using PsbCGEnumerator;
using PsbCGEnumerator.Models;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using System.Collections.Concurrent;
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
var saveTaskList = new ConcurrentBag<Task>();
var cgTrees = CGPieceTreeBuilder.Build(resx, layerInfo);
var cfg = Configuration.Default.Clone();
cfg.PreferContiguousImageBuffers = true;
foreach (var cgTree in cgTrees)
{
    Step(cgTree);
}
Task.WaitAll(saveTaskList.ToArray());

void Step(CGPiece piece)
{
    if (piece.Children.Count != 0)
    {
        foreach (var child in piece.Children)
        {
            Step(child);
        }
        return;
    }

    var saveName = Path.GetFileNameWithoutExtension(layerJsonFilePath) + $"_{piece.Layer.Name}.png";
    saveTaskList.Add(Task.Run(async () =>
    {
        await piece.MergeImage(imagePaths);
        piece.SavePngAndDispose(saveName);
    }));
}