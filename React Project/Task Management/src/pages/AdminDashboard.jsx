import { useState, useEffect, useCallback } from "react";
import UserManagement from "../components/UserManagement";
import { useAuth } from "../context/AuthContext";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";

const API_BASE = "https://localhost:7299/api";

export default function AdminDashboard() {
  const { logout } = useAuth();
  const [chartData, setChartData] = useState([]);
  const [stats, setStats] = useState({ users: 0, todos: 0, completed: 0 });
  const [loading, setLoading] = useState(true);
  const [importLoading, setImportLoading] = useState(false);
  const [message, setMessage] = useState("");

  const fetchDashboardData = useCallback(async () => {
    try {
      setLoading(true);

      const [usersRes, todosRes] = await Promise.all([
        fetch(`${API_BASE}/AdminUsersApi`, { credentials: "include" }),
        fetch(`${API_BASE}/AdminTodosApi`, { credentials: "include" }),
      ]);

      if (!usersRes.ok || !todosRes.ok) {
        throw new Error("دریافت اطلاعات داشبورد با خطا مواجه شد. آیا ادمین هستید؟");
      }

      const [users, todos] = await Promise.all([
        usersRes.json(),
        todosRes.json(),
      ]);

      setStats({
        users: users.length,
        todos: todos.length,
        completed: todos.filter((t) => t.completed).length,
      });

      const data = users.map((u) => {
        const userTodos = todos.filter(
          (t) => String(t.userId) === String(u.id)
        );

        return {
          name: u.email.split("@")[0],
          "انجام‌شده": userTodos.filter((t) => t.completed).length,
          "در انتظار": userTodos.filter((t) => !t.completed).length,
        };
      });

      setChartData(data);
    } catch (error) {
      console.error("Dashboard Fetch Error:", error);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchDashboardData();
  }, [fetchDashboardData]);

  const importJsonPlaceholderTodos = async () => {
    try {
      setImportLoading(true);
      setMessage("");

      const response = await fetch(
        `${API_BASE}/TodosApi/import-jsonplaceholder`,
        {
          method: "POST",
          credentials: "include",
        }
      );

      const text = await response.text();

      let data;
      try {
        data = JSON.parse(text);
      } catch {
        throw new Error("پاسخ سرور JSON معتبر نیست.");
      }

      if (!response.ok) {
        throw new Error(data.message || "Import ناموفق بود.");
      }

      setMessage(data.message || "Import با موفقیت انجام شد.");
      await fetchDashboardData();
    } catch (error) {
      setMessage(error.message || "خطا در Import");
    } finally {
      setImportLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-50 p-6" dir="rtl">
      <div className="mb-6 flex flex-wrap items-center gap-3">
        <button
          type="button"
          onClick={importJsonPlaceholderTodos}
          disabled={importLoading}
          className="rounded-xl bg-indigo-600 px-4 py-2 text-white shadow-sm transition hover:bg-indigo-700 disabled:opacity-60"
        >
          {importLoading ? "در حال Import..." : "Import تسک‌های JSONPlaceholder"}
        </button>

        {message && (
          <p className="text-sm text-slate-700">{message}</p>
        )}
      </div>

      <div className="grid grid-cols-1 gap-4 mb-8 md:grid-cols-3">
        {[
          {
            label: "کاربران",
            value: stats.users,
            color: "bg-indigo-100 text-indigo-700",
          },
          {
            label: "کل تسک‌ها",
            value: stats.todos,
            color: "bg-blue-100 text-blue-700",
          },
          {
            label: "انجام‌شده",
            value: stats.completed,
            color: "bg-green-100 text-green-700",
          },
        ].map(({ label, value, color }) => (
          <div
            key={label}
            className={`rounded-2xl p-4 text-center shadow-sm ${color}`}
          >
            <p className="text-3xl font-bold">{value}</p>
            <p className="text-sm mt-1">{label}</p>
          </div>
        ))}
      </div>

      <div className="bg-white rounded-2xl shadow p-4">
        <h2 className="text-lg font-semibold text-gray-700 mb-4">
          توزیع تسک‌ها به ازای هر کاربر
        </h2>

        {loading ? (
          <div className="h-[300px] flex items-center justify-center text-sm text-gray-500">
            در حال بارگذاری نمودار...
          </div>
        ) : (
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={chartData}>
              <XAxis dataKey="name" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Bar dataKey="انجام‌شده" fill="#6366f1" radius={[4, 4, 0, 0]} />
              <Bar dataKey="در انتظار" fill="#e5e7eb" radius={[4, 4, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        )}
      </div>

      <UserManagement onUsersChanged={fetchDashboardData} />
    </div>
  );
}




