# ChefAI-Planner - Backend 🚀

ChefAI-Planner is a professional RESTful API that acts as a personal AI Nutritionist. Users can input available ingredients and get creative, healthy recipes instantly using Google Gemini AI.

## 🌟 Key Features

* **AI Recipe Generation:** Uses Google Gemini API to create recipes from ingredients.
* **Secure Auth:** JWT-based login and registration system.
* **Recipe Book:** Save, view, and manage your personal recipe collection.
* **Smart Search:** Search through saved recipes by title.
* **PDF Export:** Download any recipe as a professionally formatted PDF.
* **Ownership Security:** Users can only access and manage their own data.

## 🛠️ Tech Stack

* **Backend:** ASP.NET Core 8 (Web API)
* **Database:** PostgreSQL / SQL Server (EF Core)
* **AI:** Google Gemini 2.5 Flash
* **Security:** JWT Bearer Authentication
* **PDF Engine:** QuestPDF

## 🔌 API Endpoints

### Authentication
- `POST /api/auth/register` - Create a new account
- `POST /api/auth/login` - Get access token

### Recipe Management
- `POST /api/recipe/generate` - Generate AI recipe (Requires Auth)
- `POST /api/recipe/save` - Save generated recipe to DB
- `GET /api/recipe/my-recipes` - List all saved recipes
- `GET /api/recipe/{id}` - Get full details of a recipe
- `GET /api/recipe/search?title=...` - Search recipes by name
- `GET /api/recipe/download/{id}` - Export recipe as PDF
---
Developed as a Full-Stack Trainee Project.
