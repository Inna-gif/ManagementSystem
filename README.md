# # ManagementSystem

## Опис

Це навчальний проєкт для створення системи керування з базою даних, що містить три таблиці: **Users**, **Items** і **Actions**. До складу входять два застосунки:

* **ASP.NET Core MVC** – адмін-панель для керування даними (CRUD-операції);
* **.NET MAUI** – клієнтський застосунок для перегляду даних і виконання дій.

## Встановлення

1. Клонувати репозиторій:

   ```bash
   git clone https://github.com/твій-логін/ManagementSystem.git
   ```
2. Відкрити рішення у **Visual Studio 2022**.
3. Переконатися, що встановлені робочі навантаження:

   * ASP.NET and web development;
   * .NET MAUI.
4. Запустити серверну частину (**ServerApp**) клавішею `F5`.
5. Запустити клієнтську частину (**ClientApp**) на Android-емуляторі.

## Структура проєкту

```
ManagementSystem/
├── Server/                # Серверна частина (ASP.NET Core MVC)
│   └── ServerApp/         # Проєкт адмін-панелі
│       ├── Controllers/   # Контролери MVC
│       ├── Models/        # Моделі (Users, Items, Actions)
│       ├── Views/         # Представлення (HTML, Razor)
│       └── appsettings.json
│
├── Client/                # Клієнтська частина (.NET MAUI)
│   └── ClientApp/         # Проєкт MAUI
│       ├── Views/         # Сторінки застосунку
│       ├── ViewModels/    # Логіка MVVM
│       └── Models/        # Класи даних
│
├── README.md              # Опис проєкту
└── .gitignore             # Файл ігнорування для Git
```

