import {
  Link,
  NavLink,
  Outlet,
  useNavigate,
} from "react-router-dom";
import { useAuth } from "../context/AuthContext";

const Layout = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const displayName =
    user?.username ||
    user?.userName ||
    user?.email ||
    "کاربر";

  const isAdmin = user?.isAdmin === true;

  const handleLogout = async () => {
    await logout();
    navigate("/login", { replace: true });
  };

  const getNavLinkClass = ({ isActive }) =>
    [
      "text-sm font-medium transition-colors",
      isActive
        ? "text-indigo-600"
        : "text-slate-600 hover:text-indigo-600",
    ].join(" ");

  return (
    <div
      dir="rtl"
      className="min-h-screen flex flex-col bg-slate-50 text-slate-800"
    >
      {/* هدر بالای صفحه */}
      <header className="sticky top-0 z-50 border-b border-slate-200 bg-white">
        <div className="mx-auto flex max-w-6xl flex-wrap items-center justify-between gap-3 px-4 py-4">

          {user && (
            <div className="text-left">
              <p className="text-sm font-medium text-slate-600">
                خوش آمدید،{" "}
                <span className="font-semibold text-indigo-600">
                  {displayName}
                </span>
              </p>
            </div>
          )}

          <div className="flex items-center gap-4">

            <Link
              to={isAdmin ? "/admin" : "/dashboard"}
              className="text-xl font-bold text-indigo-600 transition-colors hover:text-indigo-700"
            >
              TaskApp
            </Link>
          </div>

          {/* منوی مسیرها */}
          <nav
            aria-label="منوی اصلی"
            className="order-3 flex w-full items-center gap-4 border-t border-slate-100 pt-3 sm:order-2 sm:w-auto sm:border-t-0 sm:pt-0"
          >
          <button
            type="button"
            onClick={handleLogout}
            className="rounded-lg bg-slate-100 px-4 py-2 text-sm font-medium text-slate-700 transition-colors hover:bg-red-50 hover:text-red-600 focus:outline-none focus:ring-2 focus:ring-red-400 focus:ring-offset-2"
          >
            خروج
          </button>

            {isAdmin && (
              <>
                <NavLink to="/admin" end className={getNavLinkClass}>
                  مدیریت
                </NavLink>

                <NavLink
                  to="/admin/email-preview"
                  className={getNavLinkClass}
                >
                  پیش‌نمایش ایمیل
                </NavLink>
              </>
            )}
          </nav>
        </div>
      </header>

      {/* محتوای صفحه */}
      <main className="mx-auto w-full max-w-6xl flex-grow px-4 py-8">
        <Outlet />
      </main>

      {/* فوتر */}
      <footer className="border-t border-slate-200 bg-white py-6 text-center">
        <p className="text-sm text-slate-500">
          © {new Date().getFullYear()} تمامی حقوق محفوظ است | ساخته شده توسط
          سید علی امام
        </p>
      </footer>
    </div>
  );
};

export default Layout;







