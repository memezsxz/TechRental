# TechRental System

TechRental is an equipment rental management system built using ASP.NET Core MVC for web interfaces and Windows Forms for desktop functionality. It is developed as part of the IT8118 Advanced Programming group project.

## 📁 Project Structure

- `WebApp/` — ASP.NET Core MVC web application
- `FormsApp/` — Windows Forms desktop client
- `Database/` — Entity Framework Core models and DB context
- `Identity/` — User authentication and role management
- `Helper/` — Utilities for PDF generation, notifications, and S3 upload
- `TechRental.sln` — Visual Studio solution file

## 📦 Features

### ✅ WebApp (ASP.NET Core MVC)
- User registration and login with role-based access (Customer, Admin, Manager)
- Rental request creation, status tracking, and calendar validation
- Rental record transactions and return processing
- File upload and PDF agreement generation
- Notification management and audit logging
- Fully documented Razor Views with sorting, filtering, pagination

### 🖥️ FormsApp (Windows Forms)
- Internal desktop interface for managers to handle rental operations
- Equipment management and offline record editing
- Sync with central SQL Server database

## 🧾 Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- Microsoft SQL Server
- Windows Forms
- AWS S3 (or S3-compatible storage) for file management
- PDFSharp or iTextSharp for PDF generation
- Bootstrap, jQuery, Flatpickr for frontend enhancements

## 🧑‍💻 Demo Accounts

| Username       | Password | Role     | Features Accessible                                  |
|----------------|----------|----------|------------------------------------------------------|
| Shima@gmail.com | As123!    | Customer | Create/view rental requests, view PDFs               |
| chloe.perry@example.com | As123!    | Customer | Create/view rental requests, view PDFs               |
| Manager1@gmail.com   | Pa$$word123   | Manager  | Approve requests, edit records, view all data        |
| Admin1@gmail.com     | Pa$$word123   | Admin    | Full access, user and system management              |

## 🗄️ Database Overview

The system uses an Entity-Relationship model consisting of:
- **Users**: Customers, Admins, and Managers
- **Equipment**: Available items with condition, feedback, and availability
- **RentalRequests**: Requests made by users with status and timeline
- **RentalRecords**: Approved rentals with pickup and return data
- **Documents**: Uploaded agreements stored via GUID for S3 integration
- **AuditLogs & Notifications**: Tracks system actions and user updates

## 📸 Screenshots & Documentation

Screenshots showing all key functionalities and validation scenarios are available in the `IT8118 Project Screenshots Document`.

## 📂 How to Run

### Prerequisites
- Visual Studio 2022+
- SQL Server (LocalDB or full version)
- .NET SDK 7.0+

### Steps
1. Clone or extract the project.
2. Open `TechRental.sln` in Visual Studio.
3. Configure the connection string in `appsettings.json`.
4. Run `Update-Database` via Package Manager Console.
5. Press F5 to launch the WebApp or run `FormsApp` manually.

## 📌 Notes

- Rental requests cannot overlap for the same equipment.
- Role-based UI and logic are enforced at controller and view levels.
- DocumentController supports PDF viewing and forced download modes.
- TempData messages are used for user feedback across most actions.

---

> This project was created for academic purposes and demonstrates a full-stack .NET solution combining web and desktop technologies with real-world features.
