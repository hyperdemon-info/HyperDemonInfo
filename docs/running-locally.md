# Running Locally

Running the website locally is done by running the `HyperDemonInfo.Server` project. This is the ASP.NET Core back-end. The server project is responsible for serving the Blazor WebAssembly app to the browser.

## Prerequisites

1. Install the .NET SDK for the version specified in the `TargetFramework` property in the `Directory.Build.props` file. (At the time of writing, this is .NET 6.0.)
2. Download [tailwindcss](https://github.com/tailwindlabs/tailwindcss/releases) for your operating system.
3. Place the downloaded executable in the `src` folder and rename it to `tw.exe`. The file extension is required on Windows, but this might not work on Mac or Linux. You can exclude `.exe` from the file name and change the file name in the `Exec` task in the `tailwind build` `Target` in the `HyperDemonInfo.Client.csproj` file.

## How To Run

Navigate to `src/HyperDemonInfo.Server` and execute `dotnet run` from the terminal, or use an IDE of your choice to run the server.
