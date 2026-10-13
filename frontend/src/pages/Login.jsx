import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function Login() {
  const { login } = useAuth();
  const navigate = useNavigate();

  const [form, setForm] = useState({ email: "", password: "" });
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const onChange = (key) => (e) => {
    setForm((prev) => ({ ...prev, [key]: e.target.value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    const email = form.email.trim();
    const password = form.password;

    if (!email || !password) {
      setError("لطفاً ایمیل و رمز عبور را وارد کنید");
      return;
    }

    setLoading(true);
    try {
      const user = await login(email, password);

      if (user?.isAdmin === true) {
        navigate("/admin", { replace: true });
      } else {
        navigate("/dashboard", { replace: true });
      }
    } catch (err) {
      setError(err?.message || "خطا در ورود");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 px-4" dir="rtl">
      <form
        onSubmit={handleSubmit}
        className="bg-white p-8 rounded-lg shadow-md w-full max-w-sm"
      >
        <h2 className="text-2xl font-bold mb-6 text-center text-gray-800">ورود</h2>

        {error && (
          <p className="bg-red-100 text-red-600 text-sm p-2 rounded mb-4 text-center">
            {error}
          </p>
        )}

        <div className="mb-4">
          <label className="block text-gray-700 mb-1">ایمیل</label>
          <input
            type="email"
            value={form.email.toLowerCase()}
            onChange={onChange("email")}
            className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
            placeholder="example@gmail.com"
            autoComplete="email"
          />
        </div>

        <div className="mb-6">
          <label className="block text-gray-700 mb-1">رمز عبور</label>
          <input
            type="password"
            value={form.password}
            onChange={onChange("password")}
            className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
            placeholder="********"
            autoComplete="current-password"
          />
        </div>

        <button
          type="submit"
          disabled={loading}
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 transition disabled:opacity-50"
        >
          {loading ? "در حال ورود..." : "ورود"}
        </button>

        <p className="text-center text-sm text-gray-600 mt-4">
          حساب ندارید؟{" "}
          <Link to="/register" className="text-blue-600 hover:underline">
            ثبت‌نام
          </Link>
        </p>
      </form>
    </div>
  );
}







