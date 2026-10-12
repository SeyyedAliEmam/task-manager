import { useState, useEffect } from "react";
import { PieChart, Pie, Cell, Tooltip, Legend } from "recharts";

const COLORS = ["#6366f1", "#e5e7eb"];
const API_URL = "https://localhost:7299/api/TodosApi";

export default function UserDashboard() {
  const [todos, setTodos] = useState([]);
  const [newTitle, setNewTitle] = useState("");
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    fetchTodos();
  }, []);

  const fetchTodos = async () => {
    try {
      setLoading(true);

      const res = await fetch(API_URL, {
        method: "GET",
        credentials: "include",
      });

      if (!res.ok) {
        throw new Error("خطا در دریافت تسک‌ها");
      }

      const data = await res.json();
      setTodos(data);
    } catch (error) {
      console.error("خطا در دریافت تسک‌ها:", error);
    } finally {
      setLoading(false);
    }
  };

  const addTodo = async () => {
    if (!newTitle.trim() || submitting) return;

    try {
      setSubmitting(true);

      const res = await fetch(API_URL, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify({
          title: newTitle.trim(),
        }),
      });

      if (!res.ok) {
        throw new Error("خطا در افزودن تسک");
      }

      const created = await res.json();

      setTodos((prev) => [...prev, created]);
      setNewTitle("");
    } catch (error) {
      console.error("خطا در افزودن تسک:", error);
    } finally {
      setSubmitting(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    await addTodo();
  };

  const toggleTodo = async (todo) => {
    try {
      const updatedCompleted = !todo.completed;

      const res = await fetch(`${API_URL}/${todo.id}`, {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify({
          completed: updatedCompleted,
        }),
      });

      if (!res.ok) {
        throw new Error("خطا در تغییر وضعیت تسک");
      }

      const updatedTodo = await res.json();

      setTodos((prev) =>
        prev.map((t) => (t.id === todo.id ? updatedTodo : t))
      );
    } catch (error) {
      console.error("خطا در تغییر وضعیت تسک:", error);
    }
  };

  const deleteTodo = async (id) => {
    try {
      const res = await fetch(`${API_URL}/${id}`, {
        method: "DELETE",
        credentials: "include",
      });

      if (!res.ok) {
        throw new Error("خطا در حذف تسک");
      }

      setTodos((prev) => prev.filter((t) => t.id !== id));
    } catch (error) {
      console.error("خطا در حذف تسک:", error);
    }
  };

  const done = todos.filter((t) => t.completed).length;

  const chartData = [
    { name: "انجام‌شده", value: done },
    { name: "در انتظار", value: todos.length - done },
  ];

  return (
    <div className="min-h-screen bg-gray-50 p-6" dir="rtl">
      <div className="mb-6 flex justify-center rounded-2xl bg-white p-4 shadow">
        <PieChart width={260} height={200}>
          <Pie
            data={chartData}
            cx={120}
            cy={90}
            innerRadius={50}
            outerRadius={80}
            dataKey="value"
          >
            {chartData.map((_, i) => (
              <Cell key={i} fill={COLORS[i]} />
            ))}
          </Pie>
          <Tooltip />
          <Legend />
        </PieChart>
      </div>

      <form onSubmit={handleSubmit} className="mb-4 flex gap-2">
        <input
          className="flex-1 rounded-lg border px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-400"
          placeholder="تسک جدید..."
          value={newTitle}
          onChange={(e) => setNewTitle(e.target.value)}
        />

        <button
          type="submit"
          disabled={submitting}
          className="rounded-lg bg-indigo-600 px-4 text-sm text-white hover:bg-indigo-700 disabled:cursor-not-allowed disabled:opacity-60"
        >
          {submitting ? "در حال افزودن..." : "افزودن"}
        </button>
      </form>

      {loading ? (
        <p className="text-sm text-gray-500">در حال دریافت تسک‌ها...</p>
      ) : todos.length === 0 ? (
        <p className="text-sm text-gray-500">هنوز تسکی ثبت نشده است.</p>
      ) : (
        <ul className="space-y-2">
          {todos.map((todo) => (
            <li
              key={todo.id}
              className="flex items-center justify-between rounded-xl bg-white px-4 py-3 shadow-sm"
            >
              <div className="flex items-center gap-3">
                <input
                  type="checkbox"
                  checked={todo.completed}
                  onChange={() => toggleTodo(todo)}
                  className="h-4 w-4 accent-indigo-600"
                />

                <span
                  className={`text-sm ${
                    todo.completed
                      ? "text-gray-400 line-through"
                      : "text-gray-700"
                  }`}
                >
                  {todo.title}
                </span>
              </div>

              <button
                onClick={() => deleteTodo(todo.id)}
                type="button"
                className="text-xs text-red-400 hover:text-red-600"
              >
                حذف
              </button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}




