using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using System.Diagnostics;
using Windows.Devices.I2c;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 を参照してください

namespace DRV8830
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private I2cDevice MOTOR;
        private byte MotorAddr = 0x64;
        private int speed=25;
        private int dir=1;

        private const int fwd = 1;
        private const int rev = 2;

        /// <summary>
        /// 
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            Unloaded += MainPage_Unloaded;
            InitMotor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            MOTOR.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        private async void InitMotor()
        {

            try
            {
                var i2c = await I2cController.GetDefaultAsync();
                MOTOR = i2c.GetDevice(new I2cConnectionSettings(MotorAddr));
                MOTOR.Write(new byte[] { 0x00, 0x03 });
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFwd_Click(object sender, RoutedEventArgs e)
        {
            dir = fwd;
            var val = (speed << 2) + dir;
            MOTOR.Write(new byte[] { 0x00, (byte)val });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            MOTOR.Write(new byte[] { 0x00, 0x03 });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRev_Click(object sender, RoutedEventArgs e)
        {
            dir = rev;
            var val = (speed << 2) + dir;
            MOTOR.Write(new byte[] { 0x00, (byte)val });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSlow_Click(object sender, RoutedEventArgs e)
        {
            if (speed < 18)
            {
                speed = 18;
            }
            else
            {
                speed -= 1;
            }
            var val = (speed << 2) + dir;
            MOTOR.Write(new byte[] { 0x00, (byte)val });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFast_Click(object sender, RoutedEventArgs e)
        {
            if (speed > 31)
            {
                speed = 31;
            }
            else
            {
                speed += 1;
            }
            var val = (speed << 2) + dir;
            MOTOR.Write(new byte[] { 0x00, (byte)val });
        }
    }
}
