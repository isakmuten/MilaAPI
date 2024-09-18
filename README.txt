INSTRUCTIONS FOR RUNNING THE APP

	1. Open cmd
	2. run: git clone https://github.com/isakmuten/MilaAPI.git
	3. Install Dependencies, in cmd run: cd MilaAPI -> dotnet restore
	4. run: dotnet ef database update
	5. run the application

If you get errors regarding EF Tools:
	1. open cmd and navigate to C:\
	2. run: dotnet tool uninstall --global dotnet-ef
	3. run: dotnet tool install --global dotnet-ef --version 8.0.8
	4. navigate to MilaAPI (C:\<your-user>\MilaAPI\)
	5. run: dotnet ef database update

When running:
	1. Use /api/Users/Login
	2. Fill in the request with credentials below: 
		{
		  "email": "isak@muten.com",
		  "password": "isakisak123"
		}
	3. Locate the token in the response body and copy the access token inside the quotiations (" ")
		Example. eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZW1haWwiOiJpc2FrQG11dGVuLmNvbSIsIm5iZiI6MTcyNjY
		3MzA3OSwiZXhwIjoxNzI2Njc2Njc5LCJpYXQiOjE3MjY2NzMwNzksImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcxOTkiLCJhdWQiOiJodHR
		wczovL2xvY2FsaG9zdDo3MTk5In0.PNrxbd6_nddrikid3_UKPEPJNyo5iOQKMFSCGlJX70M 

	4. Go to the top of the viewport and locate the green "Authorize" button.
	5. In he value field type: Bearer
		followed by the copied token and login.
			Example: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZW1haWwiOiJpc2FrQG11dGVuLmNvbSIsIm5i
			ZiI6MTcyNjY3MzA3OSwiZXhwIjoxNzI2Njc2Njc5LCJpYXQiOjE3MjY2NzMwNzksImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcxOTkiLCJhd
			WQiOiJodHRwczovL2xvY2FsaG9zdDo3MTk5In0.PNrxbd6_nddrikid3_UKPEPJNyo5iOQKMFSCGlJX70M
	6. Now you're logged in and able to test alla the features available. 