﻿using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows.Input;

namespace native.Behaviours
{
    public class TextBoxBindingUpdateOnEnterBehaviour : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.KeyDown += OnTextBoxKeyDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.KeyDown -= OnTextBoxKeyDown;
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var txtBox = sender as TextBox;
                txtBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }
    }
}