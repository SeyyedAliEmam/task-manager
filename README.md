# سیستم مدیریت اعضا و تسک‌ها | Task Manager

پروژه فول‌استک مدیریت اعضا و تسک‌ها شامل بک‌اند **ASP.NET Core** و فرانت‌اند **React + Vite** که به‌صورت مستقیم به هم متصل هستند.

فرانت‌اند با `fetch` به API دات‌نت روی `https://localhost:7299/api` وصل می‌شود و احراز هویت با کوکی و `credentials: include` انجام می‌شود.

## ویژگی‌ها

**بک‌اند (backend/MemberShip):**

* معماری تمیز در چهار لایه: Domain ،Application ،Infrastructure ،Web
* موجودیت‌های User و Todos با BaseEntity مشترک
* الگوی Generic Repository و Unit of Work
* انتیتی‌فریم‌ورک Core با AppContext و Migration
* کنترلرهای MVC برای پنل تحت وب + کنترلرهای API برای ری‌اکت:
* `AccountApiController` : ثبت‌نام، ورود، خروج، کاربر جاری
* `TodosApiController` : عملیات CRUD تسک‌ها
* `AdminUsersApiController` و `AdminTodosApiController` : مدیریت ادمین
* ارسال ایمیل و ورود با کد یک‌بارمصرف با IEmailSender
* رابط فارسی و راست‌چین با فایل `rtl.css`

**فرانت‌اند (frontend):**

* ری‌اکت ۱۹ با Vite ،React Router ۷ و Tailwind CSS ۴
* مدیریت احراز هویت سراسری با `context/AuthContext.jsx`
* اتصال مستقیم لاگین و ثبت‌نام به `api/AccountApi`
* داشبورد کاربر در `pages/UserDashboard.jsx` با اتصال به `api/TodosApi`
* داشبورد ادمین در `pages/AdminDashboard.jsx` با نمودار
* نمودار با chart.js و recharts ،آیکون با lucide-react
* مسیر محافظت‌شده با `components/ProtectedRoute.jsx`

## تکنولوژی‌ها

|بخش|تکنولوژی|
|-|-|
|بک‌اند|ASP.NET Core ،سی‌شارپ ،Entity Framework Core ،SQL Server|
|فرانت‌اند|React 19 ،Vite ،Tailwind 4 ،React Router 7|
|نمودار|chart.js ،react-chartjs-2 ،recharts|
|احراز هویت|Cookie Authentication + Fetch credentials:include|

## ساختار پروژه

```
task-manager/
├── backend/
│   └── MemberShip/
│       ├── MemberShip.sln
│       ├── MemberShip.Domain/ (User.cs, Todos.cs, BaseEntity.cs)
│       ├── MemberShip.Application/ (IRepositories, IUOF, IEmailSender)
│       ├── MemberShip.Infrastructure/ (AppContext.cs, Repositories, Migrations)
│       └── MemberShip.Web/ (Controllers, Views, wwwroot, Program.cs)
├── frontend/
│   ├── package.json
│   ├── vite.config.js
│   ├── src/
│   │   ├── context/AuthContext.jsx
│   │   ├── pages/UserDashboard.jsx
│   │   ├── pages/AdminDashboard.jsx
│   │   ├── pages/Login.jsx
│   │   ├── pages/RegisterPage.jsx
│   │   └── components/
│   └── public/
├── .gitignore
└── README.md
```

## نحوه اتصال فرانت به بک‌اند

```js
// frontend/src/context/AuthContext.jsx
const BASE\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\_URL = "https://localhost:7299/api/AccountApi";
fetch(`${BASE\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\_URL}/login`, { method: "POST", credentials: "include" })

// frontend/src/pages/UserDashboard.jsx
const API\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\_URL = "https://localhost:7299/api/TodosApi";
fetch(API\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\_URL, { credentials: "include" })
```

بک‌اند باید روی `https://localhost:7299` اجرا شود و CORS و کوکی برای همین آدرس تنظیم باشد.

## راه‌اندازی

### پیش‌نیازها

* دات‌نت SDK نسخه ۷ یا ۸
* نودجی‌اس ۱۸ به بالا و npm
* SQL Server یا LocalDB

### ۱. اجرای بک‌اند

```bash
cd backend/MemberShip
dotnet restore
dotnet ef database update --project MemberShip.Infrastructure --startup-project MemberShip.Web
dotnet run --project MemberShip.Web
```

آدرس بک‌اند: `https://localhost:7299`

کانکشن‌استرینگ نمونه در `MemberShip.Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MemberShipDb;Trusted\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\_Connection=True;TrustServerCertificate=True;"
  }
}
```

مقادیر واقعی را فقط در `appsettings.Development.json` بگذارید که در گیت نادیده گرفته می‌شود.

### ۲. اجرای فرانت‌اند

```bash
cd frontend
npm install
npm run dev
```

اپ روی `http://localhost:5173` اجرا می‌شود و خودش به بک‌اند وصل می‌شود.

## سازنده

**SeyyedAliEmam**

لینک ریپو: https://github.com/SeyyedAliEmam/task-manager

## 

