using PsbCGEnumerator.Models;

namespace PsbCGEnumerator
{
    public static class Utils
    {
        public static string GetLayerJsonFromResxJson(string resxJsonPath)
        {
            var directory = Path.GetDirectoryName(resxJsonPath);
            var resxFileName = Path.GetFileName(resxJsonPath);
            var jsonFileName = resxFileName.Replace(".resx.json", ".json");
            return Path.Combine(directory!, jsonFileName);  // 传入null也完全没有问题
        }

        public static Dictionary<string, string> GetImagePathDict(string resxJsonPath, ResxModel resx)
        {
            var result = new Dictionary<string, string>();
            var directory = Path.GetDirectoryName(resxJsonPath);
            foreach (var kvp in resx.Resources)
                result.Add(kvp.Key, Path.Combine(directory!, kvp.Value));
            return result;
        }
    }
}
