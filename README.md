# ASP.NET Core Validation #69530

This sample validates asynchronous Blazor form validation behavior described in
[dotnet/aspnetcore#69530](https://github.com/dotnet/aspnetcore/issues/69530).

It demonstrates the difference between:

- A rejected username (`Taken`), which produces an ordinary validation message.
- A failed availability lookup (`Throw exception`), which produces validation
  fault state and generic retry feedback.
- A successful lookup (`Available`), which clears previous fault state after a
  new validation pass.

## Requirements

- .NET 11 RC1 SDK or later

## Render modes

Test the same scenarios in both pages:

- Interactive Server: `/validation-server` (also the default `/` route)
- Interactive WebAssembly: `/validation-webassembly`

Wait for the WebAssembly page to finish loading before testing it.
