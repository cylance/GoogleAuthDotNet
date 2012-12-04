using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Cylance.Research.Tools.GAuthClient
{

    public partial class FormMain : Form
    {

        private class IdentityPanel : Panel
        {
        }

        public FormMain()
        {
            InitializeComponent();

            this.panelScrollable.SuspendLayout();
            this.tableLayoutPanelIdentities.SuspendLayout();

            this.panelScrollable.HorizontalScroll.Enabled = false;
            this.panelScrollable.HorizontalScroll.Visible = false;

            this.panelScrollable.VerticalScroll.Enabled = true;
            this.panelScrollable.VerticalScroll.SmallChange = 32;
            this.panelScrollable.VerticalScroll.Visible = true;

            this.tableLayoutPanelIdentities.RowCount = 0;
            this.tableLayoutPanelIdentities.RowStyles.Clear();
            this.tableLayoutPanelIdentities.Width =
                this.tableLayoutPanelIdentities.Parent.ClientRectangle.Width
                - this.tableLayoutPanelIdentities.Parent.Margin.Left
                - this.tableLayoutPanelIdentities.Parent.Margin.Right
                - this.tableLayoutPanelIdentities.Parent.Padding.Left
                - this.tableLayoutPanelIdentities.Parent.Padding.Right;

            this.tableLayoutPanelIdentities.ResumeLayout();
            this.panelScrollable.ResumeLayout();

            LoadOtpIdentities();

            PopulateIdentityPanels();
        }

        private Panel CreateIdentityPanel(string name)
        {
            Panel panel = new IdentityPanel();

            panel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            panel.BackColor = SystemColors.Window;
            panel.Height = 60;
            panel.BorderStyle = BorderStyle.Fixed3D;

            Label labelname = new Label();

            panel.Controls.Add(labelname);

            labelname.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            labelname.AutoEllipsis = true;
            labelname.BackColor = labelname.Parent.BackColor;
            labelname.Font = new Font(labelname.Font, FontStyle.Bold);
            labelname.Height = 16;
            labelname.Name = "labelName";
            labelname.Text = name;
            labelname.TextAlign = ContentAlignment.TopCenter;
            labelname.Top = 10;
            labelname.UseMnemonic = false;
            labelname.Width =
                labelname.Parent.ClientRectangle.Width
                - labelname.Parent.Padding.Left
                - labelname.Parent.Padding.Right;

            TextBox textotp = new TextBox();

            panel.Controls.Add(textotp);

            textotp.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textotp.BackColor = textotp.Parent.BackColor;       // .NET Forms bug: setting ForeColor doesn't work unless BackColor is also set
            textotp.BorderStyle = BorderStyle.None;
            textotp.Font = new Font("Arial", 18.0F);
            textotp.ForeColor = SystemColors.WindowText;
            textotp.Multiline = false;
            textotp.Name = "textOtp";
            textotp.ReadOnly = true;
            textotp.TabStop = false;
            textotp.TextAlign = HorizontalAlignment.Center;
            textotp.Top = labelname.Bottom;
            textotp.Width =
                textotp.Parent.ClientRectangle.Width
                - textotp.Parent.Padding.Left
                - textotp.Parent.Padding.Right;

            PictureBox picturedel = new PictureBox();

            panel.Controls.Add(picturedel);
            picturedel.BringToFront();

            picturedel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picturedel.Image = GAuthClient.Properties.Resources.iconDelete.ToBitmap();
            picturedel.Click += new EventHandler(this.pictureDelete_Click);
            picturedel.Name = "pictureDelete";
            picturedel.Size = new Size(8, 8);
            picturedel.Left = picturedel.Parent.ClientRectangle.Width - picturedel.Width - picturedel.Margin.Right;
            picturedel.Top = picturedel.Margin.Top;

            return panel;
        }

        private void AddIdentityPanel(Panel identityPanel)
        {
            int row = this.tableLayoutPanelIdentities.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            this.tableLayoutPanelIdentities.Controls.Add(identityPanel, 0, row);

            this.tableLayoutPanelIdentities.RowCount = (row + 1);
        }

        private void AddIdentityPanel(string name)
        {
            Panel identitypanel = CreateIdentityPanel(name);

            AddIdentityPanel(identitypanel);
        }

        private void PopulateIdentityPanels()
        {
            this.panelScrollable.SuspendLayout();
            this.tableLayoutPanelIdentities.SuspendLayout();

            this.tableLayoutPanelIdentities.Controls.Clear();
            this.tableLayoutPanelIdentities.RowCount = 0;
            this.tableLayoutPanelIdentities.RowStyles.Clear();

            foreach (OtpIdentity identity in this._OtpIdentities)
            {
                AddIdentityPanel(identity.Name);
            }

            UpdateOtp();

            this.tableLayoutPanelIdentities.ResumeLayout();
            this.panelScrollable.ResumeLayout();
        }

        // adapted from a great article at http://www.compuphase.com/cmetric.htm
        double ColorDistance(Color color1, Color color2)
        {
            double rmeanfactor = (color1.R + color2.R) / 512.0;

            return Math.Sqrt(
                ((2.0 + rmeanfactor) * Math.Pow(color1.R - color2.R, 2.0)) +
                ( 4.0                * Math.Pow(color1.G - color2.G, 2.0)) +
                ((3.0 - rmeanfactor) * Math.Pow(color1.B - color2.B, 2.0)) );
        }

        private void UpdateOtp()
        {
            Color color = SystemColors.WindowText;

            int remaining;
            GetCounter(out remaining);

            if (remaining <= 5)
            {
                if (ColorDistance(color, Color.Red) >= 240.0)   // choose red unless it's too similar to default text color
                    color = Color.Red;
                else color = Color.Yellow;
            }

            foreach (Control control in this.tableLayoutPanelIdentities.Controls)
            {
                if (!(control is IdentityPanel))
                    continue;

                int row = this.tableLayoutPanelIdentities.GetRow(control);
                if (row < 0 || row >= this._OtpIdentities.Count)
                    continue;

                TextBox textotp = (TextBox)((IdentityPanel)control).Controls["textOtp"];

                textotp.ForeColor = color;
                textotp.Text = CalculateOneTimePassword(this._OtpIdentities[row].Secret).ToString("000000");
            }
        }

        private static long GetCounter()
        {
            int n;
            return GetCounter(out n);
        }

        private static long GetCounter(out int secondsRemaining)
        {
            long l =
                Convert.ToInt64(
                    Math.Floor(
                        (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds));

            secondsRemaining = (30 - (int)(l % 30));
            return (l / 30);
        }

        private static int CalculateOneTimePassword(byte[] secretBytes)
        {
            byte[] data = BitConverter.GetBytes(GetCounter());
            Array.Reverse(data);

            byte[] hmac = new HMACSHA1(secretBytes).ComputeHash(data);

            int offset = hmac[hmac.Length - 1] & 0x0F;

            return
                (((hmac[offset + 0] & 0x7F) << 24) |
                 ( hmac[offset + 1]         << 16) |
                 ( hmac[offset + 2]         <<  8) |
                   hmac[offset + 3]) % 1000000;
        }

        //private const char[] Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();
        public static int Base32DecodeChar(char ch)
        {
            if (ch >= 'A' && ch <= 'Z')
                return (ch - 'A');
            else if (ch >= '2' && ch <= '7')
                return (ch - '2' + 26);
            else if (ch >= 'a' && ch <= 'z')
                return (ch - 'a');
            else return -1;
        }

        public static byte[] Base32DecodeString(string encoded)
        {
            int encodedbits = (encoded.Length * 5);
            int ofs = 0;

            byte[] output = new byte[((encoded.Length * 5) + 7) / 8];

            for (int i = 0, j = 0; i < encoded.Length; i++)
            {
                int chval = Base32DecodeChar(encoded[i]);
                if (chval < 0) return null;

                switch (ofs)
                {
                    case 0:
                        output[j]    = unchecked((byte)(chval << 3));           // bbbbb000
                        break;

                    case 5:
                        output[j++] |= unchecked((byte)(chval >> 2));           // .....bbb 00000000
                        output[j]    = unchecked((byte)(chval << 6));           // ........ bb000000
                        break;

                    case 2:
                        output[j]   |= unchecked((byte)(chval << 1));           // ........ ..bbbbb0
                        break;

                    case 7:
                        output[j++] |= unchecked((byte)(chval >> 4));           // ........ .......b
                        output[j]    = unchecked((byte)(chval << 4));           // ........ ........ bbbb0000
                        break;

                    case 4:
                        output[j++] |= unchecked((byte)(chval >> 1));           // ........ ........ ....bbbb
                        output[j]    = unchecked((byte)(chval << 7));           // ........ ........ ........ b0000000
                        break;

                    case 1:
                        output[j]   |= unchecked((byte)(chval << 2));           // ........ ........ ........ .bbbbb00
                        break;

                    case 6:
                        output[j++] |= unchecked((byte)(chval >> 3));           // ........ ........ ........ ......bb
                        output[j]    = unchecked((byte)(chval << 5));           // ........ ........ ........ ........ bbb00000
                        break;

                    case 3:
                        output[j++] |= unchecked((byte) chval);                 // ........ ........ ........ ........ ...bbbbb
                        break;
                } //switch

                ofs = ((ofs + 5) & 7);
            } //for

            return output;
        }

        private class OtpIdentity
        {
            public OtpIdentity(string name, byte[] secret)
            {
                this.Name = name;
                this.Secret = secret;
            }

            public string Name;
            public byte[] Secret;
        }

        private const string GAuthKey = @"Software\Cylance\GAuthClient";
        private const string OtpIdentitiesKey = GAuthKey + @"\Identities";

        private List<OtpIdentity> _OtpIdentities = new List<OtpIdentity>();

        private void AddOtpIdentity(string name, byte[] secret)
        {
            this._OtpIdentities.Add(
                new OtpIdentity(name, secret));
        }

        private void RemoveOtpIdentity(int index)
        {
            this._OtpIdentities.RemoveAt(index);
        }

        private void LoadOtpIdentities()
        {
            RegistryKey regkey;

            try
            {
                regkey = Registry.CurrentUser.OpenSubKey(FormMain.OtpIdentitiesKey);
                if (regkey == null) return;
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    this,
                    String.Format(
                        "An error occurred while loading saved identities.\r\n" +
                        "You may need to re-enter any names and secrets you previously saved.\r\n\r\n{0}",
                        e.ToString()),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            this._OtpIdentities.Clear();

            try
            {
                for (int i = 0; i < 1000; i++)
                {
                    try
                    {
                        object value = regkey.GetValue(i.ToString());

                        if (value == null)
                            break;
                        else if (!(value is byte[]))
                            continue;

                        byte[] input = (byte[])value;

                        if (input.Length <= 16 || input.Length > 256)
                            continue;

                        byte[] obfuskey = new Rfc2898DeriveBytes(
                            (((long)i * 214013) + 2531011).ToString(),
                            Encoding.ASCII.GetBytes(FormMain.OtpIdentitiesKey)
                        ).GetBytes(32);

                        Rijndael rijndael = RijndaelManaged.Create();

                        byte[] iv = new byte[rijndael.BlockSize / 8];
                        Array.Copy(
                            SHA256Managed.Create().ComputeHash(
                                Encoding.ASCII.GetBytes(FormMain.OtpIdentitiesKey)),
                            iv,
                            iv.Length);

                        byte[] output = RijndaelManaged.Create().CreateDecryptor(
                            obfuskey,
                            iv
                        ).TransformFinalBlock(
                            input, 0, input.Length);

                        if (output.Length <= 16 || output.Length > input.Length)
                            continue;

                        byte[] hash = MD5.Create().ComputeHash(output, 16, output.Length - 16);

                        if (hash.Length != 16)
                            continue;

                        int j;
                        for (j = 0; j < hash.Length; j++)
                            if (hash[j] != output[j]) break;

                        if (j < hash.Length) continue;

                        for (; j < output.Length; j++)
                            if (output[j] == 0) break;

                        if (!(j < output.Length)) continue;

                        string name = Encoding.UTF8.GetString(output, 16, j - 16);
                        if (name == null) continue;

                        j++;
                        byte[] secret = new byte[output.Length - j];
                        Array.Copy(output, j, secret, 0, output.Length - j);

                        AddOtpIdentity(name, secret);
                    }
                    catch
                    {
                    }
                } //for
            }
            finally
            {
                regkey.Close();
            }
        }

        private bool SaveOtpIdentities()
        {
            RegistryKey regkey;

            try
            {
                regkey = Registry.CurrentUser.CreateSubKey(FormMain.OtpIdentitiesKey);
                if (regkey == null) return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    this,
                    String.Format(
                        "An error occurred while saving identities.\r\n" +
                        "You may need to re-enter names and secrets the next time you restart the program.\r\n\r\n{0}",
                        e.ToString()),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            bool anysuccess = false;

            try
            {
                bool error = false;

                int i = 0;
                foreach (OtpIdentity identity in this._OtpIdentities)
                {
                    try
                    {
                        byte[] namebytes = Encoding.UTF8.GetBytes(identity.Name);
                        byte[] secret = identity.Secret;

                        byte[] input = new byte[16 + namebytes.Length + 1 + secret.Length];
                        Array.Copy(namebytes, 0, input, 16, namebytes.Length);
                        Array.Copy(secret, 0, input, 16 + namebytes.Length + 1, secret.Length);

                        byte[] hash = MD5.Create().ComputeHash(input, 16, input.Length - 16);

                        if (hash.Length != 16)
                        {
                            error = true;
                            continue;
                        }

                        Array.Copy(hash, input, hash.Length);

                        byte[] obfuskey = new Rfc2898DeriveBytes(
                            (((long)i * 214013) + 2531011).ToString(),
                            Encoding.ASCII.GetBytes(FormMain.OtpIdentitiesKey)
                        ).GetBytes(32);

                        Rijndael rijndael = RijndaelManaged.Create();

                        byte[] iv = new byte[rijndael.BlockSize / 8];
                        Array.Copy(
                            SHA256Managed.Create().ComputeHash(
                                Encoding.ASCII.GetBytes(FormMain.OtpIdentitiesKey)),
                            iv,
                            iv.Length);

                        byte[] output = RijndaelManaged.Create().CreateEncryptor(
                            obfuskey,
                            iv
                        ).TransformFinalBlock(
                            input, 0, input.Length);

                        regkey.SetValue(i.ToString(), output, RegistryValueKind.Binary);

                        i++;
                        anysuccess = true;
                    }
                    catch
                    {
                        error = true;
                    }
                } //foreach

                try
                {
                    string[] values = regkey.GetValueNames();

                    foreach (string name in values)             // delete any vestigial values in excess of the currently known identities
                    {
                        int n;
                        if (int.TryParse(name, out n) && n >= i)
                            regkey.DeleteValue(i.ToString(), false);
                    }

                    if (this._OtpIdentities.Count == 0)
                        anysuccess = true;
                }
                catch
                {
                }

                if (error)
                {
                    MessageBox.Show(
                        this,
                        "An error occurred while saving one or more identities.\r\n" +
                        "You may need to re-enter names and secrets the next time you restart the program.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            finally
            {
                regkey.Close();
            }

            return anysuccess;
        }

        private void buttonNewAdd_Click(object sender, EventArgs e)
        {
            string name = this.textBoxNewName.Text;
            byte[] secret = Base32DecodeString(this.textBoxNewSecret.Text);

            if (name == null || secret == null)
            {
                MessageBox.Show(
                    this,
                    "The secret entered is not valid.\r\n" +
                    "A secret may consist only of English letters (A through Z) and the numbers 2 through 7.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                this.textBoxNewSecret.Select();
                this.textBoxNewSecret.SelectAll();
                return;
            }

            this.panelScrollable.SuspendLayout();
            this.tableLayoutPanelIdentities.SuspendLayout();

            AddOtpIdentity(name, secret);

            AddIdentityPanel(this.textBoxNewName.Text);

            UpdateOtp();

            this.tableLayoutPanelIdentities.ResumeLayout();
            this.panelScrollable.ResumeLayout();

            if (!SaveOtpIdentities())
                return;

            this.textBoxNewName.Clear();
            this.textBoxNewSecret.Clear();

            this.textBoxNewName.Select();
        }

        private void pictureDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                this,
                "Do you want to permanently delete this identity?",
                "Confirm Delete",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            if (result != DialogResult.Yes)
                return;

            int row = this.tableLayoutPanelIdentities.GetRow(((Control)sender).Parent);
            if (row < 0 || row >= this._OtpIdentities.Count) return;

            this._OtpIdentities.RemoveAt(row);

            PopulateIdentityPanels();

            SaveOtpIdentities();
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            UpdateOtp();
        }

    } //class FormMain

} //namespace Cylance.Research.Tools.GAuthClient


/*
HMAC computation based on http://googleauthcsharp.codeplex.com/,
which is based on http://jsfiddle.net/russau/uRCTk/
and http://code.google.com/p/google-authenticator/ .


---- googleauthcsharp.codeplex.com copyright:

Copyright (c) 2011, Lars Truijens
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.

* Neither the name of Lars Truijens nor the names of its contributors may be
  used to endorse or promote products derived from this software without
  specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
