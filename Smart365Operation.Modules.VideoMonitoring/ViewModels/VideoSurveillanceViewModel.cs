using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Smart365Operation.Modules.VideoMonitoring.Models;
using Smart365Operation.Modules.VideoMonitoring.Services;
using Smart365Operation.Modules.VideoMonitoring.Utility;
using Prism.Events;

namespace Smart365Operation.Modules.VideoMonitoring.ViewModels
{
    public class VideoSurveillanceViewModel : BindableBase, IVideoService
    {
        private IEventAggregator _eventAggregator;

        public VideoSurveillanceViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            MakeRegions(Rows, Columns);
            _eventAggregator.GetEvent<PubSubEvent<string>>().Subscribe(s => Play(s));
        }

        private void MakeRegions(int row, int columns)
        {
            int index = 0;
            for (int rowIndex = 0; rowIndex < row; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columns; columnIndex++)
                {
                    if (Regions.Count < (row * columns))
                    {
                        if (Regions.Count <= index)
                        {
                            Regions.Add(new RegionInfo() { ColumnIndex = columnIndex, RowIndex = rowIndex, Index = index });
                        }
                    }
                    else if (Regions.Count > (row * columns))
                    {
                        int isDisplayingCount = 0;
                        for (int iNum = 0; iNum < Regions.Count; iNum++)
                        {
                            if (Regions[iNum].IsDisplaying)
                            {
                                isDisplayingCount++;
                            }
                        }

                        if (isDisplayingCount > (row * columns))
                        {
                            for (int iNum = row * columns; iNum < Regions.Count; iNum++)
                            {
                                if (Regions[iNum].IsDisplaying && Regions[iNum].SessionId != IntPtr.Zero)
                                {
                                    HkAction.Stop(Regions[iNum].SessionId);
                                }
                            }
                            for (int iNum = Regions.Count; iNum > row * columns; iNum--)
                            {
                                Regions.RemoveAt(iNum - 1);
                            }
                        }
                        else
                        {
                            for (int iNum = Regions.Count; iNum > row * columns; iNum--)
                            {
                                Regions.RemoveAt(iNum - 1);
                            }
                        }
                        return;
                    }
                    else
                    {
                        return;
                    }

                    index++;
                }
            }
        }

        private void ResetSelectedIndex()
        {
            if (SelectedIndex == 0)
            {
                foreach (var region in Regions)
                {
                    if (region.IsDisplaying && region.SessionId != IntPtr.Zero)
                    {
                        SelectedIndex++;
                    }
                }
            }

            if (SelectedIndex >= Regions.Count - 1)
                SelectedIndex = 0;
        }

        private int _rows = 1;
        public int Rows
        {
            get { return _rows; }
            set { SetProperty(ref _rows, value); }
        }

        private int _columns = 1;
        public int Columns
        {
            get { return _columns; }
            set { SetProperty(ref _columns, value); }
        }

        private ObservableCollection<RegionInfo> _regions = new ObservableCollection<RegionInfo>();
        public ObservableCollection<RegionInfo> Regions
        {
            get { return _regions; }
            set { SetProperty(ref _regions, value); }
        }

        private DisplayMode _selectedDisplayMode = DisplayMode.One;
        public DisplayMode SelectedDisplayMode
        {
            get { return _selectedDisplayMode; }
            set
            {
                if (value == DisplayMode.Stop)
                {
                    StopAll();
                }
                if (value != _selectedDisplayMode)
                {
                    UpdateDisplayRegions(value);
                }
                SetProperty(ref _selectedDisplayMode, value);
            }
        }

        private int _selectedIndex = 0;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }


        private DelegateCommand<object> _playVideoCommand;

        public DelegateCommand<object> PlayVideoCommand
        {
            get
            {
                if (_playVideoCommand == null)
                {
                    _playVideoCommand = new DelegateCommand<object>(PlayVideo, CanPlayVideo);
                }
                return _playVideoCommand;
            }
        }

        private void PlayVideo(object obj)
        {
            var cameraViewModel = obj as CameraViewModel;
            if (cameraViewModel != null)
                Play(cameraViewModel.CameraId);
        }

        private bool CanPlayVideo(object obj)
        {
            return true;
        }

        private void UpdateDisplayRegions(DisplayMode displayMode)
        {
           
            switch (displayMode)
            {
                case DisplayMode.One:
                    Rows = 1;
                    Columns = 1;
                    break;
                case DisplayMode.Two:
                    Rows = 2;
                    Columns = 1;
                    break;
                case DisplayMode.Four:
                    Rows = 2;
                    Columns = 2;
                    break;
                case DisplayMode.Six:
                    Rows = 2;
                    Columns = 3;
                    break;
                case DisplayMode.Nine:
                    Rows = 3;
                    Columns = 3;
                    break;
                default:
                    break;
            }
            MakeRegions(Rows, Columns);

            ResetSelectedIndex();
        }


        #region VideoPlay Impl
        public void Play(string cameraId)
        {
            var region = GetCurrentDisplayRegion();
            if (region.IsDisplaying && region.SessionId != IntPtr.Zero)
            {
                HkAction.Stop(region.SessionId);
            }
            region.SessionId = HkAction.AllocSession();
            if (region.SessionId != null && !string.IsNullOrEmpty(cameraId))
            {
                var playStatus = HkAction.Play(region.DisplayHandler, cameraId, region.SessionId);
                if (playStatus)
                {
                    region.IsDisplaying = true;
                    SelectedIndex = SelectedIndex >= Regions.Count - 1 ? 0 : (SelectedIndex + 1);
                }
            }
        }

        private RegionInfo GetCurrentDisplayRegion()
        {
            return Regions.FirstOrDefault(r => r.Index == SelectedIndex);
        }

        public void StopAll()
        {
            foreach (var region in Regions)
            {
                if (region.IsDisplaying && region.SessionId != IntPtr.Zero)
                {
                    HkAction.Stop(region.SessionId);
                }
            }
            SelectedIndex = 0;
        }
        #endregion
    }
}
