// Made And Checked By DELTA SYNTH & Gemini AI
// ต้นฉบับโดย OpenUtau Team (https://github.com/stakira/OpenUtau)

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using OpenUtau.App.ViewModels;
using OpenUtau.Core;
using Serilog;

namespace OpenUtau.App {
    public class Program {
        // ตัวแปร Mutex สำหรับตรวจสอบการทำงานซ้ำซ้อนแบบรวดเร็ว (Fast Single Instance Check)
        private static Mutex appMutex;

        // โค้ดเริ่มต้นการทำงาน ห้ามเรียกใช้ Avalonia หรือ API ภายนอกก่อนที่ AppMain จะถูกเรียก
        [STAThread]
        public static void Main(string[] args) {
            // ลงทะเบียนผู้ให้บริการ Encoding เพื่อรองรับรหัสภาษาที่หลากหลาย
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            InitLogging();

            string processName = Process.GetCurrentProcess().ProcessName;
            
            // [เข้าโปรแกรมไว] ใช้ Mutex ตรวจสอบการเปิดโปรแกรมซ้ำแทนการสแกน Process ซึ่งทำงานได้เร็วกว่ามาก
            appMutex = new Mutex(true, "OpenUtau_SingleInstance_Mutex", out bool createdNew);
            
            if (processName != "dotnet" && !createdNew) {
                Log.Information($"โปรเซส {processName} กำลังทำงานอยู่แล้ว ระบบกำลังดำเนินการปิดการทำงานที่ซ้ำซ้อนอย่างรวดเร็ว");
                return; // ออกทันที
            }

            // บันทึกข้อมูลระบบลงใน Log เพื่อการตรวจสอบปัญหา
            Log.Information($"ระบบปฏิบัติการ: {Environment.OSVersion}");
            Log.Information($"{RuntimeInformation.OSDescription} " +
                $"{RuntimeInformation.OSArchitecture} " +
                $"{RuntimeInformation.ProcessArchitecture}");
            Log.Information($"OpenUtau เวอร์ชั่น: v{Assembly.GetEntryAssembly()?.GetName().Version} " +
                $"{RuntimeInformation.RuntimeIdentifier}");
            Log.Information($"ที่อยู่ข้อมูล (Data Path): {PathManager.Inst.DataPath}");
            Log.Information($"ที่อยู่แคช (Cache Path): {PathManager.Inst.CachePath}");
            Log.Information($"การเข้ารหัสระบบ: {Encoding.GetEncoding(0)?.WebName ?? "null"}");

            try {
                Run(args);
                Log.Information($"กำลังปิดโปรแกรมอย่างปกติ");
            } catch (Exception ex) {
                Log.Fatal(ex, "เกิดข้อผิดพลาดร้ายแรงขณะรันโปรแกรม");
            } finally {
                if (!OS.IsMacOS()) {
                    // ทำความสะอาดระบบเครือข่ายสำหรับ Windows/Linux แบบไม่รอ (Non-blocking)
                    NetMQ.NetMQConfig.Cleanup(/*block=*/false);
                }
                
                // [ออกไว] บังคับให้เขียน Log ที่ค้างอยู่ลงไฟล์และปิดการทำงานของระบบ Log ทันที
                Log.CloseAndFlush();
                
                // คืนค่า Mutex เพื่อเคลียร์ทรัพยากรระบบอย่างสมบูรณ์
                if (createdNew) {
                    appMutex.ReleaseMutex();
                    appMutex.Dispose();
                }
            }
        }

        // การตั้งค่า Avalonia สำหรับส่วนติดต่อผู้ใช้ (UI)
        public static AppBuilder BuildAvaloniaApp() {
            FontManagerOptions fontOptions = new();
            
            // ปรับสมดุลการแสดงผลฟอนต์ภาษาไทยให้ครอบคลุมทุก OS
            string thaiFonts = "Leelawadee UI, Tahoma, Sarabun, Ayuthaya, Thonburi, FreeSans";

            if (OS.IsLinux()) {
                try {
                    using Process process = Process.Start(new ProcessStartInfo("fc-match")
                    {
                        ArgumentList = { "-f", "%{family}" },
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    })!;
                    
                    // [เข้าโปรแกรมไว] ใส่ Timeout ป้องกันโปรแกรมค้างหากคำสั่ง fc-match ของ Linux ตอบสนองช้า
                    if (process.WaitForExit(500)) { 
                        string fontFamily = process.StandardOutput.ReadToEnd().Trim();
                        if (!string.IsNullOrEmpty(fontFamily)) {
                            string[] fontFamilies = fontFamily.Split(',');
                            fontOptions.DefaultFamilyName = $"{fontFamilies[0]}, {thaiFonts}";
                        }
                    } else {
                        // หากหมดเวลา ให้ข้ามไปใช้ค่า Default ทันที
                        fontOptions.DefaultFamilyName = thaiFonts;
                    }
                } catch {
                    fontOptions.DefaultFamilyName = thaiFonts; // Fallback เมื่อเกิด Error
                }
            } else if (OS.IsMacOS()) {
                // สำหรับ macOS เน้นฟอนต์ที่แสดงผลภาษาไทยและญี่ปุ่นได้ชัดเจน
                fontOptions.DefaultFamilyName = $"Hiragino Sans, {thaiFonts}, San Francisco, Helvetica Neue";
            } else if (OS.IsWindows()) {
                // สำหรับ Windows เน้น Leelawadee UI ซึ่งเป็นมาตรฐานของภาษาไทย
                fontOptions.DefaultFamilyName = $"Segoe UI, {thaiFonts}";
            }

            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI()
                .With(fontOptions) // ใช้การตั้งค่าฟอนต์ที่เราปรับปรุงแล้ว
                .With(new X11PlatformOptions { EnableIme = true }); // รองรับการพิมพ์ภาษาไทย (IME) บน Linux
        }

        public static void Run(string[] args)
            => BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(
                    args, ShutdownMode.OnMainWindowClose);

        // ระบบบันทึก Log และการจัดการข้อผิดพลาดที่ไม่คาดคิด
        public static void InitLogging() {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Debug()
                .WriteTo.Logger(lc => lc
                    .MinimumLevel.Information()
                    .WriteTo.File(PathManager.Inst.LogFilePath, rollingInterval: RollingInterval.Day, encoding: Encoding.UTF8))
                .WriteTo.Logger(lc => lc
                    .MinimumLevel.ControlledBy(DebugViewModel.Sink.Inst.LevelSwitch)
                    .WriteTo.Sink(DebugViewModel.Sink.Inst))
                .CreateLogger();

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((sender, args) => {
                Log.Error((Exception)args.ExceptionObject, "พบข้อผิดพลาดที่ไม่สามารถจัดการได้ (Unhandled Exception)");
            });
            Log.Information("เริ่มต้นระบบบันทึกข้อมูลเรียบร้อยแล้ว");
        }
    }
}
