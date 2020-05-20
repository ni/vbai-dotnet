//////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2019, National Instruments Corp.

// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:

// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NationalInstruments.Vision;
using NationalInstruments.VBAI;
using NationalInstruments.VBAI.Structures;
using NationalInstruments.VBAI.Enums;

namespace dotNET_API_Example
{
    public partial class Form1 : Form
    {
        VBAIEngine engine;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory + "\\..\\..\\..\\";
        }

        private void launch_Click(object sender, EventArgs e)
        {
            try
            {
                // Passing an emtpy string for the version will launch the latest Vision Builder AI engine available on the system.
                engine = new VBAIEngine("APITest", "", true);
                launch.Enabled = false;
                open.Enabled = true;
            }
            catch (VBAIException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void open_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = openFileDialog1.ShowDialog();
                if (dialogResult.ToString() == "OK")
                {
                    engine.OpenInspection(openFileDialog1.FileName);
                    InspectionInfo info = engine.GetInspectionInfo();
                    Path.Text = string.Copy(info.productName);
                    inspect.Enabled = true;
                }
            }
            catch(VBAIException ex)
            {
                Path.Text = String.Copy("");
                MessageBox.Show(ex.Message);
            }

        }

        private void inspect_Click(object sender, EventArgs e)
        {
            bool newImageAvailable, inspectionStatus;
            VBAIDateTime timeStamp;
            try
            {
                engine.RunInspectionOnce(-1);
                VisionImage image = engine.GetInspectionImage("", 1, 1, out newImageAvailable);
                imageViewer1.Attach(image);
                engine.GetInspectionResults(out timeStamp, out inspectionStatus);
                PassFailIndicator.Checked = inspectionStatus;
            }
            catch (VBAIException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void quit_Click(object sender, EventArgs e)
        {
            try
            {
                if (engine != null)
                    engine.CloseConnection(true);
            }
            catch (VBAIException ex)
            {
                MessageBox.Show(ex.Message);
            }
            Application.Exit();
        }
    }
}