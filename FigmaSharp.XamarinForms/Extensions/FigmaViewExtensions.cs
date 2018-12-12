/* 
 * FigmaViewExtensions.cs 
 * 
 * Author:
 *   Javier Suárez <jsuarez@microsoft.com>
 *
 * Copyright (C) 2018 Microsoft, Corp
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit
 * persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
 * NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
 * USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using FigmaSharp.XamarinForms.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace FigmaSharp.XamarinForms.Extensions
{
    public static class FigmaViewExtensions
    {
        public static AbsoluteLayout ToXamarinFormsLayout(this FigmaNode parent, AbsoluteLayout parentView, FigmaNode child, List<FigmaImageView> figmaImageViews = null)
        {
            Console.WriteLine("[{0}({1})] Processing {2}..", child.id, child.name, child.GetType());

            if (child is IFigmaDocumentContainer instance && child is IConstraints instanceConstrains)
            {
                // Button
                if (child.name.Equals("button", StringComparison.InvariantCultureIgnoreCase) ||
                    child.name.Equals("button default", StringComparison.InvariantCultureIgnoreCase))
                {
                    var button = new Button();
                    parentView.Children.Add(button);
                    button.IsVisible = !child.visible;

                    if (instance.children.OfType<FigmaGroup>().Any())
                    {
                        button.Text = string.Empty;
                    }
                    else
                    {
                        var figmaText = instance.children.OfType<FigmaText>().FirstOrDefault();

                        if (figmaText != null)
                        {
                            button.Opacity = figmaText.opacity;
                            button.Text = figmaText.characters;
                        }

                        button.BackgroundColor = instance.backgroundColor.ToXamarinFormsColor();

                        return null;
                    }
                }

                // Entry
                if (child.name.Equals("text field", StringComparison.InvariantCultureIgnoreCase) ||
                    child.name.Equals("Field", StringComparison.InvariantCultureIgnoreCase))
                {
                    var textField = new Entry();
                    parentView.Children.Add(textField);

                    textField.IsVisible = !child.visible;
                    var figmaText = instance.children.OfType<FigmaText>()
                        .FirstOrDefault();

                    textField.Opacity = figmaText.opacity;
                    textField.Text = figmaText.characters;
                }
            }

            if (child.GetType() == typeof(FigmaVector))
            {
                Console.WriteLine("Not implemented {0}", child.name);
            }
            else if (child.GetType() == typeof(FigmaInstance))
            {
                Console.WriteLine("Not implemented {0}", child.name);
            }
            // Frame
            else if (child is FigmaFrameEntity figmaFrameEntity)
            {
                var currengroupView = new Frame
                {
                    IsVisible = !child.visible,
                    Opacity = figmaFrameEntity.opacity,
                    BackgroundColor = figmaFrameEntity.backgroundColor.ToXamarinFormsColor()
                };

                parentView.Children.Add(currengroupView);

                Console.WriteLine(figmaFrameEntity);
            }
            // Label
            else if (child.GetType() == typeof(FigmaText))
            {
                var text = (FigmaText)child;

                var label = new Label
                {
                    HorizontalTextAlignment = text.style.textAlignHorizontal == "CENTER" ? TextAlignment.Center : text.style.textAlignHorizontal == "LEFT" ? TextAlignment.Start : TextAlignment.End,
                    Opacity = text.opacity,
                    IsVisible = !child.visible
                };

                var fills = text.fills.FirstOrDefault();

                if (fills != null)
                {
                    label.TextColor = fills.color.ToXamarinFormsColor();
                }

                parentView.Children.Add(label);

                Console.WriteLine(text);
            }
            // BoxView
            else if (child.GetType() == typeof(FigmaRectangleVector))
            {
                var rectangleVector = (FigmaVectorEntity)child;

                BoxView currengroupView = new BoxView
                {
                    IsVisible = child.visible,
                    Opacity = rectangleVector.opacity
                };

                if (child is FigmaRectangleVector vector)
                {
                    currengroupView.CornerRadius = vector.cornerRadius;
                }

                var fills = rectangleVector.fills.FirstOrDefault();

                if (fills?.color != null)
                {
                    currengroupView.BackgroundColor = fills.color.ToXamarinFormsColor();
                }
            }
            else
            {
                Console.WriteLine("[{1}({2})] Not implemented: {0}", child.GetType(), child.id, child.name);
            }

            return null;
        }
    }
}