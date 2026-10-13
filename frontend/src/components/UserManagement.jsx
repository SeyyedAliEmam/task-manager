import { useEffect, useState } from "react";
import { useAuth } from "../context/AuthContext";

const API_URL = "https://localhost:7299/api/AdminUsersApi";

export default function UserManagement({ onUsersChanged }) {
  const { user: currentUser } = useAuth();

  const [users, setUsers] = useState([]);
  const [editingId, setEditingId] = useState(null);
  const [loading, setLoading] = useState(true);

  const [deleteTarget, setDeleteTarget] = useState(null);

  const [deleting, setDeleting] = useState(false);

  const [notification, setNotification] = useState(null);

  const [tempUser, setTempUser] = useState({
    email: "",
    password: "",
    isAdmin: false,
  });

  const showNotification = (type, message) => {
    setNotification({ type, message });

    window.setTimeout(() => {
      setNotification(null);
    }, 4500);
  };

  const getServerErrorMessage = async (res, defaultMessage) => {
    const contentType = res.headers.get("content-type") || "";

    try {
      if (contentType.includes("application/json")) {
        const data = await res.json();

        if (data?.message) return data.message;
        if (data?.title) return data.title;

        if (data?.errors) {
          return Object.values(data.errors).flat().join(" | ");
        }
      }

      const text = await res.text();
      return text || defaultMessage;
    } catch {
      return defaultMessage;
    }
  };

  const fetchUsers = async () => {
    setLoading(true);

    try {
      const res = await fetch(API_URL, {
        credentials: "include",
      });

      if (!res.ok) {
        const message = await getServerErrorMessage(
          res,
          "خطا در بارگذاری لیست کاربران"
        );
        throw new Error(message);
      }

      const data = await res.json();
      setUsers(Array.isArray(data) ? data : []);
    } catch (error) {
      console.error("Fetch users error:", error);
      showNotification("error", error.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  const handleSave = async (id = null) => {
    if (!tempUser.email?.trim()) {
      showNotification("error", "ایمیل الزامی است.");
      return;
    }

    if (!id && !tempUser.password?.trim()) {
      showNotification("error", "رمز عبور الزامی است.");
      return;
    }

    const method = id ? "PUT" : "POST";
    const url = id ? `${API_URL}/${id}` : API_URL;

    const payload = id
      ? {
          email: tempUser.email.trim(),
          isAdmin: Boolean(tempUser.isAdmin),
          ...(tempUser.password.trim()
            ? { password: tempUser.password }
            : {}),
        }
      : {
          email: tempUser.email.trim(),
          password: tempUser.password,
          isAdmin: Boolean(tempUser.isAdmin),
        };

    try {
      const res = await fetch(url, {
        method,
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify(payload),
      });

      if (!res.ok) {
        const message = await getServerErrorMessage(res, "عملیات ذخیره انجام نشد.");
        throw new Error(message);
      }

      setEditingId(null);
      setTempUser({
        email: "",
        password: "",
        isAdmin: false,
      });

      await fetchUsers();
      onUsersChanged?.();

      showNotification(
        "success",
        id ? "اطلاعات کاربر با موفقیت ویرایش شد." : "کاربر جدید با موفقیت ایجاد شد."
      );
    } catch (error) {
      console.error("Save user error:", error);
      showNotification("error", `عملیات ناموفق بود: ${error.message}`);
    }
  };

  const requestDelete = (user) => {
    if (String(user.id) === String(currentUser?.id)) {
      showNotification("error", "شما نمی‌توانید حساب کاربری جاری خود را حذف کنید.");
      return;
    }

    setDeleteTarget(user);
  };

  const confirmDelete = async () => {
    if (!deleteTarget) return;

    const userId = deleteTarget.id;

    try {
      setDeleting(true);

      const res = await fetch(`${API_URL}/${userId}`, {
        method: "DELETE",
        credentials: "include",
      });

      if (!res.ok) {
        const message = await getServerErrorMessage(res, "حذف کاربر انجام نشد.");
        throw new Error(message);
      }

      setUsers((previousUsers) =>
        previousUsers.filter((user) => String(user.id) !== String(userId))
      );

      setDeleteTarget(null);
      onUsersChanged?.();

      showNotification("success", "کاربر با موفقیت حذف شد.");

      await fetchUsers();
    } catch (error) {
      console.error("Delete user error:", error);
      showNotification("error", `خطا در حذف کاربر: ${error.message}`);
    } finally {
      setDeleting(false);
    }
  };

  const startEdit = (user) => {
    setEditingId(user.id);

    setTempUser({
      email: user.email || "",
      password: "",
      isAdmin: Boolean(user.isAdmin),
    });
  };

  const cancelEdit = () => {
    setEditingId(null);

    setTempUser({
      email: "",
      password: "",
      isAdmin: false,
    });
  };

  return (
    <>
      {/* پیام موفقیت / خطا */}
      {notification && (
        <div
          className={[
            "fixed top-5 left-5 z-[60] max-w-md rounded-xl px-4 py-3 text-sm shadow-lg",
            notification.type === "success"
              ? "bg-emerald-600 text-white"
              : "bg-red-600 text-white",
          ].join(" ")}
        >
          {notification.message}
        </div>
      )}

      <div className="mt-8 overflow-hidden rounded-2xl border border-gray-100 bg-white shadow-sm">
        <div className="flex items-center justify-between border-b border-gray-50 bg-gray-50/50 p-4">
          <h2 className="font-bold text-gray-700">مدیریت کاربران</h2>

          <span className="text-xs text-gray-400">
            {users.length} کاربر ثبت شده
          </span>
        </div>

        <div className="overflow-x-auto">
          <table className="w-full text-right text-sm">
            <thead className="bg-gray-50 text-gray-500">
              <tr>
                <th className="p-3">ایمیل (نام کاربری)</th>
                <th className="p-3">رمز عبور</th>
                <th className="p-3">نقش سیستم</th>
                <th className="p-3 text-center">عملیات</th>
              </tr>
            </thead>

            <tbody className="divide-y divide-gray-50">
              {/* فرم افزودن کاربر */}
              <tr className="bg-indigo-50/30">
                <td className="p-2">
                  <input
                    type="email"
                    placeholder="ایمیل جدید..."
                    className="w-full rounded-lg border border-gray-200 p-1.5 text-sm focus:ring-1 focus:ring-indigo-400"
                    value={editingId ? "" : tempUser.email}
                    onChange={(e) => {
                      if (!editingId) {
                        setTempUser({
                          ...tempUser,
                          email: e.target.value.toLowerCase(),
                        });
                      }
                    }}
                    disabled={Boolean(editingId)}
                  />
                </td>

                <td className="p-2">
                  <input
                    type="password"
                    placeholder="رمز..."
                    className="w-full rounded-lg border border-gray-200 p-1.5 text-sm focus:ring-1 focus:ring-indigo-400"
                    value={editingId ? "" : tempUser.password}
                    onChange={(e) => {
                      if (!editingId) {
                        setTempUser({
                          ...tempUser,
                          password: e.target.value,
                        });
                      }
                    }}
                    disabled={Boolean(editingId)}
                  />
                </td>

                <td className="p-2">
                  <select
                    className="border-none bg-transparent text-xs focus:ring-0"
                    value={editingId ? "user" : tempUser.isAdmin ? "admin" : "user"}
                    onChange={(e) => {
                      if (!editingId) {
                        setTempUser({
                          ...tempUser,
                          isAdmin: e.target.value === "admin",
                        });
                      }
                    }}
                    disabled={Boolean(editingId)}
                  >
                    <option value="user">کاربر عادی</option>
                    <option value="admin">مدیر (Admin)</option>
                  </select>
                </td>

                <td className="p-2 text-center">
                  <button
                    type="button"
                    onClick={() => handleSave()}
                    disabled={Boolean(editingId)}
                    className="rounded-lg bg-indigo-600 px-4 py-1.5 text-xs text-white hover:bg-indigo-700 disabled:cursor-not-allowed disabled:opacity-30"
                  >
                    افزودن
                  </button>
                </td>
              </tr>

              {/* لیست کاربران */}
              {users.map((user) => (
                <tr key={user.id} className="transition-colors hover:bg-gray-50">
                  {editingId === user.id ? (
                    <>
                      <td className="p-2">
                        <input
                          type="email"
                          className="w-full rounded-md border p-1"
                          value={tempUser.email}
                          onChange={(e) =>
                            setTempUser({
                              ...tempUser,
                              email: e.target.value.toLowerCase(),
                            })
                          }
                        />
                      </td>

                      <td className="p-2">
                        <input
                          type="password"
                          placeholder="رمز جدید (اختیاری)"
                          className="w-full rounded-md border p-1 placeholder-gray-300"
                          value={tempUser.password}
                          onChange={(e) =>
                            setTempUser({
                              ...tempUser,
                              password: e.target.value,
                            })
                          }
                        />
                      </td>

                      <td className="p-2">
                        <select
                          className="rounded-md border p-1 text-xs"
                          value={tempUser.isAdmin ? "admin" : "user"}
                          onChange={(e) =>
                            setTempUser({
                              ...tempUser,
                              isAdmin: e.target.value === "admin",
                            })
                          }
                        >
                          <option value="user">کاربر عادی</option>
                          <option value="admin">مدیر (Admin)</option>
                        </select>
                      </td>

                      <td className="space-x-2 space-x-reverse p-2 text-center">
                        <button
                          type="button"
                          onClick={() => handleSave(user.id)}
                          className="text-xs font-bold text-green-600 hover:text-green-700"
                        >
                          ذخیره
                        </button>

                        <button
                          type="button"
                          onClick={cancelEdit}
                          className="text-xs text-gray-400 hover:text-gray-600"
                        >
                          لغو
                        </button>
                      </td>
                    </>
                  ) : (
                    <>
                      <td className="p-3 font-medium text-gray-700">
                        {user.email}{" "}
                        {String(user.id) === String(currentUser?.id) && "⭐"}
                      </td>

                      <td className="p-3 text-gray-400">••••••</td>

                      <td className="p-3">
                        <span
                          className={[
                            "rounded-full px-2 py-0.5 text-[10px] font-semibold",
                            user.isAdmin
                              ? "bg-purple-100 text-purple-600"
                              : "bg-gray-100 text-gray-600",
                          ].join(" ")}
                        >
                          {user.isAdmin ? "مدیر (Admin)" : "کاربر عادی"}
                        </span>
                      </td>

                      <td className="space-x-3 space-x-reverse p-3 text-center">
                        <button
                          type="button"
                          onClick={() => startEdit(user)}
                          className="text-xs text-blue-500 hover:text-blue-700"
                        >
                          ویرایش
                        </button>

                        <button
                          type="button"
                          onClick={() => requestDelete(user)}
                          className="text-xs text-red-400 hover:text-red-600"
                        >
                          حذف
                        </button>
                      </td>
                    </>
                  )}
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {loading && (
          <div className="p-4 text-center text-xs text-gray-400">
            در حال بروزرسانی...
          </div>
        )}
      </div>

      {/* مودال تأیید حذف */}
      {deleteTarget && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
          <button
            type="button"
            aria-label="بستن پنجره تأیید حذف"
            disabled={deleting}
            onClick={() => setDeleteTarget(null)}
            className="absolute inset-0 cursor-default bg-black/50 backdrop-blur-[1px]"
          />

          <div
            role="dialog"
            aria-modal="true"
            className="relative w-full max-w-md rounded-2xl bg-white p-6 text-right shadow-2xl"
          >
            <div className="mb-4 flex h-11 w-11 items-center justify-center rounded-full bg-red-100 text-xl text-red-600">
              !
            </div>

            <h3 className="text-lg font-bold text-gray-800">حذف کاربر</h3>

            <p className="mt-2 text-sm leading-6 text-gray-500">
              آیا از حذف کاربر{" "}
              <span className="font-bold text-gray-700">
                {deleteTarget.email}
              </span>{" "}
              اطمینان دارید؟
              <br />
              <span className="text-red-500">این عملیات قابل بازگشت نیست.</span>
            </p>

            <div className="mt-6 flex justify-end gap-3">
              <button
                type="button"
                disabled={deleting}
                onClick={() => setDeleteTarget(null)}
                className="rounded-xl border border-gray-200 px-4 py-2 text-sm text-gray-600 hover:bg-gray-50 disabled:opacity-50"
              >
                انصراف
              </button>

              <button
                type="button"
                disabled={deleting}
                onClick={confirmDelete}
                className="min-w-28 rounded-xl bg-red-600 px-4 py-2 text-sm font-medium text-white hover:bg-red-700 disabled:cursor-not-allowed disabled:opacity-60"
              >
                {deleting ? "در حال حذف..." : "بله، حذف کن"}
              </button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}
