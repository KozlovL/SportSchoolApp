# 1. Сборка проекта (stage 1: build environment)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# Используем официальный образ .NET SDK для сборки (содержит компилятор и инструменты)

WORKDIR /app
# Устанавливаем рабочую директорию внутри контейнера

# Копируем решение (.sln) и проект (.csproj) в контейнер
COPY *.sln .
COPY *.csproj .
# Копируем только проектные файлы для восстановления зависимостей

RUN dotnet restore
# Восстанавливаем NuGet-пакеты для проекта

# Копируем остальной код в контейнер
COPY . .
# После восстановления пакетов можно скопировать оставшиеся файлы проекта

RUN dotnet publish -c Release -o out
# Компилируем проект в режиме Release и публикуем его в папку /out

# 2. Финальный образ (stage 2: runtime environment)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
# Используем более легкий образ только с ASP.NET Runtime (без SDK)

WORKDIR /app
# Устанавливаем рабочую директорию для запуска приложения

COPY --from=build /app/out .
# Копируем скомпилированное приложение из стадии сборки

EXPOSE 80
# Указываем, что приложение слушает порт 80 (можно переопределить при запуске)

ENTRYPOINT ["dotnet", "SportSchoolApp.dll"]
# Команда запуска приложения
