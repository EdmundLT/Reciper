# Reciper
Reciper is a simple recipe management API developed using .NET 6 and EF Core. It allows users to create, view, update, and delete recipes along with their ingredients and instructions.

## Installation
To install Reciper, you'll need to have .NET 5 SDK and a compatible database management system installed on your system. You can follow these steps to install Reciper:

Clone the repository: git clone https://github.com/<your-github-username>/reciper.git
Navigate to the project folder: cd reciper
Restore the NuGet packages: dotnet restore
Update the database schema: dotnet ef database update
Run the application: dotnet run
## Usage
Reciper provides a RESTful API that you can use to manage your recipes. The API supports the following endpoints:

- GET /recipes: Get a list of all recipes.
- GET /recipes/{id}: Get a specific recipe by ID.
- POST /recipes: Create a new recipe.
- PUT /recipes/{id}: Update an existing recipe by ID.
- DELETE /recipes/{id}: Delete a recipe by ID.
- POST /recipes/{id}/ingredients: Add one or more ingredients to a recipe.
- POST /recipes/{id}/instructions: Add one or more instructions to a recipe.
