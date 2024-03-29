# How to contribute to NordigenDotNet

### Did you find a bug?

1. **Ensure the bug was not already reported** by searching on GitHub under
   [Issues](https://github.com/VMelnalksnis/NordigenDotNet/issues?q=is%3Aissue+label%3Abug).

2. If you're unable to find an issue addressing the problem,
   [open a new one](https://github.com/VMelnalksnis/NordigenDotNet/issues/new?assignees=VMelnalksnis&labels=bug&template=bug_report.md).
   Be sure to include a **title and clear description**,
   as much relevant information as possible,
   and, if possible, a **code sample** or an **executable test case**
   demonstrating the expected behavior that is not occurring.

### Do you want to request a new feature or change?

1. **Ensure the feature is not already in progress or rejected** by searching on GitHub under
   [Issues](https://github.com/VMelnalksnis/NordigenDotNet/issues?q=is%3Aissue+label%3Aenhancement).
   If it exists, make sure to add a +1 reaction to show your interest in the feature.

2. If you're unable to find an issue regarding this feature,
   [open a new one](https://github.com/VMelnalksnis/NordigenDotNet/issues/new?assignees=VMelnalksnis&labels=enhancement&template=feature_request.md).

### If opening a PR to resolve an issue

- When testing, integration tests are recommended where possible.
- Due to inconsistent data returned by each bank, when adding data with additional fields which is not included in the sandbox accounts, please add new tests with mocked data for these fields to cover them, please see an example of this [here](./tests/VMelnalksnis.NordigenDotNet.Tests/Accounts/AccountClientTests.cs).
