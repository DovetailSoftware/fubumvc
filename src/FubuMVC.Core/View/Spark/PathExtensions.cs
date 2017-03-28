using System.Text.RegularExpressions;

namespace FubuMVC.Core.View.Spark
{
    public static class PathExtensions
    {
        private static readonly Regex _parseOriginRegex = new Regex(@"_?([^\\/]+)[\\/]?", RegexOptions.Compiled);

        public static string GetOrigin(this string path)
        {
            var match = _parseOriginRegex.Match(path);
            return match.Groups[1].Value;
        }
    }
}