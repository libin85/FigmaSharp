﻿using System;
using System.Collections.Generic;
using System.Reflection;
using AppKit;
using FigmaSharp.Converters;

namespace FigmaSharp
{
    public class FigmaDelegate : IFigmaDelegate
    {
        static readonly FigmaViewConverter[] figmaViewConverters = {
            new FigmaVectorViewConverter (),
            new FigmaFrameEntityConverter (),
            new FigmaTextConverter (),
            new FigmaVectorEntityConverter (),
            new FigmaRectangleVectorConverter (),
            new FigmaElipseConverter (),
            new FigmaLineConverter ()
        };

        static readonly FigmaCodePositionConverter positionConverter = new MacFigmaCodePositionConverter();
        static readonly FigmaCodeAddChildConverter addChildConverter = new MacFigmaCodeAddChildConverter();

        public bool IsYAxisFlipped => true;

        public IImageWrapper GetImage (string url)
        {
            var image = new NSImage(new Foundation.NSUrl(url));
            return new ImageWrapper(image);
        }

        public IImageWrapper GetImageFromManifest (Assembly assembly, string imageRef)
        {
            var assemblyImage = FigmaViewsHelper.GetManifestImageResource(assembly, string.Format("{0}.png", imageRef));
            return new ImageWrapper (assemblyImage);
        }

        public IImageWrapper GetImageFromFilePath(string filePath)
        {
           var image = new NSImage(filePath);
           return new ImageWrapper(image);
        }

        public IImageViewWrapper GetImageView(IImageWrapper imageWrapper)
        {
            var wrapper = new ImageViewWrapper(new NSImageView());
            wrapper.SetImage(imageWrapper);
            return wrapper;
        }

        public FigmaViewConverter[] GetFigmaConverters() => figmaViewConverters;

        public string GetFigmaFileContent(string file, string token) =>
             FigmaApiHelper.GetFigmaFileContent(file, token);

        public IViewWrapper CreateEmptyView() => new ViewWrapper();

        public FigmaResponse GetFigmaResponseFromContent(string template) =>
            FigmaApiHelper.GetFigmaResponseFromContent(template);

        public string GetManifestResource(Assembly assembly, string file) =>
            FigmaApiHelper.GetManifestResource(assembly, file);

        public void BeginInvoke(Action handler) => NSApplication.SharedApplication.InvokeOnMainThread(handler);

        public FigmaCodePositionConverter GetPositionConverter() => positionConverter;

        public FigmaCodeAddChildConverter GetAddChildConverter() => addChildConverter;
    }
}
