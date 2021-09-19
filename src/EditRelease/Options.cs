using CommandLine;
using System.Collections.Generic;

namespace EditRelease
{
    public class Options
    {
#if DEBUG
        [Option(shortName: 't', longName: "token", Required = false, HelpText = "Authentication token, use either GITHUB_TOKEN or a Personal Access Token.")]

#else
        [Option(shortName: 't', longName: "token", Required = true, HelpText = "Authentication token, use either GITHUB_TOKEN or a Personal Access Token.")]

#endif
        public string Token { get; set; }

        [Option(shortName: 'i', longName: "id", Required = true, HelpText = "The id of the release, e.g. github.event.release.id.")]
        public int ReleaseId { get; set; }

        [Option(shortName: 'n', longName: "name", Required = false, HelpText = "The name of the release.")]
        public string Name { get; set; }

        [Option(shortName: 'm', longName: "namemode", Required = false, HelpText = "Whether to add or replace the release name.")]
        public string NameMode { get; set; }

        [Option(shortName: 'd', longName: "draft", Required = false, HelpText = "Set true makes the release a draft, false publishes the release.")]
        public bool Draft { get; set; }

        [Option(shortName: 'p', longName: "prerelease", Required = false, HelpText = "Set true to identify the release as a pre-release, false to identify the release as a full release.")]
        public bool Prerelease { get; set; }

        [Option(shortName: 'b', longName: "body", Required = false, HelpText = "Text for the body of the release.")]
        public string Body { get; set; }

        [Option(shortName: 'o', longName: "bodymode", Required = false, HelpText = "Whether to add or replace the release body.")]
        public string BodyMode { get; set; }

        [Option(shortName: 'f', longName: "files", Separator = ',', Required = false, HelpText = "Comma separated list of files whose content will be added after Body.")]
        public IEnumerable<string> Files { get; set; }

        [Option(shortName: 's', longName: "spacing", Required = false, HelpText = "Number of blank lines between each file in the release body.", Default = 1)]
        public int Spacing { get; set; }
    }
}
