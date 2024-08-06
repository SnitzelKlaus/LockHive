### Create Api Generator that will generate swagger.json and api client


Install tool manifest file in your solution, run the following command in root. This needs to be done only one time
 ```
dotnet new tool-manifest
```

# Generating swagger files
Run the Password.Api.Service solution. Click on "/swagger/v1/swagger.json" in the UI. This will open a new browser with the swagger.json. 
Copy the json and insert the json into "..\PasswordManager.Password.Client\swagger.json"

When you have done that go to next step generating client

# Generating client
From solution root
```
dotnet apigenerator generate -b PasswordManager -n Password -s src\Password\PasswordManager.Password.Client\swagger.json -o src\Password\PasswordManager.Password.Client
```
