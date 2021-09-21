using CommandLine;
using System;
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

        [Option(shortName: 'i', longName: "id", Required = true, HelpText = "The id of the release to edit, e.g. github.event.release.id.")]
        public int ReleaseId { get; set; }

        [Option(shortName: 'n', longName: "name", Required = false, HelpText = "New text for the name of the release.")]
        public string Name { get; set; }

        [Option(shortName: 'm', longName: "replacename", Required = false, HelpText = "Set true to replace the release name, false to add to the release name (default).")]
        public string ReplaceNameString { get; set; }

        public bool ReplaceName => !string.IsNullOrWhiteSpace(ReplaceNameString) && ReplaceNameString.Equals("true", StringComparison.OrdinalIgnoreCase);

        [Option(shortName: 'd', longName: "draft", Required = false, HelpText = "Set true to change the release to a draft, false to publish the release. Omit if you do not want to change the draft status of the release.")]
        public string DraftString { get; set; }

        public bool? Draft
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DraftString))
                    return null;
                else if (DraftString.Equals("true", StringComparison.OrdinalIgnoreCase))
                    return true;
                else
                    return false;
            }
        }

        [Option(shortName: 'p', longName: "prerelease", Required = false, HelpText = "Set true to identify the release as a pre-release, false to identify the release as a full release. Omit if you do not want to change the status of the release.")]
        public string PrereleaseString { get; set; }

        public bool? Prerelease
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PrereleaseString))
                    return null;
                else if (PrereleaseString.Equals("true", StringComparison.OrdinalIgnoreCase))
                    return true;
                else
                    return false;
            }
        }

        [Option(shortName: 'b', longName: "body", Required = false, HelpText = "New text for the body of the release.")]
        public string Body { get; set; }

        [Option(shortName: 'o', longName: "replacebody", Required = false, HelpText = "Set true to replace the release body, false to add to the release body. (default)", Default = false)]
        public string ReplaceBodyString { get; set; }

        public bool ReplaceBody => !string.IsNullOrWhiteSpace(ReplaceBodyString) && ReplaceBodyString.Equals("true", System.StringComparison.OrdinalIgnoreCase);

        [Option(shortName: 'f', longName: "files", Separator = ',', Required = false, HelpText = "A comma separated list of files whose content will be added after the release body text.")]
        public IEnumerable<string> Files { get; set; }

        [Option(shortName: 's', longName: "spacing", Required = false, HelpText = "The number of blank lines required between each addition to the release body. (default = 1)", Default = 1)]
        public int Spacing { get; set; }
    }
}
