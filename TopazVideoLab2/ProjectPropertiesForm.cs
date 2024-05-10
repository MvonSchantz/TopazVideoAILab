using System.DirectoryServices.ActiveDirectory;
using System.Globalization;

namespace TopazVideoLab2
{
    public partial class ProjectPropertiesForm : Form
    {
        public ProjectPropertiesForm()
        {
            InitializeComponent();

            DeinterlaceDropDownList.SelectedIndex = 0;
        }

        private const float AspectMidPoint = ((4.0f / 3.0f) + (16.0f / 9.0f)) / 2.0f;

        public void GuessAspectRatioAndColorSpace()
        {
            int inputWidth = int.Parse(InputWidthTextBox.Text);
            int inputHeight = int.Parse(InputHeightTextBox.Text);
            float aspect = inputWidth / (float)inputHeight;
            if (aspect < AspectMidPoint)
            {
                AspectRatioDropdownList.SelectedIndex = 0; // 4:3
            }
            else
            {
                AspectRatioDropdownList.SelectedIndex = 1; // 16:9
            }

            if (inputWidth <= 768 && inputHeight <= 576)
            {
                ColorSpaceDropDownList.SelectedIndex = 0;
            }
            else
            {
                ColorSpaceDropDownList.SelectedIndex = 1;
            }
        }

        private bool disableResolutionUpdate = false;

        private void AspectRatioDropdownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int inputWidth = int.Parse(InputWidthTextBox.Text);
            int inputHeight = int.Parse(InputHeightTextBox.Text);

            disableResolutionUpdate = true;

            int width;
            int height;
            switch (AspectRatioDropdownList.SelectedIndex)
            {
                case 0: // 4:3
                    width = 960;
                    height = 720;
                    if (inputWidth * 2 > 1440 && inputHeight * 2 > 1080)
                    {
                        width = 1440;
                        height = 1080;
                    }
                    if (inputWidth * 2 > 1920 && inputHeight * 2 > 1400)
                    {
                        width = 1920;
                        height = 1440;
                    }
                    if (inputWidth * 2 > 2880 && inputHeight * 2 > 2160)
                    {
                        width = 2880;
                        height = 2160;
                    }
                    break;
                case 1: // 16:9
                    width = 1280;
                    height = 720;
                    if (inputWidth * 2 > 1920 && inputHeight * 2 > 1080)
                    {
                        width = 1920;
                        height = 1080;
                    }
                    if (inputWidth * 2 > 2560 && inputHeight * 2 > 1400)
                    {
                        width = 2560;
                        height = 1440;
                    }
                    if (inputWidth * 2 > 3840 && inputHeight * 2 > 2160)
                    {
                        width = 3840;
                        height = 2160;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            switch (height)
            {
                case 720:
                    ResolutionComboBox.SelectedIndex = 1;
                    break;
                case 1080:
                    ResolutionComboBox.SelectedIndex = 2;
                    break;
                case 1440:
                    ResolutionComboBox.SelectedIndex = 3;
                    break;
                case 2160:
                    ResolutionComboBox.SelectedIndex = 4;
                    break;
                default:
                    ResolutionComboBox.SelectedIndex = 0;
                    break;
            }

            OutputWidthTextBox.Text = width.ToString(NumberFormatInfo.InvariantInfo);
            OutputHeightTextBox.Text = height.ToString(NumberFormatInfo.InvariantInfo);

            disableResolutionUpdate = false;
        }

        private void OutputTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }

            if (!int.TryParse(textBox.Text, out _))
            {
                e.Cancel = true;
            }
        }

        private void ResolutionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (disableResolutionUpdate)
            {
                return;
            }

            switch (AspectRatioDropdownList.SelectedIndex)
            {
                case 0:
                    switch (ResolutionComboBox.SelectedIndex)
                    {
                        case 1:
                            OutputWidthTextBox.Text = "960";
                            OutputHeightTextBox.Text = "720";
                            break;
                        case 2:
                            OutputWidthTextBox.Text = "1440";
                            OutputHeightTextBox.Text = "1080";
                            break;
                        case 3:
                            OutputWidthTextBox.Text = "1920";
                            OutputHeightTextBox.Text = "1440";
                            break;
                        case 4:
                            OutputWidthTextBox.Text = "2880";
                            OutputHeightTextBox.Text = "2160";
                            break;
                    }
                    break;
                case 1:
                    switch (ResolutionComboBox.SelectedIndex)
                    {
                        case 1:
                            OutputWidthTextBox.Text = "1280";
                            OutputHeightTextBox.Text = "720";
                            break;
                        case 2:
                            OutputWidthTextBox.Text = "1920";
                            OutputHeightTextBox.Text = "1080";
                            break;
                        case 3:
                            OutputWidthTextBox.Text = "2560";
                            OutputHeightTextBox.Text = "1440";
                            break;
                        case 4:
                            OutputWidthTextBox.Text = "3840";
                            OutputHeightTextBox.Text = "2160";
                            break;
                    }
                    break;
            }
        }
    }
}


