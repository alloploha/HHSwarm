using HHSwarm.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Model.Widgets
{
    public abstract class Widget
    {
        public ushort ID;
        public ushort ParentID;
        public Coordinate Position;
        public Coordinate Size;

        public delegate void InitializationCompletedDelegate();
        public event InitializationCompletedDelegate InitializationCompleted;

        internal void OnInitializationCompleted()
        {
            InitializationCompleted?.Invoke();
        }
    }

    public abstract class Widget<TWidgetInfo> where TWidgetInfo : Widget
    {
        private Lazy<Task<TWidgetInfo>> LazyWidgetInfo;

        /// <summary>
        /// This is a thread-blocking property. It blocks until all <see cref="TWidgetInfo"/> data arrived and <see cref="WidgetInfoIsReady"/> called.
        /// Don't try to access this property from constructors, consider to put code into <see cref="InitializeWidget"/> which will be calles right before <see cref="WidgetInfoIsReady"/> is fired.
        /// </summary>
        public virtual TWidgetInfo WidgetInfo => LazyWidgetInfo.Value.Result;

        public delegate void WidgetInfoIsReadyDelegate(Widget<TWidgetInfo> widget);
        public event WidgetInfoIsReadyDelegate WidgetInfoIsReady;

        public Widget(Task<TWidgetInfo> widgetInfo)
        {
            LazyWidgetInfo = new Lazy<Task<TWidgetInfo>>(async () => await widgetInfo);
            widgetInfo.ContinueWith(t =>
            {
                InitializeWidget(true);
                WidgetInfoIsReady?.Invoke(this);
            }
            , TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        /// <summary>
        /// It is safe to access <see cref="WidgetInfo"/>  from this method.
        /// </summary>
        protected virtual void InitializeWidget(bool finalize)
        {
            FinalizeInitialization(finalize);
        }

        protected void FinalizeInitialization(bool finalize)
        {
            if (finalize)
                WidgetInfo.OnInitializationCompleted();
        }
    }
}
