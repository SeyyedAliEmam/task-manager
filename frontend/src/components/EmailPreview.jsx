import { useMemo, useState } from "react";
import { generateOtpEmailTemplate } from "../emailTemplates/otpEmailTemplate";
import { generateBirthdayEmailTemplate } from "../emailTemplates/birthdayEmailTemplate";

export default function EmailPreview() 
{

  const [type, setType] = useState("otp");

  const html = useMemo(() => {
    if (type === "otp") {
      return generateOtpEmailTemplate({
        userName: "کاربر عزیز",
        otpCode: "123456",
        appName: "Task Manager",
        expireMinutes: 5,
        supportEmail: "taskmanager@gmail.com",
        logoText: "TM",
      });
    }

    return generateBirthdayEmailTemplate({
      userName: "کاربر عزیز",
      appName: "Task Manager",
      supportEmail: "taskmanager@gmail.com",
      logoText: "TM",
      birthMessage: "امیدواریم امسال برات پر از رشد، سلامتی و موفقیت باشه.",
      giftText: "امروز روز توئه، پس با انرژی و لبخند جشنش بگیر ✨",
    });
  }, [type]);

  return (
    <div className="min-h-screen bg-slate-100 p-6">
      <div className="mx-auto max-w-6xl">
        <div className="mb-4 rounded-2xl bg-white p-4 shadow-sm">
          <h1 className="mb-4 text-xl font-bold text-slate-800">
            پیش‌ نمایش قالب ایمیل
          </h1>

          <div className="flex flex-wrap gap-3">
            <button
              onClick={() => setType("otp")}
              className={`rounded-xl px-4 py-2 text-sm font-medium transition ${
                type === "otp"
                  ? "bg-indigo-600 text-white"
                  : "bg-slate-200 text-slate-700"
              }`}
            >
              قالب OTP
            </button>

            <button
              onClick={() => setType("birthday")}
              className={`rounded-xl px-4 py-2 text-sm font-medium transition ${
                type === "birthday"
                  ? "bg-pink-600 text-white"
                  : "bg-slate-200 text-slate-700"
              }`}
            >
              قالب تبریک تولد
            </button>
          </div>
        </div>

        <div className="overflow-hidden rounded-2xl bg-white shadow-sm">
          <iframe
            title="email-preview"
            srcDoc={html}
            className="h-[800px] w-full border-0"
          />
        </div>
      </div>
    </div>
  );
}
