export function generateBirthdayEmailTemplate({
  userName = "کاربر",
  appName = "Task Manager",
  supportEmail = "support@taskmanager.com",
  logoText = "TM",
  birthMessage = "امیدواریم امروز پر از شادی، موفقیت و لحظه‌های فوق‌العاده برایت باشد.",
  giftText = "تولدتان مبارک! از همراهی شما با ما واقعاً خوشحالیم.",
}) {
  return `
<!DOCTYPE html>
<html lang="fa" dir="rtl">
  <head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>تبریک تولد</title>
  </head>
  <body style="margin:0; padding:0; background-color:#fff7ed; font-family:Tahoma, Arial, sans-serif;">
    <table width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color:#fff7ed; padding:24px 12px;">
      <tr>
        <td align="center">
          <table width="620" cellpadding="0" cellspacing="0" border="0" style="width:620px; max-width:100%; background-color:#ffffff; border-radius:18px; overflow:hidden;">
            
            <!-- Header -->
            <tr>
              <td style="background:linear-gradient(135deg, #f97316, #ec4899); padding:28px 24px; text-align:center;">
                <div style="display:inline-block; background:#ffffff; color:#f97316; font-size:20px; font-weight:bold; width:52px; height:52px; line-height:52px; border-radius:14px; text-align:center;">
                  ${logoText}
                </div>
                <h1 style="margin:16px 0 8px; color:#ffffff; font-size:26px; font-weight:bold;">
                  🎉 تولدت مبارک 🎂
                </h1>
                <p style="margin:0; color:#fff1f2; font-size:14px;">
                  از طرف تیم ${appName}
                </p>
              </td>
            </tr>

            <!-- Body -->
            <tr>
              <td style="padding:32px 24px; color:#111827; font-size:15px; line-height:2;">
                <p style="margin:0 0 14px;">
                  سلام <strong>${userName}</strong> عزیز،
                </p>

                <p style="margin:0 0 14px;">
                  ${giftText}
                </p>

                <div style="
                  margin:24px 0;
                  background:linear-gradient(135deg, #fff7ed, #fdf2f8);
                  border:1px solid #fed7aa;
                  border-radius:16px;
                  padding:20px;
                  text-align:center;
                ">
                  <div style="font-size:42px; margin-bottom:10px;">🎈🎁🎊</div>
                  <p style="margin:0; color:#9a3412; font-size:16px; font-weight:bold; line-height:2;">
                    ${birthMessage}
                  </p>
                </div>

                <p style="margin:0 0 14px;">
                  امیدواریم سال جدید زندگی‌ات پر از پیشرفت، سلامتی و کلی خبر خوب باشد.
                </p>

                <p style="margin:0;">
                  با آرزوی بهترین‌ها برای تو 🌹
                </p>
              </td>
            </tr>

            <!-- Footer -->
            <tr>
              <td style="background-color:#fff7ed; padding:20px 24px; text-align:center; color:#7c2d12; font-size:12px; line-height:1.9;">
                <p style="margin:0 0 8px;">
                  این پیام با عشق از طرف <strong>${appName}</strong> برای شما ارسال شده است.
                </p>
                <p style="margin:0 0 8px;">
                  ارتباط با ما:
                  <a href="mailto:${supportEmail}" style="color:#ea580c; text-decoration:none;">${supportEmail}</a>
                </p>
                <p style="margin:0;">
                  © 2026 ${appName}. همه حقوق محفوظ است.
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
