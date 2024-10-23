using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DictionaryEditorV1
{
    public partial class MyNumericControl : TextBox
    {
        private int currentValue = 1;
        public MyNumericControl()
        {
            InitializeComponent();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
                e.Handled = !char.IsDigit(e.KeyChar); // && !char.IsControl(e.KeyChar);
            base.OnKeyPress(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.Text = currentValue.ToString("D4");
            }
            base.OnKeyDown(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            this.Text = currentValue.ToString("D4");
            base.OnLostFocus(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Text))
            {
                int temp = 0;
                bool res = int.TryParse(this.Text, out temp);
                if (res)
                {
                    currentValue = temp;
                    if (currentValue < 1) { currentValue = 1; }
                    if (currentValue > 9999) { currentValue = 9999; }
                }
            }

            base.OnTextChanged(e);
        }
        public void Increment()
        {
            currentValue++;
            if (currentValue > 9999)
            {
                currentValue = 9999;
            }
            this.Text = currentValue.ToString("D4");
        }

        public void Decrement()
        {
            currentValue--;
            if (currentValue < 1)
            {
                currentValue = 1;
            }
            this.Text = currentValue.ToString("D4");

        }

    }
}
