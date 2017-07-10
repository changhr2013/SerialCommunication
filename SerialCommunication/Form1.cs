using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using INIFILE;
using System.Text.RegularExpressions;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        SerialPort sp1 = new SerialPort();

        public Form1()
        {
            InitializeComponent();
        }
        //窗口加载
        private void Form1_Load(object sender, EventArgs e)
        {
            //加载所有
            INIFILE.Profile.LoadProfile();

            // 预置波特率
            switch (Profile.G_BAUDRATE)
            {
                case "300":
                    cbBaudRate.SelectedIndex = 0;
                    break;
                case "600":
                    cbBaudRate.SelectedIndex = 1;
                    break;
                case "1200":
                    cbBaudRate.SelectedIndex = 2;
                    break;
                case "2400":
                    cbBaudRate.SelectedIndex = 3;
                    break;
                case "4800":
                    cbBaudRate.SelectedIndex = 4;
                    break;
                case "9600":
                    cbBaudRate.SelectedIndex = 5;
                    break;
                case "19200":
                    cbBaudRate.SelectedIndex = 6;
                    break;
                case "38400":
                    cbBaudRate.SelectedIndex = 7;
                    break;
                case "115200":
                    cbBaudRate.SelectedIndex = 8;
                    break;
                default:
                    {
                        MessageBox.Show("波特率预置参数错误。", "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
            }

            //预置波特率
            switch (Profile.G_DATABITS)
            {
                case "5":
                    cbDataBits.SelectedIndex = 0;
                    break;
                case "6":
                    cbDataBits.SelectedIndex = 1;
                    break;
                case "7":
                    cbDataBits.SelectedIndex = 2;
                    break;
                case "8":
                    cbDataBits.SelectedIndex = 3;
                    break;
                default:
                    {
                        MessageBox.Show("数据位预置参数错误。", "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

            }
            //预置停止位
            switch (Profile.G_STOP)
            {
                case "1":
                    cbStop.SelectedIndex = 0;
                    break;
                case "1.5":
                    cbStop.SelectedIndex = 1;
                    break;
                case "2":
                    cbStop.SelectedIndex = 2;
                    break;
                default:
                    {
                        MessageBox.Show("停止位预置参数错误。", "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
            }

            //预置校验位
            switch (Profile.G_PARITY)
            {
                case "NONE":
                    cbParity.SelectedIndex = 0;
                    break;
                case "ODD":
                    cbParity.SelectedIndex = 1;
                    break;
                case "EVEN":
                    cbParity.SelectedIndex = 2;
                    break;
                default:
                    {
                        MessageBox.Show("校验位预置参数错误。","异常提示信息",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                    }
            }

            //检查是否含有串口  
               string[] str = SerialPort.GetPortNames();  
               if (str == null)  
               {  
                   MessageBox.Show("本机没有串口！", "信息提示",MessageBoxButtons.OK,MessageBoxIcon.Information);  
                   return;  
               }  
      
               //添加串口项目  
               foreach (string s in System.IO.Ports.SerialPort.GetPortNames())  
               {
                   //获取有多少个COM口  
                   cbSerial.Items.Add(s);  
               }  
      
               //串口设置默认选择项  
               //设置cbSerial的默认选项  
               cbSerial.SelectedIndex = 0;
               sp1.BaudRate = 9600;

            //这个类中不检查跨线程的调用是否合法
            /*.NET2.0以后加强了安全机制，不允许在WinForm中直接跨线程访问控件的属性*/
               Control.CheckForIllegalCrossThreadCalls = false;

               sp1.DataReceived += new SerialDataReceivedEventHandler(sp1_DataReceived);

            //发送和接收格式默认是字符串
               rbSendStr.Checked = true;
               rbRcvStr.Checked = true;

            //设置数据读取超时为1秒
               sp1.ReadTimeout = 1000;

            //设置编码
               sp1.Encoding = System.Text.Encoding.GetEncoding("UTF-8");

            //关闭串口
               sp1.Close();
        }

        /// <summary>
        /// 对接收的数据进行处理的方法
        /// </summary>
        void sp1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sp1.IsOpen)
            {
                //输出当前时间
                DateTime dateTime = DateTime.Now;

                txtRcv.Text += dateTime.GetDateTimeFormats('f')[0].ToString() + "\r\n";

                //改变前景色为蓝色
                txtRcv.SelectAll();
                txtRcv.ForeColor = Color.Blue;

                //BytestoRead:sp1接收的字符个数
                //byte[] byteRead = new byte[sp1.BytesToRead];

                //rdRcvStr:"接收字符串"单选按钮
                if (rbRcvStr.Checked)
                {
                    try
                    {
                        byte[] strBytes = new byte[sp1.BytesToRead];
                        sp1.Read(strBytes, 0, strBytes.Length);
                        sp1.DiscardInBuffer();

                        string receiveStr = System.Text.Encoding.UTF8.GetString(strBytes);
                        txtRcv.Text += receiveStr + "\r\n";

                        //清空SerialPort控件中的buffer
                        sp1.DiscardInBuffer();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "异常信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSend.Text = "";
                    }
                }
                else
                {
                    try
                    {
                        //创建接收字节数组
                        Byte[] receiveData = new Byte[sp1.BytesToRead];

                        //读取数据
                        sp1.Read(receiveData, 0, receiveData.Length);

                        //清空SerialPort控件中的buffer
                        sp1.DiscardInBuffer();

                        string strRcv = null;

                        for (int i = 0; i < receiveData.Length; i++)
                        {
                            //16进制显示
                            strRcv += String.Format(" {0} ",receiveData[i].ToString("X2"));
                        }
                        txtRcv.Text += strRcv+"\r\n";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,"异常提示信息",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        txtSend.Text = "";
                    }
                }
            }
            else
            {
                MessageBox.Show("请打开某个串口", "信息提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// "清空"按钮事件
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtRcv.Text = "";
        }

        /// <summary>
        /// "退出"按钮事件
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 点击“发送按钮”事件
        /// </summary>
        private void btnSend_Click(object sender, EventArgs e)
        {
            //发送前判断串口是否打开
            if (!sp1.IsOpen)
            {
                MessageBox.Show("请先打开串口！","提示信息",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            //判断cbTimeSend是否被勾选，来决定是否启动Timer控件进行定时发送
            if (cbTimeSend.Checked)
            {
                timeSend.Enabled = true;
            }
            else
            {
                timeSend.Enabled = false;
            }

            string strSend = txtSend.Text;

            if (rbSend16.Checked==true)
            {
                //如果选择的16进制发送，处理数字转换
                string sendBuf = strSend;
                //去除空格，并将原来的中英文逗号替换成空格,最后去掉16进制的0x
                string sendNoNull = sendBuf.Trim();                
                string sendNoEComma = sendNoNull.Replace(',', ' ');
                string sendNoCComma = sendNoEComma.Replace('，', ' ');
                string strSend16Scale=sendNoCComma.Replace("0x","");
                strSend16Scale.Replace("0X","");

                //将数据用空格分隔成字符串数组
                string[] strArray = strSend16Scale.Split(' ');

                int byteBufferLength = strArray.Length;

                for (int i = 0; i < byteBufferLength; i++)
                {
                    if (strArray[i]=="")
                    {
                        byteBufferLength--;
                    }
                }

                byte[] byteBuffer = new byte[byteBufferLength];
                int ii = 0;
                for (int i = 0; i < strArray.Length; i++)
                {
                    //将获取的字符做相加运算？
                    Byte[] bytesOfStr = Encoding.Default.GetBytes(strArray[i]);

                    int decNum = 0;
                    if (strArray[i]=="")
                    {
                        continue;
                    }
                    else
                    {
                        decNum = Convert.ToInt32(strArray[i], 16);
                    }

                    try
                    {
                        //为了防止用户输入错误，所以规定用户只能一个字节一个字节的输入数据
                        byteBuffer[ii] = Convert.ToByte(decNum);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("字节越界，请逐个字节输入！\n"+ex.Message, "异常提示信息",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        timeSend.Enabled = false;
                        return;
                    }

                    ii++;
                }

                sp1.Write(byteBuffer, 0, byteBuffer.Length);

            }
            else if(rbSendStr.Checked==true)
            {
                //以字符串形式发送时,直接写入数据
                sp1.WriteLine(String.Format("<{0}>",txtSend.Text));                
            }
            else
            {
                MessageBox.Show("请选择数据发送的格式！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 定时发送的定时器Timer
        /// </summary>
        private void timeSend_Tick(object sender, EventArgs e)
        {
            //转换时间间隔
            string strSecond = txtSecond.Text;

            try
            {
                //Interval的单位是ms
                int isecond = int.Parse(strSecond) * 1000;
                timeSend.Interval = isecond;

                if (timeSend.Enabled==true)
                {
                    //如果timeSend空间是可用状态，用按钮的PerformClick()方法生成按钮的点击事件。
                    btnSend.PerformClick();
                }
            }
            catch (Exception ex)
            {
                //若出现异常，就将Timer控件状态设为不可用，然后抛出异常提示
                timeSend.Enabled = false;
                MessageBox.Show("设置的时间间隔格式错误!\n"+ex.Message, "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 打开/关闭串口的按钮事件
        /// </summary>
        private void btnSwitch_Click(object sender, EventArgs e)
        {
            if (!sp1.IsOpen)
            {
                try
                {
                    //设置串口号
                    string serialName = cbSerial.SelectedItem.ToString();
                    sp1.PortName = serialName;

                    //对串口参数进行设置
                    string strBaudRate = cbBaudRate.Text;
                    string strDateBits = cbDataBits.Text;
                    string strStopBits = cbStop.Text;

                    Int32 iBaudRate = Convert.ToInt32(strBaudRate);
                    Int32 iDateBits = Convert.ToInt32(strDateBits);

                    //设置波特率
                    sp1.BaudRate = iBaudRate;
                    //设置数据位
                    sp1.DataBits = iDateBits;
                    //设置停止位
                    switch (cbStop.Text)
                    {
                        case "1": sp1.StopBits = StopBits.One; break;
                        case "1.5": sp1.StopBits = StopBits.OnePointFive; break;
                        case "2": sp1.StopBits = StopBits.Two; break;                        
                        default:
                            MessageBox.Show("停止位参数不正确", "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                    }
                    //设置校验位
                    switch (cbParity.Text)
                    {
                        case "无": sp1.Parity = Parity.None; break;
                        case "奇校验": sp1.Parity = Parity.Odd; break;
                        case "偶校验": sp1.Parity = Parity.Even; break;
                        default:
                            MessageBox.Show("校验位参数不正确！", "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                    }

                    //如果串口为打开状态，就先关闭一下来初始化串口
                    if (sp1.IsOpen==true)
                    {
                        sp1.Close();
                    }

                    //状态栏设置
                    tsSpNum.Text = "串口号：" + sp1.PortName + "|";
                    tsBaudRate.Text = "波特率：" + sp1.BaudRate + "|";
                    tsDataBits.Text = "数据位：" + sp1.DataBits + "|";
                    tsStopBits.Text = "停止位：" + sp1.StopBits + "|";
                    tsParity.Text = "校验位：" + sp1.Parity;

                    //设置必要的控件为不可用状态
                    cbSerial.Enabled = false;
                    cbBaudRate.Enabled = false;
                    cbDataBits.Enabled = false;
                    cbStop.Enabled = false;
                    cbParity.Enabled = false;

                    //打开串口
                    sp1.Open();
                    //将按钮内容设置为"关闭串口"
                    btnSwitch.Text = "关闭串口";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误：" + ex.Message, "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timeSend.Enabled = false;
                    return;                    
                }
            }
            else
            {
                //状态栏设置
                tsSpNum.Text = "串口号：未指定|";
                tsBaudRate.Text = "波特率：未指定|";
                tsDataBits.Text = "数据位：未指定|";
                tsStopBits.Text = "停止位：未指定|";
                tsParity.Text = "校验位：未指定|";

                //恢复控件功能，设置必要控件为可用状态
                cbSerial.Enabled = true;
                cbBaudRate.Enabled = true;
                cbDataBits.Enabled = true;
                cbStop.Enabled = true;
                cbParity.Enabled = true;

                //关闭串口
                sp1.Close();

                //设置按钮内容为"打开串口"
                btnSwitch.Text = "打开串口";

                //关闭计时器Timer
                timeSend.Enabled = false;
            }
        }

        /// <summary>
        /// 程序窗口关闭时的事件
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //保存配置到INI文件，关闭打开的串口
            INIFILE.Profile.SaveProfile();
            sp1.Close();
        }


        /// <summary>
        /// "保存设置"按钮点击事件
        /// </summary>        
        private void btnSave_Click(object sender, EventArgs e)
        {
            //设置各"串口设置"
            string strBaudRate = cbBaudRate.Text;
            string strDataBits = cbDataBits.Text;
            string strStopBits = cbStop.Text;
            Int32 iBaudRate = Convert.ToInt32(strBaudRate);
            Int32 iDataBits = Convert.ToInt32(strDataBits);

            //保存波特率设置
            Profile.G_BAUDRATE = iBaudRate + "";
            //保存数据位设置
            Profile.G_DATABITS = iDataBits + "";
            //保存停止位设置
            switch (cbStop.Text)
            {
                case "1": Profile.G_STOP = "1"; break;
                case "1.5": Profile.G_STOP = "1.5"; break;
                case "2": Profile.G_STOP = "2"; break;
                default:
                    MessageBox.Show("停止位参数不正确", "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
            //保存校验位设置
            switch (cbParity.Text)
            {
                case "无": Profile.G_PARITY = "NONE"; break;
                case "奇校验": Profile.G_PARITY = "ODD"; break;
                case "偶校验": Profile.G_PARITY = "EVEN"; break;
                default:
                    MessageBox.Show("校验位参数不正确", "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }

            //保存设置
            Profile.SaveProfile();

            //保存成功提示
            MessageBox.Show("设置保存成功！","信息提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        /// <summary>
        /// 对发送框的输入进行校验
        /// </summary>
        private void txtSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (rbSend16.Checked == true)
            {
                //正则匹配,\b表示退格键（对16进制的输入进行格式校验）
                string pattern = "[0-9a-fA-F]|\b|0x|0X|,|，";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(e.KeyChar.ToString());

                if (match.Success)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = false;
            }
        }

        /// <summary>
        /// 对定时输入的时间进行校验
        /// </summary>
        private void txtSecond_KeyPress(object sender, KeyPressEventArgs e)
        {
            string pattern = "[0-9]|\b";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(e.KeyChar.ToString());

            if (match.Success)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
