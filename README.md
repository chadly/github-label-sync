# Github Label Sync

> Sync labels across all repositories in an organization

## Usage

[Generate an API access token](https://help.github.com/articles/creating-a-personal-access-token-for-the-command-line/) [for your account](https://github.com/settings/tokens) with repo permissions.

```bash
cd src
dotnet run [myaccesstoken]
```

or if you also want to delete extra labels:

```bash
cd src
dotnet run [myaccesstoken] remove-labels
```
