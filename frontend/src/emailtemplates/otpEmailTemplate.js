export function generateOtpEmailTemplate({
  userName = "کاربر",
  otpCode = "123456",
  appName = "Task Manager",
  expireMinutes = 5,
  supportEmail = "support@taskmanager.com",
  logoText = "TM",
}) {
  return `
<!DOCTYPE html>
<html lang="fa" dir="rtl">
  <head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>کد ورود یکبار مصرف</title>
  </head>
  <body style="margin:0; padding:0; background-color:#f3f4f6; font-family:Tahoma, Arial, sans-serif;">
    <table width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color:#f3f4f6; padding:24px 12px;">
      <tr>
        <td align="center">
          <table width="620" cellpadding="0" cellspacing="0" border="0" style="width:620px; max-width:100%; background-color:#ffffff; border-radius:18px; overflow:hidden;">
            
            <!-- Header -->
            <tr>
              <td style="background:linear-gradient(135deg, #4f46e5, #7c3aed); padding:28px 24px; text-align:center;">
                <div style="display:inline-block; background:#ffffff; color:#4f46e5; font-size:20px; font-weight:bold; width:52px; height:52px; line-height:52px; border-radius:14px; text-align:center;">
                  ${logoText}
                </div>
                <h1 style="margin:16px 0 8px; color:#ffffff; font-size:24px; font-weight:bold;">
                  ${appName}
                </h1>
                <p style="margin:0; color:#e9e7ff; font-size:14px;">
                  کد ورود یکبار مصرف شما آماده است
                </p>
              </td>
            </tr>

            <!-- Body -->
            <tr>
              <td style="padding:32px 24px; color:#111827; font-size:15px; line-height:2;">
                <p style="margin:0 0 14px;">
                  سلام <strong>${userName}</strong>،
                </p>

                <p style="margin:0 0 14px;">
                  برای ورود به حساب کاربری‌ات، از کد یکبار مصرف زیر استفاده کن:
                </p>

                <div style="margin:28px 0; text-align:center;">
                  <div style="
                    display:inline-block;
                    background-color:#eef2ff;
                    border:2px dashed #4f46e5;
                    color:#4f46e5;
                    font-size:32px;
                    font-weight:bold;
                    letter-spacing:8px;
                    padding:18px 28px;
                    border-radius:16px;
                    direction:ltr;
                  ">
                    ${otpCode}
                  </div>
                </div>

                <p style="margin:0 0 14px;">
                  این کد تا <strong>${expireMinutes} دقیقه</strong> معتبر است.
                </p>

                <p style="margin:0 0 14px; color:#b91c1c;">
                  اگر این درخواست توسط شما ثبت نشده است، لطفاً این ایمیل را نادیده بگیرید و در صورت نیاز رمز عبور خود را تغییر دهید.
                </p>

                <div style="margin-top:24px; padding:16px; background:#f9fafb; border-radius:12px; color:#4b5563; font-size:13px; line-height:1.9;">
                  <strong>نکته امنیتی:</strong>
                  این کد را در اختیار هیچ شخص دیگری قرار ندهید. تیم ${appName} هرگز کد ورود شما را درخواست نمی‌کند.
                </div>
              </td>
            </tr>

            <!-- Footer -->
            <tr>
              <td style="background-color:#f9fafb; padding:20px 24px; text-align:center; color:#6b7280; font-size:12px; line-height:1.9;">
                <p style="margin:0 0 8px;">
                  این ایمیل به صورت خودکار از طرف <strong>${appName}</strong> ارسال شده است.
                </p>
                <p style="margin:0 0 8px;">
                  برای پشتیبانی: 
                  <a href="mailto:${supportEmail}" style="color:#4f46e5; text-decoration:none;">${supportEmail}</a>
                </p>
                <p style="margin:0;">
                  © 2026 ${appName}. تمامی حقوق محفوظ است.
                </p>
              </td>
            </tr>

          </table>
        </td>
      </tr>
    </table>
  </body>
</html>
  `;
}
