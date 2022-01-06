# Edit Release Action

A GitHub Action for editing an existing release. Edit the Name, Draft status and Pre-release status of a release as well as adding text and the content of markdown files to the Body of a release.

As a Docker based action Edit Release requires a Linux runner, see [Types of Action](https://docs.github.com/en/actions/creating-actions/about-custom-actions#types-of-actions).

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

```yaml
name: Edit Release
uses: irongut/EditRelease@v1.1.0
with:
  token: ${{ secrets.GITHUB_TOKEN }}
  id: ${{ github.event.release.id }}
  name: "Beta"
  prerelease: true
  body: "This is a pre-release version for testing purposes."
  files: "changelog.md,testcoverage.md"
```

### Workflow Example

This workflow will run when you publish a release. It builds and tests a .Net 5 Nuget library before deploying it to GitHub Packages and adding a test coverage report to the release.

```yaml
name: Build + Deploy

on:
  release:
    types: [published]
    branches: [master]

env:
  GITHUB_PACKAGE_URL: 'https://nuget.pkg.github.com/${{ github.repository_owner }}/index.json'

jobs:
  build:
    runs-on: ubuntu-latest
    name: Release Build
    steps:
    - name: Checkout
      uses: actions/checkout@v2
      with:
        fetch-depth: 0

    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 5.0.x

    - name: Restore Dependencies
      run: dotnet restore src/Example.sln

    - name: Build
      run: dotnet build src/Example.sln --configuration Release --no-restore

    - name: Test
      run: dotnet test src/Example.sln --configuration Release --no-build --verbosity normal --collect:"XPlat Code Coverage" --results-directory ./coverage

    - name: Copy Test Details
      run: cp coverage/**/coverage.cobertura.xml coverage/coverage.cobertura.xml

    - name: Create Test Report
      uses: irongut/CodeCoverageSummary@v1.2.0
      with:
        filename: coverage/coverage.cobertura.xml
        badge: true
        format: 'markdown'
        output: 'both'

    - name: Upload Nuget Artifact
      uses: actions/upload-artifact@v2.2.4
      with:
        name: release-nuget
        path: src/Example/bin/Release/Example.Library*.nupkg

    - name: Upload Test Report Artifact
      uses: actions/upload-artifact@v2.2.4
      with:
        name: release-nuget
        path: code-coverage-results.md

  deploy:
    name: Deploy to GitHub Packages
    needs: [build]
    runs-on: ubuntu-latest
    steps:
    - name: Download Artifacts
      uses: actions/download-artifact@v2
      with:
        name: release-nuget

    - name: Setup Nuget
      uses: NuGet/setup-nuget@v1.0.5
      with:
        nuget-version: latest

    - name: Add GitHub package source
      run: nuget sources Add -Name GitHub -Source ${{env.GITHUB_PACKAGE_URL}} -UserName ${{ github.repository_owner }} -Password ${{ secrets.GITHUB_TOKEN }}

    - name: Push to GitHub Packages
      run: nuget push **/*.nupkg -source GitHub -SkipDuplicate

    - name: Add Test Report to Release
      uses: irongut/EditRelease@v1.1.0
      with:
        token: ${{ secrets.GITHUB_TOKEN }}
        id: ${{ github.event.release.id }}
        body: "Released to GitHub Packages."
        files: "code-coverage-results.md"
```

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
