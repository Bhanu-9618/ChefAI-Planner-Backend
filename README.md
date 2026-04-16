# 🚀 ChefAI-Planner - Backend

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET Core](https://img.shields.io/badge/.NET_8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=JSON%20web%20tokens&logoColor=white)
![Google Gemini](https://img.shields.io/badge/Google_Gemini-8E75B2?style=for-the-badge&logo=googlebard&logoColor=white)

ChefAI-Planner is a robust, professional RESTful API built with **C#** and **ASP.NET Core 8**. It serves as the core engine for the ChefAI application, acting as a personal AI Nutritionist. By securely communicating with Google's advanced LLMs, it processes user-supplied ingredients to generate creative, healthy recipes instantly.

## 🌟 Key Features

* **🧠 AI Recipe Generation:** Seamless integration with the Google Gemini API to dynamically create structured recipes from a list of raw ingredients.
* **🔐 Secure Authentication:** Implements stateless JWT (JSON Web Tokens) bearer authentication for secure user registration and login workflows.
* **📚 Personal Recipe Book:** Endpoints designed to save, retrieve, and manage an individual user's generated recipe collection.
* **🔎 Smart Search Engine:** Filter and search through saved recipes quickly by title.
* **📑 High-Quality PDF Export:** Utilizes the QuestPDF engine to compile and download any generated recipe as a beautifully formatted, physical PDF document.
* **🛡️ Data Ownership & Security:** Strict authorization policies ensure users can only query, modify, and manage their own private data.

## 🛠️ Technology Stack

* **Language:** C#
* **Framework:** ASP.NET Core 8 Web API
* **ORM:** Entity Framework Core (EF Core)
* **Database:** PostgreSQL (Configurable for SQL Server)
* **AI Provider:** Google Gemini 2.5 Flash
* **Security:** JWT Authentication & BCrypt Password Hashing
* **Document Generation:** QuestPDF
