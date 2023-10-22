using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace NullTool
{
    public partial class APIWindow
    {
        private Timer mouseClickTimer;
        private System.Drawing.Point fixedMousePosition; // Vị trí cố định của chuột

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;

        public APIWindow()
        {
            InitializeComponent();

            // Thiết lập vị trí cố định ban đầu (giữa và ở dưới thanh toolbar)
            int screenX = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            int screenY = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
            int toolbarHeight = 40; // Điều này có thể thay đổi tùy thuộc vào thanh toolbar của bạn
            fixedMousePosition = new System.Drawing.Point(screenX / 2, screenY - toolbarHeight / 2);

            // Tạo timer để thực hiện sự kiện click chuột mỗi 1 giây
            mouseClickTimer = new Timer();
            mouseClickTimer.Interval = 1000; //1 phut
            mouseClickTimer.Tick += MouseClickTimer_Tick;
            mouseClickTimer.Start();
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MouseDown_Border(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                // Khi người dùng click chuột trái, thực hiện sự kiện click
                mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)fixedMousePosition.X, (uint)fixedMousePosition.Y, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, (uint)fixedMousePosition.X, (uint)fixedMousePosition.Y, 0, 0);
            }
        }

        private void MouseClickTimer_Tick(object sender, EventArgs e)
        {
            // Thực hiện sự kiện click chuột trái ở vị trí cố định
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)fixedMousePosition.X, (uint)fixedMousePosition.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)fixedMousePosition.X, (uint)fixedMousePosition.Y, 0, 0);
        }

        private void ChangeInterval_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem người dùng đã nhập giá trị hợp lệ chưa
            if (int.TryParse(txtInterval.Text, out int interval))
            {
                // Thiết lập thời gian cho timer bằng giá trị mà người dùng đã nhập
                mouseClickTimer.Interval = interval;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Giá trị thời gian không hợp lệ.");
            }
        }


    }
}
