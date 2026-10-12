import { BrowserRouter, Route, Routes } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import ProtectedRoute from "./components/ProtectedRoute";
import Layout from "./components/Layout";
import Login from "./pages/Login";
import RegisterPage from "./pages/RegisterPage";
import UserDashboard from "./pages/UserDashboard";
import AdminDashboard from "./pages/AdminDashboard";
import EmailPreview from "./components/EmailPreview";
import NotFound from "./components/NotFound";

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          {/* مسیرهای عمومی */}
          <Route path="/" element={<Login />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<RegisterPage />} />

          {/* مسیرهای محافظت‌شده مشترک */}
          <Route
            element={
              <ProtectedRoute>
                <Layout />
              </ProtectedRoute>
            }
          >
            {/* داشبورد کاربر */}
            <Route path="/dashboard" element={<UserDashboard />} />
          </Route>

          {/* مسیرهای مخصوص مدیر */}
          <Route
            element={
              <ProtectedRoute requireAdmin>
                <Layout />
              </ProtectedRoute>
            }
          >
            <Route path="/admin" element={<AdminDashboard />} />

            <Route
              path="/admin/email-preview"
              element={<EmailPreview />}
            />
          </Route>

          {/* صفحه پیدا نشد */}
          <Route path="*" element={<NotFound />} />
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  );
}





