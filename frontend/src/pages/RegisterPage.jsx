import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

const RegisterPage = () => {
  const [form, setForm] = useState({
    email: "",
    password: "",
    rePassword: "",
    dateOfBirth: "",
  });

  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const { register } = useAuth();
  const navigate = useNavigate();

  const handleChange = (key) => (e) => {
    setForm((prev) => ({
      ...prev,
      [key]: e.target.value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    if (
      !form.email.trim() ||
      !form.password.trim() ||
      !form.rePassword.trim() ||
      !form.dateOfBirth
    ) {
      setError("لطفاً تمام فیلدها را پر کنید");
      return;
    }

    if (form.password !== form.rePassword) {
      setError("رمز عبور و تکرار آن یکسان نیستند");
      return;
    }

    setLoading(true);

    try {
      const result = await register(
        form.email.trim(),
        form.password,
        form.rePassword,
        form.dateOfBirth
      );

      if (result.success) {
        navigate("/login", { replace: true });
      } else {
        setError(result.message || "ثبت‌نام انجام نشد");
      }
    } catch (err) {
      setError(err.message || "خطا در ثبت‌نام");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div
      className="min-h-screen flex items-center justify-center bg-gray-100 px-4"
      dir="rtl"
    >
      <form
        onSubmit={handleSubmit}
        className="bg-white p-8 rounded-lg shadow-md w-full max-w-md"
      >
        <h2 className="text-2xl font-bold mb-6 text-center text-gray-800">
          ثبت‌نام
        </h2>

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
            onChange={handleChange("email")}
            className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
            placeholder="example@gmail.com"
          />
        </div>

        <div className="mb-4">
          <label className="block text-gray-700 mb-1">رمز عبور</label>
          <input
            type="password"
            value={form.password}
            onChange={handleChange("password")}
            className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
            placeholder="********"
          />
        </div>

        <div className="mb-4">
          <label className="block text-gray-700 mb-1">تکرار رمز عبور</label>
          <input
            type="password"
            value={form.rePassword}
            onChange={handleChange("rePassword")}
            className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
            placeholder="********"
          />
        </div>

        <div className="mb-6">
          <label className="block text-gray-700 mb-1">تاریخ تولد</label>
          <input
            type="date"
            value={form.dateOfBirth}
            onChange={handleChange("dateOfBirth")}
            className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
          />
        </div>

        <button
          type="submit"
          disabled={loading}
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 transition disabled:opacity-50"
        >
          {loading ? "در حال ثبت‌نام..." : "ثبت‌نام"}
        </button>

        <p className="text-center text-sm text-gray-600 mt-4">
          قبلاً حساب ساخته‌ای؟{" "}
          <Link to="/login" className="text-blue-600 hover:underline">
            ورود
          </Link>
        </p>
      </form>
    </div>
  );
};

export default RegisterPage;





