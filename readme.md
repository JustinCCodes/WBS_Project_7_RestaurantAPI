# Restaurant API

This is a learning project for building a RESTful API for a restaurant management system using ASP.NET Core. The project demonstrates CRUD operations, database integration, and clean architecture principles.

## Features

-   **Menu Management**: Create, update, delete, and view menu items.
-   **Order Management**: Create and view orders.
-   **Daily Reports**: Generate daily sales reports.
-   **Global Exception Handling**: Centralized error handling for API responses.
-   **Database Seeding**: Populate the database with initial data for testing.

## Technologies Used

-   **ASP.NET Core 10**
-   **Entity Framework Core** (SQLite)
-   **FluentValidation**
-   **Scalar** (API Documentation)
-   **Clean Architecture** (Domain, Infrastructure, Features)

## Project Structure

Feature-based Architecture (Vertical Slice):

-   `Domain/` - Core business models (MenuItem, Order, etc.)
-   `Data/` - Database context, migrations, and seeding
-   `Features/` - Application features grouped by domain (Menu, Orders, Reports)
-   `Infrastructure/` - Cross-cutting concerns (e.g., exception handling)
-   `appsettings.json` - Configuration file

## Getting Started

1. **Clone the repository**

    ```bash
    git clone [https://github.com/JustinCCodes/WBS_Project_7_RestaurantAPI.git](https://github.com/JustinCCodes/WBS_Project_7_RestaurantAPI.git)
    cd WBS_Project_7_RestaurantAPI/src/Restaurant.Api
    ```

2. **Restore dependencies**

    ```bash
    dotnet restore
    ```

3. **Apply database migrations**

    ```bash
    dotnet ef database update
    ```

4. **Run the API**

    ```bash
    dotnet run
    ```

5. **API Documentation**

    This project uses the **Microsoft.AspNetCore.OpenApi** generator and **Scalar** for interactive documentation.

    - **OpenAPI Spec**: Automatically generated at `/openapi/v1.json`.
    - **Interactive UI**: Access the Scalar UI to explore and test endpoints.

    **How to access:**
    1. Start the API using `dotnet run`.
    2. Check the console output for the localhost port (e.g., `http://localhost:5000`).
    3. Navigate to the Scalar endpoint in your browser:

       ```
       http://localhost:5000/scalar/v1
       ```

## Example Endpoints

-   `GET /menu` - List all menu items
-   `POST /menu` - Create a new menu item
-   `PUT /menu/{id}` - Update a menu item
-   `DELETE /menu/{id}` - Delete a menu item
-   `GET /orders` - List all orders
-   `POST /orders` - Create a new order
-   `GET /reports/daily` - Get daily sales report

## Learning Goals

-   Practice building REST APIs with ASP.NET Core 10
-   Understand Entity Framework Core and migrations
-   Implement clean architecture and separation of concerns
-   Learn about validation, error handling, and modern API documentation with Scalar

## License

This project is for educational purposes only.
