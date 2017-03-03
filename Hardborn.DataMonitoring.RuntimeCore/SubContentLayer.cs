using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Hardborn.DataMonitoring.RuntimeCore
{
    public class SubContentLayer : Grid, ISupportInitiazlie, IContentContainer
    {
        // Fields
        private Canvas _canvas;
        private string title = "Layer";

        // Methods
        public SubContentLayer()
        {
            base.HorizontalAlignment = HorizontalAlignment.Stretch;
            base.VerticalAlignment = VerticalAlignment.Stretch;
        }

        public void AddChild(UIElement element)
        {
            if (this._canvas == null)
            {
                this.Initialize();
            }
            this._canvas.Children.Add(element);
        }

        public void Initialize()
        {
            if (base.Children.Count == 0)
            {
                this._canvas = new Canvas();
                this._canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
                this._canvas.VerticalAlignment = VerticalAlignment.Stretch;
                base.Children.Add(this._canvas);
            }
            else
            {
                foreach (UIElement element in base.Children)
                {
                    if (element is Canvas)
                    {
                        this._canvas = element as Canvas;
                        break;
                    }
                }
                if (this._canvas == null)
                {
                    this._canvas = new Canvas();
                    this._canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
                    this._canvas.VerticalAlignment = VerticalAlignment.Stretch;
                    base.Children.Add(this._canvas);
                }
            }
        }

        public void LoadCanvas(Canvas canvas)
        {
            if (this._canvas == null)
            {
                this.Initialize();
            }
            UIElementCollection children = canvas.Children;
            canvas.Children.Clear();
            foreach (UIElement element in children)
            {
                this._canvas.Children.Add(element);
            }
        }

        // Properties
        public Canvas Container
        {
            get
            {
                return this._canvas;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }
    }


}
