Parse Client Library
---

1. Rename the app.config.sample to app.config
2. Populate the appId and masterKey from Parse

If you wish to run the test project apply the same steps to the sample app.config file in the tests project.

Compile the project and include it as a reference in your project.  This project relies on Json.NET to convert the data in and out from Parse.

Example usage - creating a user:

    var client = new ParseClient.Client();
	
	// This newUserObjectId variable needs to be stored somewhere to send future updates
	var newUserObjectId = client.PostUserData("some encrypted data");

Example usage - updating a user:

    var client = new ParseClient.Client();
	
	client.PutUserData(existingUserObjectId, "some encrypted data");

