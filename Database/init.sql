IF OBJECT_ID('Schedules', 'U') IS NOT NULL DROP TABLE Schedules;
IF OBJECT_ID('Routes', 'U') IS NOT NULL DROP TABLE Routes;
IF OBJECT_ID('Drivers', 'U') IS NOT NULL DROP TABLE Drivers;

CREATE TABLE Drivers (
    driver_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    email NVARCHAR(100) NOT NULL UNIQUE,
    password_hash NVARCHAR(200) NOT NULL
);

CREATE TABLE Routes (
    route_id INT IDENTITY(1,1) PRIMARY KEY,
    number INT NOT NULL,
    start_point NVARCHAR(100) NOT NULL,
    end_point NVARCHAR(100) NOT NULL
);

CREATE TABLE Schedules (
    schedule_id INT IDENTITY(1,1) PRIMARY KEY,
    driver_id INT NOT NULL,
    route_id INT NOT NULL,
    departure_time TIME NOT NULL,
    status NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_Schedules_Drivers FOREIGN KEY (driver_id) REFERENCES Drivers(driver_id),
    CONSTRAINT FK_Schedules_Routes FOREIGN KEY (route_id) REFERENCES Routes(route_id)
);

INSERT INTO Drivers (name, email, password_hash)
VALUES 
    ('Іван Петренко', 'ivan.petrenko@example.com', 'hash123'),
    ('Олег Савчук', 'oleg.savchuk@example.com', 'hash456'),
    ('Марія Коваленко', 'maria.kovalenko@example.com', 'hash789');

INSERT INTO Routes (number, start_point, end_point)
VALUES
    (12, 'Центр', 'Південний район'),
    (24, 'Залізничний вокзал', 'Жовтневий масив'),
    (7, 'Площа Шевченка', 'Сонячний квартал');

INSERT INTO Schedules (driver_id, route_id, departure_time, status)
VALUES
    (1, 1, '08:00', 'Scheduled'),
    (1, 2, '10:30', 'Scheduled'),
    (2, 1, '12:15', 'Completed'),
    (3, 3, '14:45', 'Scheduled');