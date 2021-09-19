using CommandLine;
using Octokit;
using System;
using System.Collections.Generic;
using System.Reflection;
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
            string repo = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
            if (string.IsNullOrWhiteSpace(repo) || !repo.Contains("/"))
            {
                Console.WriteLine("Error: GITHUB_REPOSITORY environment variable not found.");
                return -2;
            }

#if DEBUG
            options.Token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
#endif
            if (string.IsNullOrWhiteSpace(options.Token))
            {
                Console.WriteLine("Error: Authentication token not found, use either GITHUB_TOKEN or a Personal Access Token.");
                return -2;
            }

            Console.WriteLine($"{Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version} started...");
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
                updateRelease.Body = $"{release.Body}{Environment.NewLine}{Environment.NewLine}Test";

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
    }
}
