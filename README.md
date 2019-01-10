# Ordigo
### Что это такое?
Ordigo - приложение для работы с заметками.

### На чем работает?
#### Сервер
1. ASP.NET Core 2.2
2. Entity Framework Core (InMemory provider)
3. Automapper
4. Newtonsoft Json

### Клиент
Клиент написал на .NET Framework 4.6.2 и использует:
1. WPF
2. RestSharp 
3. NLog
4. SimpleInjector
5. Newtonsoft Json
6. Fody

### Из чего состоит решение?
Решение состоит из трех проектов:
* Ordigo.API - библиотека для любого клиентского приложения, представляет собой интерфейс для работы с API через C#.
* Ordigo.Client - клиентское приложение.
* Ordigo.Server - серверное приложение.
