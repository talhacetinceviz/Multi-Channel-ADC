using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace ADC_Read_App
{
    public partial class Form1 : Form
    {
        byte[] readBuffer = new byte[1024];
        byte DAC_ValL = 0;
        byte DAC_ValH = 0;

        int bufferIndex = 0;
        int messageCounter = 0;

        List<byte> packetBuffer = new List<byte>();
        bool packetStarted = false;
        int expectedLength = -1;

        public Form1()
        {
            InitializeComponent();
            cmb_com.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_baud.DropDownStyle = ComboBoxStyle.DropDownList;
            txt_ADCRaw.ReadOnly = true;         txt_ADCRaw.Enabled = false;
            txt_RealVoltage.ReadOnly = true;    txt_RealVoltage.Enabled = false;
            btn_update_param.Enabled = false;   trackBar1.Enabled = false;
            btn_save_param.Enabled = false;     txt_offset.Enabled = false; txt_gain.Enabled = false;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000; // 1 saniyede bir
            timer1.Tick += Timer_ports_Tick;
            timer1.Start();

            timer2.Interval = 1000;
            timer2.Tick += Timer_checkPort_Tick;

            string[] ports = SerialPort.GetPortNames();
            cmb_com.Items.AddRange(ports);

            cmb_baud.Items.Add("1200");     //0
            cmb_baud.Items.Add("2400");     //1
            cmb_baud.Items.Add("4800");     //2
            cmb_baud.Items.Add("9600");     //3
            cmb_baud.Items.Add("14400");    //4
            cmb_baud.Items.Add("19200");    //5
            cmb_baud.Items.Add("28800");    //6
            cmb_baud.Items.Add("38400");    //7
            cmb_baud.Items.Add("57600");    //8
            cmb_baud.Items.Add("115200");   //9
            cmb_baud.Items.Add("230400");   //10
            cmb_baud.SelectedIndex = 9;

            if (cmb_com.Items.Count != 0) cmb_com.SelectedIndex = 0;
        }

        private void Timer_checkPort_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("CheckPort tick çalıştı.");
            string[] ports = SerialPort.GetPortNames();

            if (serialPort1.IsOpen == false)
            {
                try
                {
                    // Port gerçekten var mı?
                    bool portVar = SerialPort.GetPortNames().Contains(serialPort1.PortName);
                    if (!portVar)
                        throw new Exception("Port listede yok");

                    // Basit bir veri gönderme ile test et (UART üzerinden kontrol karakteri gibi)
                    serialPort1.WriteLine(""); // boş satır göndermek yeterli
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Port hatası: " + ex.Message);

                    // Port kopmuş → işlemleri yap
                    try { serialPort1.Close(); } catch { }
                    btn_connection.Text = "Connect";
                    lbl_status.Text = "Disconnected"; lbl_status.ForeColor = Color.DarkRed;
                    cmb_com.Enabled = true;
                    cmb_baud.Enabled = true;
                    timer2.Stop();
                    timer3.Stop();
                    MessageBox.Show("Seri port bağlantısı koptu.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Timer_ports_Tick(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();

            if (!ports.SequenceEqual(cmb_com.Items.Cast<string>().ToArray()))
            {
                cmb_com.Items.Clear();
                cmb_com.Items.AddRange(ports);
                if (cmb_com.Items.Count > 0 && cmb_com.SelectedIndex == -1)
                {
                    cmb_com.SelectedIndex = 0;
                }
            }
        }
        private void btn_connection_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                btn_connection.Text = "Connect";
                lbl_status.Text = "Disconnected"; lbl_status.ForeColor = Color.DarkRed;
                cmb_com.Enabled = true;
                cmb_baud.Enabled = true;
                btn_update_param.Enabled = false;
                btn_save_param.Enabled = false;
                trackBar1.Enabled = false;
                txt_offset.Enabled = false; txt_gain.Enabled = false;
                timer2.Stop(); // burada durdur
                timer3.Stop();
            }
            else
            {
                try
                {
                    serialPort1.PortName = cmb_com.SelectedItem.ToString();
                    serialPort1.BaudRate = int.Parse(cmb_baud.SelectedItem.ToString());

                    serialPort1.DataReceived -= serialPort1_DataReceived; // varsa önce kaldır
                    serialPort1.DataReceived += serialPort1_DataReceived;

                    serialPort1.Open();
                    btn_connection.Text = "Disconnect";
                    lbl_status.Text = "Connected"; lbl_status.ForeColor = Color.DarkGreen;
                    
                    timer3.Interval = 100; // 100 ms
                    timer3.Tick += Timer_Send_Query;
                    timer3.Start();

                    cmb_com.Enabled = false;
                    cmb_baud.Enabled = false;
                    btn_update_param.Enabled = true;
                    btn_save_param.Enabled = true;
                    trackBar1.Enabled = true;
                    txt_offset.Enabled = true; txt_gain.Enabled = true;

                    timer2.Start();
                    Console.WriteLine("timer_checkPort başlatıldı");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bağlantı hatası: " + ex.Message);
                }
            }
        }

        private void Timer_Send_Query(object sender, EventArgs e)
        {
            byte[] query = new byte[7];// { 0x48, 0x03, 0x00, 0x4B , 0x0A};
            byte crc_q = 0;
            query[0] = 0x48;        crc_q ^= query[0];
            query[1] = 0x03;        crc_q ^= query[1];
            query[2] = 0x02;        crc_q ^= query[2];
            query[3] = DAC_ValH;    crc_q ^= query[3];
            query[4] = DAC_ValL;    crc_q ^= query[4];
            query[5] = crc_q;
            query[6] = 0x0A;
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(query, 0, query.Length);
            }
            else
            {
                MessageBox.Show("Seri port açık değil!");
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int byteCount = serialPort1.BytesToRead;
            byte[] tempBuffer = new byte[byteCount];
            serialPort1.Read(tempBuffer, 0, byteCount);

            for (int i = 0; i < byteCount; i++)
            {
                ProcessIncomingByte(tempBuffer[i]);
            }
        }

        private void ProcessIncomingByte(byte incomingByte)
        {
            if (!packetStarted)
            {
                if (incomingByte == 0x48)
                {
                    Console.WriteLine("Correct Prefix");
                    packetBuffer.Clear();
                    packetBuffer.Add(incomingByte);
                    packetStarted = true;
                }
                return;
            }

            packetBuffer.Add(incomingByte);

            if (packetBuffer.Count == 3) // prefix, header, length
            {
                expectedLength = packetBuffer[2];
            }
            //Tottal Message Length = Message Lenght + prefix + header + length + bcc + endbit ===> (5)
            if (expectedLength != -1 && packetBuffer.Count == expectedLength + 5) 
            {
                // Tüm paket tamamlandı
                if (packetBuffer[packetBuffer.Count - 1] != 0x0A)
                {
                    ResetPacket(); Console.WriteLine("paket tamamlandı");
                    return;
                }

                // CRC hesapla
                byte bcc = 0x00;
                for (int i = 0; i < expectedLength +3; i++) // sadece message kısmı
                {
                    bcc ^= packetBuffer[i];
                }

                byte receivedCrc = packetBuffer[3 + expectedLength];
                if (bcc != receivedCrc)
                {
                    Console.WriteLine($"BCC hatası - Gelen: 0x{receivedCrc:X2}, Hesaplanan: 0x{bcc:X2}");
                    ResetPacket(); 
                    return;
                }

                // Paket geçerli → işleme al
                byte header = packetBuffer[1];
                byte[] message = packetBuffer.Skip(3).Take(expectedLength).ToArray();
                HandleMessage(header, message);

                ResetPacket();
            }
        }

        private void ResetPacket()
        {
            packetStarted = false;
            expectedLength = -1;
            packetBuffer.Clear();
        }

        private void HandleMessage(byte header, byte[] message)
        {
            switch (header)
            {
                case 0x01:
                    UInt32 Adc_Raw = (UInt32)(message[0] | (message[1]<<8) | (message[2] << 16) | (message[3] << 24));
                    UInt32 Adc_Raw2 = (UInt32)(message[4] | (message[5] << 8) | (message[6] << 16) | (message[7] << 24));
                    byte[] floatBytes = new byte[4] { message[8], message[9], message[10], message[11] };
                    float V_Real = BitConverter.ToSingle(floatBytes, 0);
                    byte[] floatBytes2 = new byte[4] { message[12], message[13], message[14], message[15] };
                    float Offset = BitConverter.ToSingle(floatBytes2, 0);
                    byte[] floatBytes3 = new byte[4] { message[16], message[17], message[18], message[19] };
                    float Gain = BitConverter.ToSingle(floatBytes3, 0);

                    Invoke(new Action(() =>
                    {
                        txt_ADCRaw.Text = Adc_Raw.ToString();
                        txt_ADC2.Text = Adc_Raw2.ToString();
                        txt_RealVoltage.Text = V_Real.ToString("F2").Replace('.', ','); // 12,03 formatında
                        txt_cur_offset.Text = Offset.ToString("F2").Replace('.', ','); // 12,03 formatında
                        txt_cur_gain.Text = Gain.ToString("F2").Replace('.', ','); // 12,03 formatında
                    }));

                    break;

                case 0x02:
                    // Sıcaklık, sistem durumu vs.
                    break;
                case 0x03:
                    // Sıcaklık, sistem durumu vs.
                    break;
                case 0x04:
                    // Sıcaklık, sistem durumu vs.
                    break;
                case 0x05:
                    // Sıcaklık, sistem durumu vs.
                    break;
                case 0x06:
                    // Sıcaklık, sistem durumu vs.
                    break;
                case 0x07:
                    // Sıcaklık, sistem durumu vs.
                    break;
                case 0x08:
                    // Sıcaklık, sistem durumu vs.
                    break;
                case 0x09:
                    // Sıcaklık, sistem durumu vs.
                    break;
                case 0x0A:
                    // Sıcaklık, sistem durumu vs.
                    break;
                default:
                    Console.WriteLine($"Bilinmeyen header: {header:X2}");
                    break;
            }
        }

        private void txt_offset_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Sadece rakam, backspace ve virgül/nokta kabul et
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
                return;
            }

            // Birden fazla nokta/virgül engelle
            var txt = sender as System.Windows.Forms.TextBox;
            if (txt != null)
            {
                string existingText = txt.Text;

                // Eğer seçili metin varsa, onu silmiş gibi düşünelim
                if (txt.SelectionLength > 0)
                {
                    existingText = existingText.Remove(txt.SelectionStart, txt.SelectionLength);
                }

                if ((e.KeyChar == '.' || e.KeyChar == ',') &&
                    (existingText.Contains(".") || existingText.Contains(",")))
                {
                    e.Handled = true;
                }
            }
        }

        private void txt_gain_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Sadece rakam, backspace ve virgül/nokta kabul et
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
                return;
            }

            // Birden fazla nokta/virgül engelle
            var txt = sender as System.Windows.Forms.TextBox;
            if (txt != null)
            {
                string existingText = txt.Text;

                // Eğer seçili metin varsa, onu silmiş gibi düşünelim
                if (txt.SelectionLength > 0)
                {
                    existingText = existingText.Remove(txt.SelectionStart, txt.SelectionLength);
                }

                if ((e.KeyChar == '.' || e.KeyChar == ',') &&
                    (existingText.Contains(".") || existingText.Contains(",")))
                {
                    e.Handled = true;
                }
            }
        }

        private void btn_update_param_Click(object sender, EventArgs e)
        {
            try
            {
                float offset = float.Parse(txt_offset.Text.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                float gain = float.Parse(txt_gain.Text.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);

                byte[] packet = CreateParamPacket(0x01, offset, gain);
                serialPort1.Write(packet, 0, packet.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hatalı giriş: " + ex.Message);
            }
        }

        private void btn_save_param_Click(object sender, EventArgs e)
        {
            try
            {
                float offset = float.Parse(txt_offset.Text.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                float gain = float.Parse(txt_gain.Text.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);

                byte[] packet = CreateParamPacket(0x02, offset, gain);
                serialPort1.Write(packet, 0, packet.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hatalı giriş: " + ex.Message);
            }
        }

        private byte[] CreateParamPacket(byte command, float offset, float gain)
        {
            byte[] data = new byte[13]; // Toplam 13 byte

            data[0] = 0x48; // Prefix
            data[1] = command; // Komut
            data[2] = 0x08; // Data uzunluğu 8 byte

            byte[] offsetBytes = BitConverter.GetBytes(offset);
            byte[] gainBytes = BitConverter.GetBytes(gain);

            // D1 (offset)
            data[3] = offsetBytes[0];
            data[4] = offsetBytes[1];
            data[5] = offsetBytes[2];
            data[6] = offsetBytes[3];

            // D2 (gain)
            data[7] = gainBytes[0];
            data[8] = gainBytes[1];
            data[9] = gainBytes[2];
            data[10] = gainBytes[3];

            // BCC hesapla (0–10 arası XOR)
            byte bcc = 0;
            for (int i = 0; i <= 10; i++)
                bcc ^= data[i];

            data[11] = bcc;

            data[12] = 0x0A; // End byte

            return data;
        }

        private void trackBar1_MouseLeave(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            lbl_dac.Text = Convert.ToString(trackBar1.Value);
            DAC_ValH = (byte)((trackBar1.Value >> 8) & 0x0F);
            DAC_ValL = (byte)(trackBar1.Value & 0xFF);
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            trackBar1.Value = 0;
            lbl_dac.Text = Convert.ToString(trackBar1.Value);
            DAC_ValH = (byte)((trackBar1.Value >> 8) & 0x0F);
            DAC_ValL = (byte)(trackBar1.Value & 0xFF);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            lbl_dac.Text = Convert.ToString(trackBar1.Value);
            DAC_ValH = (byte)((trackBar1.Value >> 8) & 0x0F);
            DAC_ValL = (byte)(trackBar1.Value & 0xFF);
        }
    }
}
