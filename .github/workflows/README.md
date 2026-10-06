# Mini Task Tracker API

Une petite API REST pour gérer des tâches, construite en ASP.NET Core 8 (Minimal API) + EF Core + SQLite.

## Endpoints

| Méthode | Route              | Description              |
|---------|---------------------|---------------------------|
| GET     | /api/tasks           | Liste toutes les tâches  |
| GET     | /api/tasks/{id}       | Récupère une tâche        |
| POST    | /api/tasks            | Crée une tâche            |
| PUT     | /api/tasks/{id}       | Met à jour une tâche      |
| DELETE  | /api/tasks/{id}       | Supprime une tâche        |

## Lancer en local

```bash
dotnet restore
dotnet run
```

Swagger disponible sur `http://localhost:5000/swagger` (ou le port affiché au démarrage).

## Lancer avec Docker

```bash
docker compose up --build
```

## Exemple de requête

```bash
curl -X POST http://localhost:8080/api/tasks \
  -H "Content-Type: application/json" \
  -d '{"title":"Réviser le cours de réseaux","dueDate":"2026-10-01"}'
```