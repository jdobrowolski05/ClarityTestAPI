## ClarityEmailer
Simple class project that contains the function to send emails

## ClarityConsole
Configurations are in app.config for SMTP server and log path. If using a relative log path, it is relative to the .exe it generates (in my case when running debug ./bin/Debug/net8.0/).

Run the console and it will just loop asking for a To, From, Subject, and Body for an email, then attempt to send and immediately ask again.

## ClarityTestAPI
Configurations are in appsettings.json for SMTP server and log path. If using a relative log path, it is relative to the project directory.

Starts an API with a SendEmail endpoint at https://localhost:7018. The expected body schema is shown in the swagger docs generated for the Email.cs class. Success is currently just returning "true" since we aren't really waiting to see what happens in the async method, though if it was more robust it could handle things like validation.

## ClarityWebApp
Just a simple web app at https://localhost:7151/. Contains a form that will send an ajax request to the aforementioned API and alert the user upon response from the server. The ClarityTestAPI will also need to be running, through something like the multiple startup projects property configuration for the solution
