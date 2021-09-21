using CommandLine;
using Octokit;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EditRelease
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            return await Parser.Default.ParseArguments<Options>(args)
                                       .MapResult(o => RunActionAsync(o),
                                                  _ => Task.FromResult(-1)) // invalid arguments
                                       .ConfigureAwait(false);
        }

        private static async Task<int> RunActionAsync(Options options)
        {
            Console.WriteLine($"{Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version} started...");

            string repo = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
            if (string.IsNullOrWhiteSpace(repo) || !repo.Contains("/"))
            {
                Console.WriteLine("Error: GITHUB_REPOSITORY environment variable not found.");
                return -2;
            }

#if DEBUG
            if (string.IsNullOrWhiteSpace(options.Token))
                options.Token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
#endif

            if (string.IsNullOrWhiteSpace(options.Token))
            {
                Console.WriteLine("Error: Authentication token not found, use either GITHUB_TOKEN or a Personal Access Token.");
                return -2;
            }

            Console.WriteLine($"Repository: {repo}, Release Id: {options.ReleaseId}");

            try
            {
                string owner = repo.Split("/")[0];
                string repoName = repo.Split("/")[1];

                GitHubClient client = new(new ProductHeaderValue("taranis-edit-release-action"))
                {
                    Credentials = new Credentials(options.Token)
                };

                Release release = await client.Repository.Release.Get(owner, repoName, options.ReleaseId).ConfigureAwait(false);

                if (release == null)
                {
                    Console.WriteLine($"Error: Unable to find Release Id {options.ReleaseId} in {repo}.");
                    return -2;
                }

                Console.WriteLine($"Release Found - Id: {release.Id}, Tag: {release.TagName}, Author: {release.Author.Login}.");

                ReleaseUpdate updateRelease = release.ToUpdate();

                if (options.Draft.HasValue)
                    updateRelease.Draft = options.Draft.Value;

                if (options.Prerelease.HasValue)
                    updateRelease.Prerelease = options.Prerelease.Value;

                if (!string.IsNullOrWhiteSpace(options.Name))
                    updateRelease.Name = options.ReplaceName ? options.Name : $"{release.Name} {options.Name}";

                if (options.ReplaceBody)
                    updateRelease.Body = string.Empty;

                if (!string.IsNullOrWhiteSpace(options.Body))
                {
                    updateRelease.Body = string.IsNullOrWhiteSpace(updateRelease.Body)
                        ? $"{options.Body}{Environment.NewLine}"
                        : $"{updateRelease.Body}{BodySpacing(options.Spacing)}{options.Body}{Environment.NewLine}";
                }

                if (options.Files?.Any() == true && !string.IsNullOrWhiteSpace(options.Files.First()))
                    updateRelease.Body = AddFilesToBody(updateRelease.Body, options);

                var result = await client.Repository.Release.Edit(owner, repoName, options.ReleaseId, updateRelease).ConfigureAwait(false);
                if (result != null)
                {
                    Console.WriteLine($"Release Updated - Id: {release.Id}, Tag: {release.TagName}, Author: {release.Author.Login}.");
                }
                else
                {
                    Console.WriteLine($"Error: Unable to update Release Id: {release.Id}.");
                    return -2;
                }

                return 0; // success
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType()} - {ex.Message}");
                return -3; // unhandled error
            }
        }

        private static string BodySpacing(int spacing)
        {
            string value = string.Empty;
            while (spacing > 0)
            {
                value = $"{value}{Environment.NewLine}";
                spacing--;
            }
            return value;
        }

        private static string AddFilesToBody(string body, Options options)
        {
            string spacing = BodySpacing(options.Spacing);
            StringBuilder bodyText = new(body);
            foreach (string filename in options.Files)
            {
                if (bodyText.Length > 0)
                    bodyText.Append(spacing);
                bodyText.Append(File.ReadAllText(filename));
            }
            return bodyText.ToString();
        }
    }
}
