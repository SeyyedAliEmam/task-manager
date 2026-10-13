import { createContext, useContext, useState, useEffect } from "react";

const AuthContext = createContext(null);


const BASE_URL = "https://localhost:7299/api/AccountApi";

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true); 

  useEffect(() => {
    const checkLoginStatus = async () => {
      try {
        const res = await fetch(`${BASE_URL}/current-user`, {
          method: "GET",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
        });

        if (res.ok) {
          const data = await res.json();
          if (data.isAuthenticated) {
            setUser(data.user);
          } else {
            setUser(null);
          }
        }
      } catch (error) {
        console.error("خطا در بررسی وضعیت سشن کاربر:", error);
      } finally {
        setLoading(false);
      }
    };

    checkLoginStatus();
  }, []);

  const login = async (email, password) => {
    const res = await fetch(`${BASE_URL}/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include", 
      body: JSON.stringify({
        email: email.trim(),
        password: password,
        rememberMe: true, 
      }),
    });

    if (!res.ok) {
      const errorData = await res.json();
      throw new Error(errorData.message || "اطلاعات وارد شده صحیح نیست");
    }

    const userData = await res.json(); 
    setUser(userData);
    return userData;
  };


  const register = async (email, password, rePassword, dateOfBirth) => {
    try {
      const res = await fetch(`${BASE_URL}/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify({
          email: email.trim(),
          password: password.trim(),
          rePassword: rePassword.trim(),
          dateOfBirth: dateOfBirth, 
        }),
      });

      const data = await res.json();

      if (!res.ok) {
        return { success: false, message: data.message || "خطایی رخ داده است" };
      }
      return { success: true, message: data.message };
    } catch (error) {
      return { success: false, message: "ارتباط با سرور برقرار نشد" };
    }
  };

  const logout = async () => {
    try {
      await fetch(`${BASE_URL}/logout`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      });
    } catch (error) {
      console.error("خطا در خروج از حساب:", error);
    } finally {
      setUser(null);
    }
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, register, loading }}>
      {!loading && children} 
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}


