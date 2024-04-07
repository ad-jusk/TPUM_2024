using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        private readonly SynchronizationContext synchronizationContext;

        public AsyncObservableCollection()
        {
            synchronizationContext = SynchronizationContext.Current;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs eventArgs)
        {
            if (SynchronizationContext.Current == synchronizationContext)
            {
                RaiseCollectionChanged(eventArgs);
            }
            else
            {
                synchronizationContext.Send(RaiseCollectionChanged, eventArgs);
            }
        }

        private void RaiseCollectionChanged(object param)
        {
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            if (SynchronizationContext.Current == synchronizationContext)
            {
                RaisePropertyChanged(eventArgs);
            }
            else
            {
                synchronizationContext.Send(RaisePropertyChanged, eventArgs);
            }
        }

        private void RaisePropertyChanged(object param)
        {
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }
    }
}
