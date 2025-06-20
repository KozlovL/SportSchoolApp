# SportSchoolApp

## Описание

**SportSchoolApp** — это веб-приложение для управления спортивной школой, разработанное на ASP.NET Core и использующее Entity Framework Core и SQLite. Система позволяет администраторам управлять учениками, тренерами, расписаниями и ролями пользователей.


## Авторы
[**Козлов Леонид**](https://github.com/KozlovL)
[**Ожогина Татьяна**](https://github.com/tatyana724)
[**Шпигальских Анастасия**](https://github.com/Anastasia080)


## Возможности

- Регистрация и авторизация пользователей с ролями (Администратор, Тренер, Ученик)
- Планирование занятий и расписаний
- Управление списками тренеров и учеников
- Ролевой доступ к функциям приложения
- Хранение данных в SQLite (по умолчанию)


## Технологии

- **Backend:** ASP.NET Core 9.0
- **ORM:** Entity Framework Core
- **Database:** SQLite
- **Authentication:** ASP.NET Identity
- **Containerization:** Docker


## Локальное развертывание с Docker

1. **Клонируйте репозиторий:**
```bash
git clone https://github.com/KozlovL/SportSchoolApp.git
```
```bash
cd SportSchoolApp
```


2. **Скачайте образ из Docker Hub:**
```bash
docker pull kozlovl/sportschoolapp:latest
```


3. **Запустите проект в Docker-контейнере:**
```bash
docker run -d -p 8080:8080 kozlovl/sportschoolapp:latest
```


Проект доступен по адресу:  
**http://localhost:8080**
