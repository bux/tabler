using System;
using System.Linq;
using Octokit;
using tabler.Logic.Classes;

namespace tabler.Logic.Helper
{
    public class GitHubVersionHelper
    {
        public ReleaseVersion CheckForNewVersion(string productVersion)
        {
            var github = new GitHubClient(new ProductHeaderValue("tabler"));
            var releases = github.Repository.Release.GetAll("bux", "tabler").Result;

            var newerRelease = releases.Where(x => x.PublishedAt.HasValue && new Version(x.TagName.Replace("v", "")) > new Version(productVersion)).OrderByDescending(x => x.PublishedAt).FirstOrDefault();

            if (newerRelease == null)
            {
                return null;
            }

            return new ReleaseVersion
            {
                Version = newerRelease.Name,
                HtmlUrl = newerRelease.HtmlUrl
            };
        }
    }
}
