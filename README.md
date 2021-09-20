# Edit Release Action

A GitHub Action for editing an existing release. EditRelease can change the Name, Draft status and Pre-release status of a release as well as adding text and the content of markdown files to the Body of a release.

## Inputs

#### `token`
**Required**

Authentication token, use either `GITHUB_TOKEN` or a Personal Access Token.

#### `id`
**Required**

The id of the release, e.g. `github.event.release.id`.

#### `name`

The name of the release.

#### `replacename`

Include to replace the release name instead of adding to it.

#### `draft`

Set `true` makes the release a draft, `false` publishes the release. Omit if you don't want to change the Draft status of the release.

#### `prerelease`

Set `true` to identify the release as a pre-release, `false` to identify the release as a full release. Omit if you don't want to change the Pre-release status of the release.

#### `body`

Text that will be added to the body of the release.

#### `replacebody`

Include to replace the content of the release body instead of adding to it.

#### `files`

A comma separated list of files whose content will be added after `body`.

#### `spacing`

The number of blank lines required between each addition to the release body. Default = 1.

## Outputs

EditRelease has no outputs other than console messages and the edited release.

## Usage



## Contributing

### Report Bugs

Please make sure the bug is not already reported by searching existing [issues].

If you're unable to find an existing issue addressing the problem please [open a new one][new-issue]. Be sure to include a title and clear description, as much relevant information as possible, a workflow sample and any logs demonstrating the problem.

### Suggest an Enhancement

Please [open a new issue][new-issue].

### Submit a Pull Request

Discuss your idea first, so that your changes have a good chance of being merged in.

Submit your pull request against the `master` branch.

Pull requests that include documentation and relevant updates to README.md will be merged faster.

## License

EditRelease is available under the MIT license, see the [LICENSE](LICENSE) file for more info.

[issues]: https://github.com/irongut/EditRelease/issues
[new-issue]: https://github.com/irongut/EditRelease/issues/new
