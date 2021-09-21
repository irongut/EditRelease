# Edit Release Action

A GitHub Action for editing an existing release. Edit the Name, Draft status and Pre-release status of a release as well as adding text and the content of markdown files to the Body of a release.

## Inputs

#### `token`
**Required**

Authentication token, use either `GITHUB_TOKEN` or a Personal Access Token.

#### `id`
**Required**

The id of the release to edit, e.g. `github.event.release.id`.

#### `name`

New text for the name of the release.

#### `replacename`

Set `true` to replace the release name, `false` to add to the release name (default).

#### `draft`

Set `true` to change the release to a draft, `false` to publish the release. Omit if you do not want to change the draft status of the release.

#### `prerelease`

Set `true` to identify the release as a pre-release, `false` to identify the release as a full release. Omit if you do not want to change the status of the release.

#### `body`

New text for the body of the release.

#### `replacebody`

Set `true` to replace the release body, `false` to add to the release body. (default)

#### `files`

A comma separated list of files whose content will be added after the release body text.

#### `spacing`

The number of blank lines required between each addition to the release body. (default = 1)

## Outputs

Edit Release has no outputs other than console messages and the edited release.

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

Edit Release Action is available under the MIT license, see the [LICENSE](LICENSE) file for more info.

[issues]: https://github.com/irongut/EditRelease/issues
[new-issue]: https://github.com/irongut/EditRelease/issues/new
