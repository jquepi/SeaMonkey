# Sea Monkey 

Sea Monkey is a simple but awesome tool to help populate an Octopus instane with a lot of test data.

To populate a bunch of test data, perform the following:

* Open the solution and build the solution.
* Launch your favourite console app/command prompt and navigate to the project's `bin/debug` folder.
* Execute `SeaMonkey.exe "https://myserver/" "API-ABCD123456890THISISAWESOME` at a Windows command prompt replacing the server and API keys with your own values.

NOTE: You'll likely need to update the `Octopus.Client` NuGet package dependency.