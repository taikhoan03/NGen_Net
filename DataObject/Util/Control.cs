using System;

namespace Crunch_DataObject.Util
{
    public class Control
    {
        /// <summary>
        /// Set default text cho Textbox Control
        /// </summary>
        /// <param name="textboxControl"></param>
        /// <param name="defaultText"></param>
        public static void SetTextboxDefaultText(System.Windows.Forms.TextBox textboxControl, string defaultText)
        {
            textboxControl.Tag = defaultText;
            textboxControl.Enter += new EventHandler(textBoxControl_SetTextboxDefaultText);
            textboxControl.Leave += new EventHandler(textBoxControl_SetTextboxDefaultText);
        }

        public static void textBoxControl_SetTextboxDefaultText(object sender, EventArgs e)
        {
            var textbox = (System.Windows.Forms.TextBox)sender;
            if (textbox.Text.Equals(textbox.Tag))
            {
                textbox.Text = string.Empty;
            }
            //else if (String.IsNullOrEmpty(textbox.Text) && !String.IsNullOrEmpty(textbox.Tag.ToString()))
            //{
            //    textbox.Text = textbox.Tag.ToString();
            //}
        }
    }
}